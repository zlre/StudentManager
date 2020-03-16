namespace StudentManager.Helpers
{
    //выделена в отдельный интерфейс потому что используется как при регистрации так и при аутентификации
    public interface IHashFunction
    {
        string Hash(byte[] salt, string password);
    }
}
