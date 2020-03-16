namespace StudentManager
{
    using System.ComponentModel.DataAnnotations;

    public class LoginCredentials
    {
        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
