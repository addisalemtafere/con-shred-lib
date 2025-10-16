namespace Convex.Shared.Http.EntityFramework
{
    public interface IConnectionStrings
    {
        string this[string key] { get; set; }
    }
}
