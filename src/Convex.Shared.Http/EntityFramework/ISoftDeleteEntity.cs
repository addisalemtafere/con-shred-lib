namespace Convex.Shared.Http.EntityFramework
{
    public interface ISoftDeleteEntity
    {
        public bool IsDeleted { get; set; }
    }
}
