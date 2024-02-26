namespace EntityManagementAPI
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> OrderByProperty<T>(this IEnumerable<T> source, string propertyName, string sortOrder)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                return source;

            if (!typeof(T).GetProperties().Any(p => string.Equals(p.Name, propertyName, StringComparison.OrdinalIgnoreCase)))
                throw new ArgumentException($"Property '{propertyName}' not found on type '{typeof(T)}'.", nameof(propertyName));

            return sortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase) ?
                source.OrderByDescending(x => typeof(T).GetProperty(propertyName, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).GetValue(x, null)) :
                source.OrderBy(x => typeof(T).GetProperty(propertyName, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).GetValue(x, null));
        }
    }
}
