namespace StudentManager
{
    using System;
    
    public class AuthenticatedUser
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string Token { get; set; }
    }
}
