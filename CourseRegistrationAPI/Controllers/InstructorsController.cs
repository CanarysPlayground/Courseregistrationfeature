using Microsoft.AspNetCore.Mvc;
using CourseRegistrationAPI.Models;
using CourseRegistrationAPI.DTOs;
using CourseRegistrationAPI.Repositories;

namespace CourseRegistrationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorsController : ControllerBase
    {
        private readonly IInstructorRepository _instructorRepository;
        private readonly IDepartmentRepository _departmentRepository;

        public InstructorsController(IInstructorRepository instructorRepository, IDepartmentRepository departmentRepository)
        {
            _instructorRepository = instructorRepository;
            _departmentRepository = departmentRepository;
        }

        // GET: api/Instructors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InstructorResponseDto>>> GetInstructors()
        {
            var instructors = await _instructorRepository.GetAllAsync();
            var response = instructors.Select(i => new InstructorResponseDto
            {
                Id = i.Id,
                FirstName = i.FirstName,
                LastName = i.LastName,
                FullName = i.FullName,
                Email = i.Email,
                DepartmentId = i.DepartmentId,
                DepartmentName = i.Department.Name,
                HireDate = i.HireDate,
                YearsOfService = i.YearsOfService,
                Phone = i.Phone,
                Bio = i.Bio,
                CourseCount = i.Courses.Count
            });

            return Ok(response);
        }

        // GET: api/Instructors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InstructorResponseDto>> GetInstructor(int id)
        {
            var instructor = await _instructorRepository.GetByIdAsync(id);

            if (instructor == null)
            {
                return NotFound($"Instructor with ID {id} not found.");
            }

            var response = new InstructorResponseDto
            {
                Id = instructor.Id,
                FirstName = instructor.FirstName,
                LastName = instructor.LastName,
                FullName = instructor.FullName,
                Email = instructor.Email,
                DepartmentId = instructor.DepartmentId,
                DepartmentName = instructor.Department.Name,
                HireDate = instructor.HireDate,
                YearsOfService = instructor.YearsOfService,
                Phone = instructor.Phone,
                Bio = instructor.Bio,
                CourseCount = instructor.Courses.Count
            };

            return Ok(response);
        }

        // GET: api/Instructors/5/courses
        [HttpGet("{id}/courses")]
        public async Task<ActionResult<IEnumerable<CourseResponseDto>>> GetInstructorCourses(int id)
        {
            var instructor = await _instructorRepository.GetByIdAsync(id);
            if (instructor == null)
            {
                return NotFound($"Instructor with ID {id} not found.");
            }

            var courses = await _instructorRepository.GetInstructorCoursesAsync(id);
            var response = courses.Select(c => new CourseResponseDto
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                Credits = c.Credits,
                MaxCapacity = c.MaxCapacity,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                InstructorId = c.InstructorId,
                InstructorName = instructor.FullName,
                DepartmentId = c.DepartmentId,
                DepartmentName = c.Department.Name,
                CreatedDate = c.CreatedDate,
                ModifiedDate = c.ModifiedDate,
                IsActive = c.IsActive,
                CurrentEnrollment = c.CurrentEnrollment,
                AvailableSpots = c.AvailableSpots,
                IsFull = c.IsFull,
                DurationInDays = c.DurationInDays
            });

            return Ok(response);
        }

        // GET: api/Instructors/department/5
        [HttpGet("department/{departmentId}")]
        public async Task<ActionResult<IEnumerable<InstructorResponseDto>>> GetInstructorsByDepartment(int departmentId)
        {
            var instructors = await _instructorRepository.GetInstructorsByDepartmentAsync(departmentId);
            var response = instructors.Select(i => new InstructorResponseDto
            {
                Id = i.Id,
                FirstName = i.FirstName,
                LastName = i.LastName,
                FullName = i.FullName,
                Email = i.Email,
                DepartmentId = i.DepartmentId,
                DepartmentName = i.Department.Name,
                HireDate = i.HireDate,
                YearsOfService = i.YearsOfService,
                Phone = i.Phone,
                Bio = i.Bio,
                CourseCount = i.Courses.Count
            });

            return Ok(response);
        }

        // POST: api/Instructors
        [HttpPost]
        public async Task<ActionResult<InstructorResponseDto>> PostInstructor(CreateInstructorDto createInstructorDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if email already exists
            var existingInstructor = await _instructorRepository.GetByEmailAsync(createInstructorDto.Email);
            if (existingInstructor != null)
            {
                return Conflict($"Instructor with email {createInstructorDto.Email} already exists.");
            }

            // Validate department exists
            var department = await _departmentRepository.GetByIdAsync(createInstructorDto.DepartmentId);
            if (department == null)
            {
                return BadRequest($"Department with ID {createInstructorDto.DepartmentId} not found.");
            }

            var instructor = new Instructor
            {
                FirstName = createInstructorDto.FirstName,
                LastName = createInstructorDto.LastName,
                Email = createInstructorDto.Email,
                DepartmentId = createInstructorDto.DepartmentId,
                HireDate = DateTime.UtcNow,
                Phone = createInstructorDto.Phone,
                Bio = createInstructorDto.Bio
            };

            await _instructorRepository.AddAsync(instructor);
            await _instructorRepository.SaveChangesAsync();

            // Fetch the instructor with navigation properties
            var createdInstructor = await _instructorRepository.GetByIdAsync(instructor.Id);

            var response = new InstructorResponseDto
            {
                Id = createdInstructor!.Id,
                FirstName = createdInstructor.FirstName,
                LastName = createdInstructor.LastName,
                FullName = createdInstructor.FullName,
                Email = createdInstructor.Email,
                DepartmentId = createdInstructor.DepartmentId,
                DepartmentName = createdInstructor.Department.Name,
                HireDate = createdInstructor.HireDate,
                YearsOfService = createdInstructor.YearsOfService,
                Phone = createdInstructor.Phone,
                Bio = createdInstructor.Bio,
                CourseCount = 0
            };

            return CreatedAtAction(nameof(GetInstructor), new { id = instructor.Id }, response);
        }

        // PUT: api/Instructors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInstructor(int id, UpdateInstructorDto updateInstructorDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var instructor = await _instructorRepository.GetByIdAsync(id);
            if (instructor == null)
            {
                return NotFound($"Instructor with ID {id} not found.");
            }

            // Check if email already exists for another instructor
            var existingInstructor = await _instructorRepository.GetByEmailAsync(updateInstructorDto.Email);
            if (existingInstructor != null && existingInstructor.Id != id)
            {
                return Conflict($"Instructor with email {updateInstructorDto.Email} already exists.");
            }

            // Validate department exists
            var department = await _departmentRepository.GetByIdAsync(updateInstructorDto.DepartmentId);
            if (department == null)
            {
                return BadRequest($"Department with ID {updateInstructorDto.DepartmentId} not found.");
            }

            instructor.FirstName = updateInstructorDto.FirstName;
            instructor.LastName = updateInstructorDto.LastName;
            instructor.Email = updateInstructorDto.Email;
            instructor.DepartmentId = updateInstructorDto.DepartmentId;
            instructor.Phone = updateInstructorDto.Phone;
            instructor.Bio = updateInstructorDto.Bio;

            await _instructorRepository.UpdateAsync(instructor);
            await _instructorRepository.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Instructors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInstructor(int id)
        {
            var instructor = await _instructorRepository.GetByIdAsync(id);
            if (instructor == null)
            {
                return NotFound($"Instructor with ID {id} not found.");
            }

            // Check if instructor has active courses
            if (instructor.Courses.Any(c => c.IsActive && c.EndDate > DateTime.Now))
            {
                return BadRequest("Cannot delete instructor with active courses.");
            }

            var result = await _instructorRepository.DeleteAsync(id);
            if (!result)
            {
                return NotFound($"Instructor with ID {id} not found.");
            }

            await _instructorRepository.SaveChangesAsync();
            return NoContent();
        }

        // GET: api/Instructors/search/email/{email}
        [HttpGet("search/email/{email}")]
        public async Task<ActionResult<InstructorResponseDto>> GetInstructorByEmail(string email)
        {
            var instructor = await _instructorRepository.GetByEmailAsync(email);

            if (instructor == null)
            {
                return NotFound($"Instructor with email {email} not found.");
            }

            var response = new InstructorResponseDto
            {
                Id = instructor.Id,
                FirstName = instructor.FirstName,
                LastName = instructor.LastName,
                FullName = instructor.FullName,
                Email = instructor.Email,
                DepartmentId = instructor.DepartmentId,
                DepartmentName = instructor.Department.Name,
                HireDate = instructor.HireDate,
                YearsOfService = instructor.YearsOfService,
                Phone = instructor.Phone,
                Bio = instructor.Bio,
                CourseCount = instructor.Courses.Count
            };

            return Ok(response);
        }
    }
}