namespace TempIsolated.Core
{
    public interface IStorage
    {
        User LoadUser();

        void SaveUser(User user);
    }
}
