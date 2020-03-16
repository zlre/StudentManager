namespace StudentManager.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using StudentManagerData;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class StudentsRepository : IStudentsRepository
    {
        private readonly StudentManagerDBContext _db;

        public StudentsRepository(StudentManagerDBContext db)
        {
            _db = db;
        }

        public async Task<int> GetAllStudentsCount(Guid userId) 
        {
            return await _db.UserStudents.Where(us => us.UserId == userId).CountAsync();
        }

        public async Task<Student> AddStudentAsync(Guid userId, Student newStudent)
        {
            Student studentFullEqual = await _db.Students.Where(
                s => s.Name == newStudent.Name &&
                     s.Patronymic == newStudent.Patronymic &&
                     s.Sex == newStudent.Sex &&
                     s.Surname == newStudent.Surname &&
                     s.StudentID == newStudent.StudentID).FirstOrDefaultAsync();

            Student studentWithEqualStudentId = null;

            if (newStudent.StudentID != null) {
                studentWithEqualStudentId = await _db.Students.Where(s => s.StudentID == newStudent.StudentID).FirstOrDefaultAsync();
            }

            if (studentFullEqual == null && studentWithEqualStudentId != null)
            {
                return null;
            }

            UserStudent currentUserStudentDependence = null;

            if (studentFullEqual != null)
            {
                currentUserStudentDependence = await _db.UserStudents.Where(us => us.UserId == userId && us.StudentId == studentFullEqual.Id).FirstOrDefaultAsync();
            }

            if (currentUserStudentDependence != null && studentFullEqual != null)
            {
                return studentFullEqual;
            }

            if (studentFullEqual == null)
            {
                studentFullEqual = new Student() {
                    Id = Guid.NewGuid(), 
                    Name = newStudent.Name, 
                    Patronymic = newStudent.Patronymic, 
                    Sex = newStudent.Sex, 
                    StudentID = newStudent.StudentID, 
                    Surname = newStudent.Surname 
                };
                _db.Students.Add(studentFullEqual);
            }

            _db.UserStudents.Add(new UserStudent() { StudentId = studentFullEqual.Id, UserId = userId });

            await _db.SaveChangesAsync();

            return studentFullEqual;
        }

        public async Task DeleteStudentAsync(Guid id)
        {
            Student student = new Student() { Id = id };
            _db.Students.Attach(student);
            _db.Students.Remove(student);
            await _db.SaveChangesAsync();
         }

        public async Task<Student> UpdateStudentAsync(Guid id, Student newStudent)
        {
            Student student = null;

            if (id != newStudent.Id)
            {
                student = await GetStudentAsync(id);

                student.Name = newStudent.Name;
                student.Patronymic = newStudent.Patronymic;
                student.Sex = newStudent.Sex;
                student.StudentID = newStudent.StudentID;
                student.Surname = newStudent.Surname;

                _db.Students.Update(student);
            }
            else
            {
                _db.Students.Update(newStudent);
                student = newStudent;
            }
            
            await _db.SaveChangesAsync();

            return student;
        }

        public async Task<IEnumerable<Student>> GetAllUserStudentsAsync(Guid userId)
        {
            var students = await _db.UserStudents.Include(i => i.Student).Where(us => us.UserId == userId).Select(s => s.Student).ToListAsync();
            return students;
        }

        public async Task<Student> GetStudentAsync(Guid id)
        {
            return await _db.Students.Where(s => s.Id == id).FirstOrDefaultAsync();
        }
    }
}
