namespace StudentManagerData
{
    using System;
    
    public class UserStudent
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid StudentId { get; set; }
        public Student Student { get; set; }
    }
}
