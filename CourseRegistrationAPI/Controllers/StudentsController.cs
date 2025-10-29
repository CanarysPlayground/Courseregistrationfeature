using Microsoft.AspNetCore.Mvc;
using CourseRegistrationAPI.Models;
using CourseRegistrationAPI.DTOs;
using CourseRegistrationAPI.Repositories;

namespace CourseRegistrationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentRepository _studentRepository;

        public StudentsController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        // GET: api/Students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentResponseDto>>> GetStudents()
        {
            var students = await _studentRepository.GetAllAsync();
            var response = students.Select(s => new StudentResponseDto
            {
                Id = s.Id,
                FirstName = s.FirstName,
                LastName = s.LastName,
                FullName = s.FullName,
                Email = s.Email,
                DateOfBirth = s.DateOfBirth,
                Age = s.Age,
                EnrollmentDate = s.EnrollmentDate,
                Phone = s.Phone,
                Address = s.Address,
                RegistrationCount = s.Registrations.Count
            });

            return Ok(response);
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentResponseDto>> GetStudent(int id)
        {
            var student = await _studentRepository.GetByIdAsync(id);

            if (student == null)
            {
                return NotFound($"Student with ID {id} not found.");
            }

            var response = new StudentResponseDto
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                FullName = student.FullName,
                Email = student.Email,
                DateOfBirth = student.DateOfBirth,
                Age = student.Age,
                EnrollmentDate = student.EnrollmentDate,
                Phone = student.Phone,
                Address = student.Address,
                RegistrationCount = student.Registrations.Count
            };

            return Ok(response);
        }

        // GET: api/Students/5/registrations
        [HttpGet("{id}/registrations")]
        public async Task<ActionResult<IEnumerable<RegistrationResponseDto>>> GetStudentRegistrations(int id)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null)
            {
                return NotFound($"Student with ID {id} not found.");
            }

            var registrations = await _studentRepository.GetStudentRegistrationsAsync(id);
            var response = registrations.Select(r => new RegistrationResponseDto
            {
                Id = r.Id,
                StudentId = r.StudentId,
                StudentName = r.Student.FullName,
                CourseId = r.CourseId,
                CourseTitle = r.Course.Title,
                InstructorName = r.Course.Instructor.FullName,
                RegistrationDate = r.RegistrationDate,
                Status = r.Status,
                StatusDescription = r.StatusDescription,
                Grade = r.Grade,
                GradeDescription = r.GradeDescription,
                CompletionDate = r.CompletionDate,
                Notes = r.Notes,
                IsActive = r.IsActive
            });

            return Ok(response);
        }

        // POST: api/Students
        [HttpPost]
        public async Task<ActionResult<StudentResponseDto>> PostStudent(CreateStudentDto createStudentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if email already exists
            var existingStudent = await _studentRepository.GetByEmailAsync(createStudentDto.Email);
            if (existingStudent != null)
            {
                return Conflict($"Student with email {createStudentDto.Email} already exists.");
            }

            var student = new Student
            {
                FirstName = createStudentDto.FirstName,
                LastName = createStudentDto.LastName,
                Email = createStudentDto.Email,
                DateOfBirth = createStudentDto.DateOfBirth,
                EnrollmentDate = DateTime.UtcNow,
                Phone = createStudentDto.Phone,
                Address = createStudentDto.Address
            };

            await _studentRepository.AddAsync(student);
            await _studentRepository.SaveChangesAsync();

            var response = new StudentResponseDto
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                FullName = student.FullName,
                Email = student.Email,
                DateOfBirth = student.DateOfBirth,
                Age = student.Age,
                EnrollmentDate = student.EnrollmentDate,
                Phone = student.Phone,
                Address = student.Address,
                RegistrationCount = 0
            };

            return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, response);
        }

        // PUT: api/Students/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, UpdateStudentDto updateStudentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null)
            {
                return NotFound($"Student with ID {id} not found.");
            }

            // Check if email already exists for another student
            var existingStudent = await _studentRepository.GetByEmailAsync(updateStudentDto.Email);
            if (existingStudent != null && existingStudent.Id != id)
            {
                return Conflict($"Student with email {updateStudentDto.Email} already exists.");
            }

            student.FirstName = updateStudentDto.FirstName;
            student.LastName = updateStudentDto.LastName;
            student.Email = updateStudentDto.Email;
            student.DateOfBirth = updateStudentDto.DateOfBirth;
            student.Phone = updateStudentDto.Phone;
            student.Address = updateStudentDto.Address;

            await _studentRepository.UpdateAsync(student);
            await _studentRepository.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var result = await _studentRepository.DeleteAsync(id);
            if (!result)
            {
                return NotFound($"Student with ID {id} not found.");
            }

            await _studentRepository.SaveChangesAsync();
            return NoContent();
        }

        // GET: api/Students/search/email/{email}
        [HttpGet("search/email/{email}")]
        public async Task<ActionResult<StudentResponseDto>> GetStudentByEmail(string email)
        {
            var student = await _studentRepository.GetByEmailAsync(email);

            if (student == null)
            {
                return NotFound($"Student with email {email} not found.");
            }

            var response = new StudentResponseDto
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                FullName = student.FullName,
                Email = student.Email,
                DateOfBirth = student.DateOfBirth,
                Age = student.Age,
                EnrollmentDate = student.EnrollmentDate,
                Phone = student.Phone,
                Address = student.Address,
                RegistrationCount = student.Registrations.Count
            };

            return Ok(response);
        }
    }
}