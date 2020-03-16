namespace StudentManager.Repositories
{
    using StudentManagerData;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IStudentsRepository
    {
        Task<IEnumerable<Student>> GetAllUserStudentsAsync(Guid userId);

        Task DeleteStudentAsync(Guid id);

        Task<Student> UpdateStudentAsync(Guid id, Student student);

        Task<Student> AddStudentAsync(Guid userId, Student student);

        Task<Student> GetStudentAsync(Guid id);

        Task<int> GetAllStudentsCount(Guid userId);
    }
}
