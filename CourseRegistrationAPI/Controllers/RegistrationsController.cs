using Microsoft.AspNetCore.Mvc;
using CourseRegistrationAPI.Models;
using CourseRegistrationAPI.DTOs;
using CourseRegistrationAPI.Repositories;

namespace CourseRegistrationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationsController : ControllerBase
    {
        private readonly IRegistrationRepository _registrationRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ICourseRepository _courseRepository;

        public RegistrationsController(
            IRegistrationRepository registrationRepository,
            IStudentRepository studentRepository,
            ICourseRepository courseRepository)
        {
            _registrationRepository = registrationRepository;
            _studentRepository = studentRepository;
            _courseRepository = courseRepository;
        }

        // GET: api/Registrations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RegistrationResponseDto>>> GetRegistrations()
        {
            var registrations = await _registrationRepository.GetAllAsync();
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

        // GET: api/Registrations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RegistrationResponseDto>> GetRegistration(int id)
        {
            var registration = await _registrationRepository.GetByIdAsync(id);

            if (registration == null)
            {
                return NotFound($"Registration with ID {id} not found.");
            }

            var response = new RegistrationResponseDto
            {
                Id = registration.Id,
                StudentId = registration.StudentId,
                StudentName = registration.Student.FullName,
                CourseId = registration.CourseId,
                CourseTitle = registration.Course.Title,
                InstructorName = registration.Course.Instructor.FullName,
                RegistrationDate = registration.RegistrationDate,
                Status = registration.Status,
                StatusDescription = registration.StatusDescription,
                Grade = registration.Grade,
                GradeDescription = registration.GradeDescription,
                CompletionDate = registration.CompletionDate,
                Notes = registration.Notes,
                IsActive = registration.IsActive
            };

            return Ok(response);
        }

        // GET: api/Registrations/student/5
        [HttpGet("student/{studentId}")]
        public async Task<ActionResult<IEnumerable<RegistrationResponseDto>>> GetRegistrationsByStudent(int studentId)
        {
            var registrations = await _registrationRepository.GetRegistrationsByStudentAsync(studentId);
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

        // GET: api/Registrations/course/5
        [HttpGet("course/{courseId}")]
        public async Task<ActionResult<IEnumerable<RegistrationResponseDto>>> GetRegistrationsByCourse(int courseId)
        {
            var registrations = await _registrationRepository.GetRegistrationsByCourseAsync(courseId);
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

        // GET: api/Registrations/status/{status}
        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<RegistrationResponseDto>>> GetRegistrationsByStatus(RegistrationStatus status)
        {
            var registrations = await _registrationRepository.GetRegistrationsByStatusAsync(status);
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

        // POST: api/Registrations
        [HttpPost]
        public async Task<ActionResult<RegistrationResponseDto>> PostRegistration(CreateRegistrationDto createRegistrationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Validate student exists
            var student = await _studentRepository.GetByIdAsync(createRegistrationDto.StudentId);
            if (student == null)
            {
                return BadRequest($"Student with ID {createRegistrationDto.StudentId} not found.");
            }

            // Validate course exists
            var course = await _courseRepository.GetByIdAsync(createRegistrationDto.CourseId);
            if (course == null)
            {
                return BadRequest($"Course with ID {createRegistrationDto.CourseId} not found.");
            }

            // Check if course is active and available
            if (!course.IsActive)
            {
                return BadRequest("Course is not active.");
            }

            if (course.StartDate <= DateTime.Now)
            {
                return BadRequest("Cannot register for courses that have already started.");
            }

            // Check if student is already registered for this course
            var existingRegistration = await _registrationRepository.GetRegistrationByStudentAndCourseAsync(
                createRegistrationDto.StudentId, createRegistrationDto.CourseId);
            if (existingRegistration != null)
            {
                return Conflict("Student is already registered for this course.");
            }

            // Check if course is full
            if (course.IsFull)
            {
                return BadRequest("Course is full.");
            }

            var registration = new Registration
            {
                StudentId = createRegistrationDto.StudentId,
                CourseId = createRegistrationDto.CourseId,
                RegistrationDate = DateTime.UtcNow,
                Status = RegistrationStatus.Enrolled,
                Notes = createRegistrationDto.Notes
            };

            await _registrationRepository.AddAsync(registration);
            await _registrationRepository.SaveChangesAsync();

            // Fetch the registration with navigation properties
            var createdRegistration = await _registrationRepository.GetByIdAsync(registration.Id);

            var response = new RegistrationResponseDto
            {
                Id = createdRegistration!.Id,
                StudentId = createdRegistration.StudentId,
                StudentName = createdRegistration.Student.FullName,
                CourseId = createdRegistration.CourseId,
                CourseTitle = createdRegistration.Course.Title,
                InstructorName = createdRegistration.Course.Instructor.FullName,
                RegistrationDate = createdRegistration.RegistrationDate,
                Status = createdRegistration.Status,
                StatusDescription = createdRegistration.StatusDescription,
                Grade = createdRegistration.Grade,
                GradeDescription = createdRegistration.GradeDescription,
                CompletionDate = createdRegistration.CompletionDate,
                Notes = createdRegistration.Notes,
                IsActive = createdRegistration.IsActive
            };

            return CreatedAtAction(nameof(GetRegistration), new { id = registration.Id }, response);
        }

        // PUT: api/Registrations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRegistration(int id, UpdateRegistrationDto updateRegistrationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var registration = await _registrationRepository.GetByIdAsync(id);
            if (registration == null)
            {
                return NotFound($"Registration with ID {id} not found.");
            }

            // Validate grade assignment rules
            if (updateRegistrationDto.Grade.HasValue && updateRegistrationDto.Status != RegistrationStatus.Completed)
            {
                return BadRequest("Grade can only be assigned to completed registrations.");
            }

            if (updateRegistrationDto.Status == RegistrationStatus.Completed && !updateRegistrationDto.Grade.HasValue)
            {
                return BadRequest("Completed registrations must have a grade assigned.");
            }

            registration.Status = updateRegistrationDto.Status;
            registration.Grade = updateRegistrationDto.Grade;
            registration.CompletionDate = updateRegistrationDto.CompletionDate;
            registration.Notes = updateRegistrationDto.Notes;

            // Set completion date automatically if status is completed
            if (updateRegistrationDto.Status == RegistrationStatus.Completed && !registration.CompletionDate.HasValue)
            {
                registration.CompletionDate = DateTime.UtcNow;
            }

            await _registrationRepository.UpdateAsync(registration);
            await _registrationRepository.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Registrations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegistration(int id)
        {
            var registration = await _registrationRepository.GetByIdAsync(id);
            if (registration == null)
            {
                return NotFound($"Registration with ID {id} not found.");
            }

            // Check if registration can be deleted
            if (registration.Status == RegistrationStatus.Completed)
            {
                return BadRequest("Cannot delete completed registrations.");
            }

            var result = await _registrationRepository.DeleteAsync(id);
            if (!result)
            {
                return NotFound($"Registration with ID {id} not found.");
            }

            await _registrationRepository.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/Registrations/5/drop
        [HttpPost("{id}/drop")]
        public async Task<IActionResult> DropRegistration(int id)
        {
            var registration = await _registrationRepository.GetByIdAsync(id);
            if (registration == null)
            {
                return NotFound($"Registration with ID {id} not found.");
            }

            if (registration.Status == RegistrationStatus.Completed)
            {
                return BadRequest("Cannot drop completed registrations.");
            }

            if (registration.Status == RegistrationStatus.Dropped)
            {
                return BadRequest("Registration is already dropped.");
            }

            registration.Status = RegistrationStatus.Dropped;
            registration.CompletionDate = DateTime.UtcNow;

            await _registrationRepository.UpdateAsync(registration);
            await _registrationRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}