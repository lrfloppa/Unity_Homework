public interface ISingletone<TService>
    where TService : class
{
    public static TService Instance { get; }
}