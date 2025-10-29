using Microsoft.AspNetCore.Mvc;
using CourseRegistrationAPI.Models;
using CourseRegistrationAPI.DTOs;
using CourseRegistrationAPI.Repositories;

namespace CourseRegistrationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentsController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        // GET: api/Departments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartmentResponseDto>>> GetDepartments()
        {
            var departments = await _departmentRepository.GetAllAsync();
            var response = departments.Select(d => new DepartmentResponseDto
            {
                Id = d.Id,
                Name = d.Name,
                Description = d.Description,
                HeadOfDepartment = d.HeadOfDepartment,
                InstructorCount = d.Instructors.Count,
                CourseCount = d.Courses.Count
            });

            return Ok(response);
        }

        // GET: api/Departments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DepartmentResponseDto>> GetDepartment(int id)
        {
            var department = await _departmentRepository.GetByIdAsync(id);

            if (department == null)
            {
                return NotFound($"Department with ID {id} not found.");
            }

            var response = new DepartmentResponseDto
            {
                Id = department.Id,
                Name = department.Name,
                Description = department.Description,
                HeadOfDepartment = department.HeadOfDepartment,
                InstructorCount = department.Instructors.Count,
                CourseCount = department.Courses.Count
            };

            return Ok(response);
        }

        // GET: api/Departments/5/instructors
        [HttpGet("{id}/instructors")]
        public async Task<ActionResult<IEnumerable<InstructorResponseDto>>> GetDepartmentInstructors(int id)
        {
            var department = await _departmentRepository.GetByIdAsync(id);
            if (department == null)
            {
                return NotFound($"Department with ID {id} not found.");
            }

            var instructors = await _departmentRepository.GetDepartmentInstructorsAsync(id);
            var response = instructors.Select(i => new InstructorResponseDto
            {
                Id = i.Id,
                FirstName = i.FirstName,
                LastName = i.LastName,
                FullName = i.FullName,
                Email = i.Email,
                DepartmentId = i.DepartmentId,
                DepartmentName = department.Name,
                HireDate = i.HireDate,
                YearsOfService = i.YearsOfService,
                Phone = i.Phone,
                Bio = i.Bio,
                CourseCount = i.Courses.Count
            });

            return Ok(response);
        }

        // GET: api/Departments/5/courses
        [HttpGet("{id}/courses")]
        public async Task<ActionResult<IEnumerable<CourseResponseDto>>> GetDepartmentCourses(int id)
        {
            var department = await _departmentRepository.GetByIdAsync(id);
            if (department == null)
            {
                return NotFound($"Department with ID {id} not found.");
            }

            var courses = await _departmentRepository.GetDepartmentCoursesAsync(id);
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
                DepartmentName = department.Name,
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

        // POST: api/Departments
        [HttpPost]
        public async Task<ActionResult<DepartmentResponseDto>> PostDepartment(CreateDepartmentDto createDepartmentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if department name already exists
            var existingDepartment = await _departmentRepository.GetByNameAsync(createDepartmentDto.Name);
            if (existingDepartment != null)
            {
                return Conflict($"Department with name '{createDepartmentDto.Name}' already exists.");
            }

            var department = new Department
            {
                Name = createDepartmentDto.Name,
                Description = createDepartmentDto.Description,
                HeadOfDepartment = createDepartmentDto.HeadOfDepartment
            };

            await _departmentRepository.AddAsync(department);
            await _departmentRepository.SaveChangesAsync();

            var response = new DepartmentResponseDto
            {
                Id = department.Id,
                Name = department.Name,
                Description = department.Description,
                HeadOfDepartment = department.HeadOfDepartment,
                InstructorCount = 0,
                CourseCount = 0
            };

            return CreatedAtAction(nameof(GetDepartment), new { id = department.Id }, response);
        }

        // PUT: api/Departments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDepartment(int id, UpdateDepartmentDto updateDepartmentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var department = await _departmentRepository.GetByIdAsync(id);
            if (department == null)
            {
                return NotFound($"Department with ID {id} not found.");
            }

            // Check if department name already exists for another department
            var existingDepartment = await _departmentRepository.GetByNameAsync(updateDepartmentDto.Name);
            if (existingDepartment != null && existingDepartment.Id != id)
            {
                return Conflict($"Department with name '{updateDepartmentDto.Name}' already exists.");
            }

            department.Name = updateDepartmentDto.Name;
            department.Description = updateDepartmentDto.Description;
            department.HeadOfDepartment = updateDepartmentDto.HeadOfDepartment;

            await _departmentRepository.UpdateAsync(department);
            await _departmentRepository.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Departments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var department = await _departmentRepository.GetByIdAsync(id);
            if (department == null)
            {
                return NotFound($"Department with ID {id} not found.");
            }

            // Check if department has instructors or courses
            if (department.Instructors.Any() || department.Courses.Any())
            {
                return BadRequest("Cannot delete department with existing instructors or courses.");
            }

            var result = await _departmentRepository.DeleteAsync(id);
            if (!result)
            {
                return NotFound($"Department with ID {id} not found.");
            }

            await _departmentRepository.SaveChangesAsync();
            return NoContent();
        }

        // GET: api/Departments/search/name/{name}
        [HttpGet("search/name/{name}")]
        public async Task<ActionResult<DepartmentResponseDto>> GetDepartmentByName(string name)
        {
            var department = await _departmentRepository.GetByNameAsync(name);

            if (department == null)
            {
                return NotFound($"Department with name '{name}' not found.");
            }

            var response = new DepartmentResponseDto
            {
                Id = department.Id,
                Name = department.Name,
                Description = department.Description,
                HeadOfDepartment = department.HeadOfDepartment,
                InstructorCount = department.Instructors.Count,
                CourseCount = department.Courses.Count
            };

            return Ok(response);
        }
    }
}