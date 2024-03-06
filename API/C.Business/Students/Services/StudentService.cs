using A.Contracts.Update_Models;
using Business.Students.Repositories;
using Contracts.Models;
using Microsoft.AspNetCore.JsonPatch;
using NotificationApi.Contracts.Events;

namespace Business.Students.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentDataAccess;

        public StudentService(IStudentRepository studentDataAccess)
        {
            _studentDataAccess = studentDataAccess;
        }

        public async Task CreateNewStudent(StudentModel student)
        {
            await _studentDataAccess.CreateNewStudent(student);
            return;
        }

        public async Task<bool> DeleteAllStudent()
        {
            return await _studentDataAccess.DeleteAllStudent();
        }

        public async Task<bool> DeleteStudent(string username)
        {
            return await _studentDataAccess.DeleteStudent(username);
        }

        public async Task<StudentModel> GetStudent(string username)
        {
            StudentModel studentModel = await _studentDataAccess.GetStudent(username);
            return studentModel;
        }

        public async Task<List<StudentModel>> GetStudents(int pageNumber, int itemPerPage, string university, string department)
        {
            return await _studentDataAccess.GetStudents(pageNumber, itemPerPage, university, department);
        }

        public async Task<bool> PartiallyUpdateStudent(string username, JsonPatchDocument<StudentModel> patchDocument)
        {
            return await _studentDataAccess.PartiallyUpdateStudent(username, patchDocument);
        }

        public async Task<long> TotalNumberOfStudents(int pageNumber, int itemPerPage, string university, string department)
        {
            return await _studentDataAccess.TotalNumberOfStudents(pageNumber, itemPerPage, university, department);
        }

        public async Task<List<UpdatedField>> UpdateStudent(string username, UpdateStudentModel student)
        {
            var studentModel = await _studentDataAccess.GetStudent(username);

            if (studentModel is null)
                return null;

            var updatedFields = new List<UpdatedField>();

            foreach (var property in typeof(UpdateStudentModel).GetProperties())
            {
                var propertyName = property.Name;
                var previousValue = studentModel.GetType().GetProperty(propertyName)?.GetValue(studentModel, null)?.ToString();
                var currentValue = property.GetValue(student)?.ToString();

                if (previousValue != currentValue)
                {
                    updatedFields.Add(new UpdatedField
                    {
                        FieldName = propertyName,
                        PreviousValue = previousValue,
                        CurrentValue = currentValue
                    });
                }
            }

            studentModel.Student_id = student.Student_id;
            studentModel.Gender = student.Gender;
            studentModel.Year_of_graduation = student.Year_of_graduation;
            studentModel.Blood_group = student.Blood_group;
            studentModel.Department = student.Department;
            studentModel.Name = student.Name;
            studentModel.University = student.University;

            var saveSuccess = await _studentDataAccess.SaveAsync(studentModel);

            if (!saveSuccess)
            {
                return null;
            }

            return updatedFields;
        }
    }
}
