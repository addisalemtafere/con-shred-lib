using System.Reflection;

namespace Convex.Shared.Common.Enums
{
    public abstract class Enumeration<TEnum> : IEquatable<Enumeration<TEnum>>, IComparable<Enumeration<TEnum>>
     where TEnum : Enumeration<TEnum>
    {

        private static readonly Lazy<Dictionary<int, TEnum>> EnumerationsDictionary = new(() => CreateEnumerationDictionary(typeof(TEnum)));

        protected Enumeration(int id, string name)
        {
            Id = id;
            Name = name;
        }

        protected Enumeration()
        {
            Id = default;
            Name = string.Empty;
        }

        public int Id { get; protected init; }
        public string Name { get; protected init; } = string.Empty;

        public static TEnum? FromId(int id)
        {
            bool isValueInDictionary = EnumerationsDictionary
                .Value
                .TryGetValue(id, out TEnum? enumeration);

            return isValueInDictionary
                ? enumeration
                : default;
        }

        public static TEnum? FromName(string name)
        {
            return EnumerationsDictionary
                .Value
                .Values
                .SingleOrDefault(x => x.Name == name);
        }

        public bool Equals(Enumeration<TEnum>? other)
        {
            if (other is null)
            {
                return false;
            }

            return GetType() == other.GetType() &&
                Id == other.Id;
        }

        public override bool Equals(object? obj)
        {
            return obj is Enumeration<TEnum> other &&
                Equals(other);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }

        private static Dictionary<int, TEnum> CreateEnumerationDictionary(Type enumType)
        {
            return GetFieldsForType(enumType)
                .ToDictionary(t => t.Id);
        }

        private static IEnumerable<TEnum> GetFieldsForType(Type enumType)
        {
            return enumType
                .GetFields(
                    BindingFlags.Public |
                    BindingFlags.Static |
                    BindingFlags.FlattenHierarchy)
                .Where(fieldInfo => enumType.IsAssignableFrom(fieldInfo.FieldType))
                .Select(fieldInfo => (TEnum)fieldInfo.GetValue(default)!);
        }

        public int CompareTo(Enumeration<TEnum>? other)
        {
            return other is null
                ? 1
                : Id.CompareTo(other.Id);
        }
    }
}
