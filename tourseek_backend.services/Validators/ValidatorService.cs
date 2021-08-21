using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using tourseek_backend.domain.Models;
using tourseek_backend.repository.UnitOfWork;
using tourseek_backend.util.Extensions;

namespace tourseek_backend.services.Validators
{
    public class ValidatorService 

    {
        protected readonly IUnitOfWork UnitOfWork;
        protected bool MandatoryCheckFlag { get; set; }
        protected string ActionName { get; set; }
        public IDictionary<string, string> ValidationErrors { get; }
        public bool IsValid => ValidationErrors.Count == 0;

        public ValidatorService(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
            ValidationErrors = new Dictionary<string, string>();
        }

        public void AddValidationError(string name, string error)
        {
            ValidationErrors[name] = error;
        }

        public bool EnsuresNotExist<TSource, TEntity>(Attribute<TSource> attribute,
            Expression<Func<TEntity, bool>> filter) where TEntity : class
        {
            int count = UnitOfWork.Repository<TEntity>().GetCountByFilter(filter);

            if (count > 0)
            {
                attribute.ValidationError = $"{attribute.Name} is already existing";
                AddValidationError(attribute.Name, attribute.ValidationError);
            }

            return true;
        }

        public bool EnsuresExist<TSource, TEntity>(Attribute<TSource> attribute,
            Expression<Func<TEntity, bool>> filter) where TEntity : class
        {
            int count = UnitOfWork.Repository<TEntity>().GetCountByFilter(filter);

            if (count == 0)
            {
                attribute.ValidationError = $"{attribute.Name} does not exist";
                AddValidationError(attribute.Name, attribute.ValidationError);
            }

            return true;
        }

        public TResult GetIfExists<TSource, TResult, TEntity>(
            Attribute<TSource> attribute,
            Expression<Func<TEntity, bool>> filter,
            Expression<Func<TEntity, TResult>> selector
        ) where TEntity : class
        {
            int count = UnitOfWork.Repository<TEntity>().GetCountByFilter(filter);
            if (count == 0)
            {
                attribute.ValidationError = $"{attribute.Name} is not found";
                AddValidationError(attribute.Name, attribute.ValidationError);
            }
            else
            {
                return UnitOfWork.Repository<TEntity>().GetResultByFilter(filter, selector);
            }

            return default;
        }

        public virtual TResult GetIfSingleExists<TSource, TResult, TEntity>(
            Attribute<TSource> attribute,
            Expression<Func<TEntity, bool>> filter,
            Expression<Func<TEntity, TResult>> selector
        ) where TEntity : class
        {
            int count = UnitOfWork.Repository<TEntity>().GetCountByFilter(filter);
            if (count == 0)
            {
                attribute.ValidationError = $"{attribute.Name} is not found";
                AddValidationError(attribute.Name, attribute.ValidationError);
            }
            else if (count > 1)
            {
                attribute.ValidationError = $"{attribute.Name} has multiple records";
                AddValidationError(attribute.Name, attribute.ValidationError);
            }
            else
            {
                return UnitOfWork.Repository<TEntity>().GetResultByFilter(filter, selector);
            }

            return default;
        }

        private bool IsEmpty<TValue>(TValue value)
        {
            if (value == null)
                return true;

            if (typeof(TValue) == typeof(string))
            {
                return string.IsNullOrWhiteSpace(value.ToString());
            }

            return false;
        }

        public bool IsEmpty<TValue>(Attribute<TValue> attribute, bool isMandatory)
        {
            attribute.IsEmpty = IsEmpty(attribute.Value);
            if (attribute.IsEmpty && isMandatory)
            {
                attribute.ValidationError = $"{attribute.Name} is empty";
                AddValidationError(attribute.Name, attribute.ValidationError);
            }

            return attribute.IsEmpty;
        }

        public bool IsEmpty<TValue>(Attribute<TValue> attribute)
        {
            return IsEmpty(attribute, MandatoryCheckFlag);
        }

        public bool OneOf<TValue>(bool isMandatory, params Attribute<TValue>[] attributes)
        {
            if (attributes.Length < 2)
                throw new ArgumentException();

            int emptyCount = 0;
            foreach (var attribute in attributes)
            {
                attribute.IsEmpty = IsEmpty(attribute.Value);
                emptyCount += attribute.IsEmpty ? 1 : 0;
            }

            if (!isMandatory && emptyCount == attributes.Length)
            {
                return false;
            }

            if (emptyCount != attributes.Length - 1)
            {
                string columnsFormatted = string.Join("/", attributes.Select(c => c.Name).ToList());
                foreach (var attribute in attributes)
                {
                    attribute.ValidationError = $"{ActionName} using either {columnsFormatted} separately";
                    AddValidationError(attribute.Name, attribute.ValidationError);
                }

                return false;
            }

            return true;
        }

        public bool ValidateFormat(Attribute<string> attribute, StringFormat format)
        {
            IsEmpty(attribute);
            if (attribute.Value.ValidateFormat(format))
                return true;

            attribute.ValidationError = $"{attribute.Name} format should be {format.ReadableFormat}";
            AddValidationError(attribute.Name, attribute.ValidationError);

            return false;
        }

        public bool? ValidatePositiveInteger(Attribute<int> attribute)
        {
            if (attribute.ValidValueOrDefault > 0)
            {
                attribute.ValidationError = $"{attribute.Name} should be more than zero";
                AddValidationError(attribute.Name, attribute.ValidationError);
            }

            return null;
        }

        public bool? ValidatePositiveDouble(Attribute<double> attribute)
        {
            if (attribute.ValidValueOrDefault > 0)
            {
                attribute.ValidationError = $"{attribute.Name} should be more than zero";
                AddValidationError(attribute.Name, attribute.ValidationError);
            }

            return null;
        }

        public bool? ValidatePositiveDecimal(Attribute<decimal> attribute)
        {
            if (attribute.ValidValueOrDefault > 0)
            {
                attribute.ValidationError = $"{attribute.Name} should be more than zero";
                AddValidationError(attribute.Name, attribute.ValidationError);
            }

            return null;
        }
    }
}