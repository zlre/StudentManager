namespace StudentManager.Repositories
{
    using StudentManagerData;
    using System;
    using System.Threading.Tasks;

    public interface IUsersRepository
    {
        Task<User> GetUserAsync(Guid id);

        Task<User> GetUserAsync(string login, string password);

        Task<bool> AddUserAsync(LoginCredentials credentials);

        Task<Guid> GetUsersIdAsync(string login);
    }
}
