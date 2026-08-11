namespace GHM.Infrastructure.IServices
{
    public interface IResourceService<T> where T : class

    {
        string GetString(string key);

        string GetString(string key, params object[] arguments);
    }
}
