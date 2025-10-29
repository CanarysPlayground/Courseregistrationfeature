using Microsoft.AspNetCore.Mvc;
using CourseRegistrationAPI.Models;
using CourseRegistrationAPI.DTOs;
using CourseRegistrationAPI.Repositories;

namespace CourseRegistrationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IInstructorRepository _instructorRepository;
        private readonly IDepartmentRepository _departmentRepository;

        public CoursesController(
            ICourseRepository courseRepository,
            IInstructorRepository instructorRepository,
            IDepartmentRepository departmentRepository)
        {
            _courseRepository = courseRepository;
            _instructorRepository = instructorRepository;
            _departmentRepository = departmentRepository;
        }

        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseResponseDto>>> GetCourses()
        {
            var courses = await _courseRepository.GetAllAsync();
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
                InstructorName = c.Instructor.FullName,
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

        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CourseResponseDto>> GetCourse(int id)
        {
            var course = await _courseRepository.GetByIdAsync(id);

            if (course == null)
            {
                return NotFound($"Course with ID {id} not found.");
            }

            var response = new CourseResponseDto
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description,
                Credits = course.Credits,
                MaxCapacity = course.MaxCapacity,
                StartDate = course.StartDate,
                EndDate = course.EndDate,
                InstructorId = course.InstructorId,
                InstructorName = course.Instructor.FullName,
                DepartmentId = course.DepartmentId,
                DepartmentName = course.Department.Name,
                CreatedDate = course.CreatedDate,
                ModifiedDate = course.ModifiedDate,
                IsActive = course.IsActive,
                CurrentEnrollment = course.CurrentEnrollment,
                AvailableSpots = course.AvailableSpots,
                IsFull = course.IsFull,
                DurationInDays = course.DurationInDays
            };

            return Ok(response);
        }

        // GET: api/Courses/5/registrations
        [HttpGet("{id}/registrations")]
        public async Task<ActionResult<IEnumerable<RegistrationResponseDto>>> GetCourseRegistrations(int id)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            if (course == null)
            {
                return NotFound($"Course with ID {id} not found.");
            }

            var registrations = await _courseRepository.GetCourseRegistrationsAsync(id);
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

        // GET: api/Courses/available
        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<CourseResponseDto>>> GetAvailableCourses()
        {
            var courses = await _courseRepository.GetAvailableCoursesAsync();
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
                InstructorName = c.Instructor.FullName,
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

        // GET: api/Courses/instructor/5
        [HttpGet("instructor/{instructorId}")]
        public async Task<ActionResult<IEnumerable<CourseResponseDto>>> GetCoursesByInstructor(int instructorId)
        {
            var courses = await _courseRepository.GetCoursesByInstructorAsync(instructorId);
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
                InstructorName = c.Instructor.FullName,
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

        // GET: api/Courses/department/5
        [HttpGet("department/{departmentId}")]
        public async Task<ActionResult<IEnumerable<CourseResponseDto>>> GetCoursesByDepartment(int departmentId)
        {
            var courses = await _courseRepository.GetCoursesByDepartmentAsync(departmentId);
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
                InstructorName = c.Instructor.FullName,
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

        // POST: api/Courses
        [HttpPost]
        public async Task<ActionResult<CourseResponseDto>> PostCourse(CreateCourseDto createCourseDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Validate instructor exists
            var instructor = await _instructorRepository.GetByIdAsync(createCourseDto.InstructorId);
            if (instructor == null)
            {
                return BadRequest($"Instructor with ID {createCourseDto.InstructorId} not found.");
            }

            // Validate department exists
            var department = await _departmentRepository.GetByIdAsync(createCourseDto.DepartmentId);
            if (department == null)
            {
                return BadRequest($"Department with ID {createCourseDto.DepartmentId} not found.");
            }

            // Validate instructor belongs to department
            if (instructor.DepartmentId != createCourseDto.DepartmentId)
            {
                return BadRequest($"Instructor does not belong to the specified department.");
            }

            // Validate dates
            if (createCourseDto.StartDate >= createCourseDto.EndDate)
            {
                return BadRequest("Start date must be before end date.");
            }

            if (createCourseDto.StartDate < DateTime.Now.AddDays(7))
            {
                return BadRequest("Course must start at least 7 days from now.");
            }

            // Check for schedule conflicts
            var hasConflict = await _courseRepository.HasScheduleConflictAsync(
                createCourseDto.InstructorId, 
                createCourseDto.StartDate, 
                createCourseDto.EndDate);

            if (hasConflict)
            {
                return Conflict("Instructor has a schedule conflict with existing courses.");
            }

            var course = new Course
            {
                Title = createCourseDto.Title,
                Description = createCourseDto.Description,
                Credits = createCourseDto.Credits,
                MaxCapacity = createCourseDto.MaxCapacity,
                StartDate = createCourseDto.StartDate,
                EndDate = createCourseDto.EndDate,
                InstructorId = createCourseDto.InstructorId,
                DepartmentId = createCourseDto.DepartmentId,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                IsActive = true
            };

            await _courseRepository.AddAsync(course);
            await _courseRepository.SaveChangesAsync();

            // Fetch the course with navigation properties
            var createdCourse = await _courseRepository.GetByIdAsync(course.Id);

            var response = new CourseResponseDto
            {
                Id = createdCourse!.Id,
                Title = createdCourse.Title,
                Description = createdCourse.Description,
                Credits = createdCourse.Credits,
                MaxCapacity = createdCourse.MaxCapacity,
                StartDate = createdCourse.StartDate,
                EndDate = createdCourse.EndDate,
                InstructorId = createdCourse.InstructorId,
                InstructorName = createdCourse.Instructor.FullName,
                DepartmentId = createdCourse.DepartmentId,
                DepartmentName = createdCourse.Department.Name,
                CreatedDate = createdCourse.CreatedDate,
                ModifiedDate = createdCourse.ModifiedDate,
                IsActive = createdCourse.IsActive,
                CurrentEnrollment = createdCourse.CurrentEnrollment,
                AvailableSpots = createdCourse.AvailableSpots,
                IsFull = createdCourse.IsFull,
                DurationInDays = createdCourse.DurationInDays
            };

            return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, response);
        }

        // PUT: api/Courses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, UpdateCourseDto updateCourseDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var course = await _courseRepository.GetByIdAsync(id);
            if (course == null)
            {
                return NotFound($"Course with ID {id} not found.");
            }

            // Validate instructor exists
            var instructor = await _instructorRepository.GetByIdAsync(updateCourseDto.InstructorId);
            if (instructor == null)
            {
                return BadRequest($"Instructor with ID {updateCourseDto.InstructorId} not found.");
            }

            // Validate department exists
            var department = await _departmentRepository.GetByIdAsync(updateCourseDto.DepartmentId);
            if (department == null)
            {
                return BadRequest($"Department with ID {updateCourseDto.DepartmentId} not found.");
            }

            // Validate instructor belongs to department
            if (instructor.DepartmentId != updateCourseDto.DepartmentId)
            {
                return BadRequest($"Instructor does not belong to the specified department.");
            }

            // Validate dates
            if (updateCourseDto.StartDate >= updateCourseDto.EndDate)
            {
                return BadRequest("Start date must be before end date.");
            }

            // Check for schedule conflicts (excluding current course)
            var hasConflict = await _courseRepository.HasScheduleConflictAsync(
                updateCourseDto.InstructorId, 
                updateCourseDto.StartDate, 
                updateCourseDto.EndDate, 
                id);

            if (hasConflict)
            {
                return Conflict("Instructor has a schedule conflict with existing courses.");
            }

            course.Title = updateCourseDto.Title;
            course.Description = updateCourseDto.Description;
            course.Credits = updateCourseDto.Credits;
            course.MaxCapacity = updateCourseDto.MaxCapacity;
            course.StartDate = updateCourseDto.StartDate;
            course.EndDate = updateCourseDto.EndDate;
            course.InstructorId = updateCourseDto.InstructorId;
            course.DepartmentId = updateCourseDto.DepartmentId;
            course.IsActive = updateCourseDto.IsActive;
            course.ModifiedDate = DateTime.UtcNow;

            await _courseRepository.UpdateAsync(course);
            await _courseRepository.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            if (course == null)
            {
                return NotFound($"Course with ID {id} not found.");
            }

            // Check if course has registrations
            if (course.Registrations.Any(r => r.Status == RegistrationStatus.Enrolled))
            {
                return BadRequest("Cannot delete course with active registrations.");
            }

            var result = await _courseRepository.DeleteAsync(id);
            if (!result)
            {
                return NotFound($"Course with ID {id} not found.");
            }

            await _courseRepository.SaveChangesAsync();
            return NoContent();
        }
    }
}