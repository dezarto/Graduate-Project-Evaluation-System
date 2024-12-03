namespace GPESAPI.Application.Services
{
    public static class LinqExtensions
    {
        // Bu metod, IEnumerable<T> tipine bir uzantıdır ve iki elemandan kombinasyonları döndürür
        public static IEnumerable<IEnumerable<T>> Combinations<T>(this IEnumerable<T> source, int length)
        {
            if (length == 1)
                return source.Select(t => new T[] { t });

            return Combinations(source, length - 1)
                .SelectMany(t => source.Where(e => !t.Contains(e)),
                            (t, e) => t.Concat(new T[] { e }));
        }
    }

}
