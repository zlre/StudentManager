namespace StudentManager.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using StudentManager.Helpers;
    using StudentManagerData;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class UsersRepository : IUsersRepository
    {
        private readonly StudentManagerDBContext _db;
        private readonly IHashFunction _hash;

        public UsersRepository(StudentManagerDBContext db, IHashFunction hash)
        {
            _db = db;
            _hash = hash;
        }
        
        public async Task<bool> AddUserAsync(LoginCredentials credentials)
        {
            Guid id = await GetUsersIdAsync(credentials.Login);
            if (id != default(Guid))
            {
                return false;
            }

            id = Guid.NewGuid();
            //солью для хэширования пароля является id пользователя
            var user = new User() { Id = id, Login = credentials.Login, Password = _hash.Hash(id.ToByteArray(), credentials.Password) };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return true;
        }

        public Task<User> GetUserAsync(Guid id)
        {
            return _db.Users.FirstOrDefaultAsync(c => c.Id == id);
        }        
        
        public Task<User> GetUserAsync(string login, string password)
        {
            return _db.Users.FirstOrDefaultAsync(c => c.Login == login && c.Password == password);
        }

        public Task<Guid> GetUsersIdAsync(string login)
        {
            return _db.Users.Where(x => x.Login == login).Select(s => s.Id).FirstOrDefaultAsync();
        }
    }
}
