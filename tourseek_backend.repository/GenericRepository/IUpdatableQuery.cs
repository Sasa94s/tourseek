namespace tourseek_backend.repository.GenericRepository
{
    public interface IUpdatableQuery<in TSource, TDestination, in TFilter, out TResult>
        : IFilterableQuery<TDestination, TFilter>
        where TSource : class
        where TDestination : class
        where TFilter : class
    {
        TResult BeforeUpdateSelector(TDestination entity);
        void UpdateActionHandler(TSource source, TDestination destination, bool isSingleUpdate = false);
    }
}