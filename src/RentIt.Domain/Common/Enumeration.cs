namespace RentIt.Domain.Common;
public abstract class Enumeration(int id, string name) : IComparable
{
    public int Id { get; } = id;
    public string Name { get; } = name;

    public override string ToString() => Name;

    public override bool Equals(object? obj)
    {
        if (obj is not Enumeration otherValue)
            return false;

        return Id.Equals(otherValue.Id);
    }

    public override int GetHashCode() => Id.GetHashCode();

    public int CompareTo(object? other) => Id.CompareTo(((Enumeration)other!).Id);

    public static IEnumerable<T> GetAll<T>() where T : Enumeration
    {
        var type = typeof(T);
        var fields = type.GetFields(System.Reflection.BindingFlags.Public |
                                    System.Reflection.BindingFlags.Static |
                                    System.Reflection.BindingFlags.DeclaredOnly);
        return fields.Select(f => f.GetValue(null)).Cast<T>();
    }
}

