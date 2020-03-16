namespace StudentManagerData
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public enum Sex 
    { 
        Man,
        Woman
    }
    
    public class Student
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Не указан пол студента")]
        public Sex Sex { get; set; }

        [Required(ErrorMessage = "Не указано имя студента")]
        [StringLength(40, ErrorMessage = "Максимальная длина имени 60 символов")]
        public string Name { get; set; }

        [StringLength(60, ErrorMessage = "Максимальная длина отчества 60 символов")]
        public string Patronymic { get; set; }

        [Required(ErrorMessage = "Не указана фамилия студента")]
        [StringLength(40, ErrorMessage = "Максимальная длина фамилии 60 символов")]
        public string Surname { get; set; }

        [StringLength(16, MinimumLength = 6, ErrorMessage = "Максимальная длина идентификатора 16 символов, минимальная 6")]
        public string StudentID { get; set; }

        public ICollection<UserStudent> UserStudents { get; set; }
    }
}
