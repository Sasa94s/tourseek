using tourseek_backend.util.Extensions;

namespace tourseek_backend.domain.Models
{
    public class Attribute<TValue>
    {
        private string _name;
        public bool IsValid => !IsEmpty && string.IsNullOrWhiteSpace(ValidationError ?? "");
        public bool IsEmpty { get; set; }
        public string ValidationError { get; set; }

        public string Name
        {
            get => _name;
            private set => _name = value.ToLowerFirstChar();
        }

        public TValue Value { get; set; }
        public TValue ValidValueOrDefault => IsValid ? Value : default;

        public static Attribute<TValue> Init(string name, TValue value)
        {
            return new Attribute<TValue> {Name = name, Value = value};
        }

        public static Attribute<TValue> Init(string name)
        {
            return new Attribute<TValue> {Name = name};
        }
    }
}