using Microsoft.AspNetCore.Mvc;
using CourseRegistrationAPI.Models;
using CourseRegistrationAPI.DTOs;
using CourseRegistrationAPI.Repositories;

namespace CourseRegistrationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IInstructorRepository _instructorRepository;

        public UsersController(
            IUserRepository userRepository,
            IStudentRepository studentRepository,
            IInstructorRepository instructorRepository)
        {
            _userRepository = userRepository;
            _studentRepository = studentRepository;
            _instructorRepository = instructorRepository;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetUsers()
        {
            var users = await _userRepository.GetAllAsync();
            var response = users.Select(u => new UserResponseDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                Role = u.Role,
                RoleDescription = u.Role.ToString(),
                IsActive = u.IsActive,
                CreatedDate = u.CreatedDate,
                LastLoginDate = u.LastLoginDate,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Phone = u.Phone,
                StudentId = u.StudentId,
                InstructorId = u.InstructorId,
                StudentName = u.Student?.FullName,
                InstructorName = u.Instructor?.FullName
            });

            return Ok(response);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseDto>> GetUser(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
            {
                return NotFound($"User with ID {id} not found.");
            }

            var response = new UserResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                RoleDescription = user.Role.ToString(),
                IsActive = user.IsActive,
                CreatedDate = user.CreatedDate,
                LastLoginDate = user.LastLoginDate,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.Phone,
                StudentId = user.StudentId,
                InstructorId = user.InstructorId,
                StudentName = user.Student?.FullName,
                InstructorName = user.Instructor?.FullName
            };

            return Ok(response);
        }

        // GET: api/Users/role/{role}
        [HttpGet("role/{role}")]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetUsersByRole(UserRole role)
        {
            var users = await _userRepository.GetUsersByRoleAsync(role);
            var response = users.Select(u => new UserResponseDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                Role = u.Role,
                RoleDescription = u.Role.ToString(),
                IsActive = u.IsActive,
                CreatedDate = u.CreatedDate,
                LastLoginDate = u.LastLoginDate,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Phone = u.Phone,
                StudentId = u.StudentId,
                InstructorId = u.InstructorId,
                StudentName = u.Student?.FullName,
                InstructorName = u.Instructor?.FullName
            });

            return Ok(response);
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<UserResponseDto>> PostUser(CreateUserDto createUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if username already exists
            var existingUser = await _userRepository.GetByUsernameAsync(createUserDto.Username);
            if (existingUser != null)
            {
                return Conflict($"User with username '{createUserDto.Username}' already exists.");
            }

            // Check if email already exists
            var existingEmailUser = await _userRepository.GetByEmailAsync(createUserDto.Email);
            if (existingEmailUser != null)
            {
                return Conflict($"User with email '{createUserDto.Email}' already exists.");
            }

            // Validate student reference if provided
            if (createUserDto.StudentId.HasValue)
            {
                var student = await _studentRepository.GetByIdAsync(createUserDto.StudentId.Value);
                if (student == null)
                {
                    return BadRequest($"Student with ID {createUserDto.StudentId} not found.");
                }
            }

            // Validate instructor reference if provided
            if (createUserDto.InstructorId.HasValue)
            {
                var instructor = await _instructorRepository.GetByIdAsync(createUserDto.InstructorId.Value);
                if (instructor == null)
                {
                    return BadRequest($"Instructor with ID {createUserDto.InstructorId} not found.");
                }
            }

            // Validate role consistency
            if (createUserDto.Role == UserRole.Student && !createUserDto.StudentId.HasValue)
            {
                return BadRequest("Student role requires a valid StudentId.");
            }

            if (createUserDto.Role == UserRole.Instructor && !createUserDto.InstructorId.HasValue)
            {
                return BadRequest("Instructor role requires a valid InstructorId.");
            }

            var user = new User
            {
                Username = createUserDto.Username,
                Email = createUserDto.Email,
                PasswordHash = createUserDto.Password, // In production, this should be hashed
                Role = createUserDto.Role,
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                FirstName = createUserDto.FirstName,
                LastName = createUserDto.LastName,
                Phone = createUserDto.Phone,
                StudentId = createUserDto.StudentId,
                InstructorId = createUserDto.InstructorId
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            // Fetch the user with navigation properties
            var createdUser = await _userRepository.GetByIdAsync(user.Id);

            var response = new UserResponseDto
            {
                Id = createdUser!.Id,
                Username = createdUser.Username,
                Email = createdUser.Email,
                Role = createdUser.Role,
                RoleDescription = createdUser.Role.ToString(),
                IsActive = createdUser.IsActive,
                CreatedDate = createdUser.CreatedDate,
                LastLoginDate = createdUser.LastLoginDate,
                FirstName = createdUser.FirstName,
                LastName = createdUser.LastName,
                Phone = createdUser.Phone,
                StudentId = createdUser.StudentId,
                InstructorId = createdUser.InstructorId,
                StudentName = createdUser.Student?.FullName,
                InstructorName = createdUser.Instructor?.FullName
            };

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, response);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UpdateUserDto updateUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound($"User with ID {id} not found.");
            }

            // Check if email already exists for another user
            var existingEmailUser = await _userRepository.GetByEmailAsync(updateUserDto.Email);
            if (existingEmailUser != null && existingEmailUser.Id != id)
            {
                return Conflict($"User with email '{updateUserDto.Email}' already exists.");
            }

            // Validate student reference if provided
            if (updateUserDto.StudentId.HasValue)
            {
                var student = await _studentRepository.GetByIdAsync(updateUserDto.StudentId.Value);
                if (student == null)
                {
                    return BadRequest($"Student with ID {updateUserDto.StudentId} not found.");
                }
            }

            // Validate instructor reference if provided
            if (updateUserDto.InstructorId.HasValue)
            {
                var instructor = await _instructorRepository.GetByIdAsync(updateUserDto.InstructorId.Value);
                if (instructor == null)
                {
                    return BadRequest($"Instructor with ID {updateUserDto.InstructorId} not found.");
                }
            }

            // Validate role consistency
            if (updateUserDto.Role == UserRole.Student && !updateUserDto.StudentId.HasValue)
            {
                return BadRequest("Student role requires a valid StudentId.");
            }

            if (updateUserDto.Role == UserRole.Instructor && !updateUserDto.InstructorId.HasValue)
            {
                return BadRequest("Instructor role requires a valid InstructorId.");
            }

            user.Email = updateUserDto.Email;
            user.Role = updateUserDto.Role;
            user.IsActive = updateUserDto.IsActive;
            user.FirstName = updateUserDto.FirstName;
            user.LastName = updateUserDto.LastName;
            user.Phone = updateUserDto.Phone;
            user.StudentId = updateUserDto.StudentId;
            user.InstructorId = updateUserDto.InstructorId;

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userRepository.DeleteAsync(id);
            if (!result)
            {
                return NotFound($"User with ID {id} not found.");
            }

            await _userRepository.SaveChangesAsync();
            return NoContent();
        }

        // GET: api/Users/search/username/{username}
        [HttpGet("search/username/{username}")]
        public async Task<ActionResult<UserResponseDto>> GetUserByUsername(string username)
        {
            var user = await _userRepository.GetByUsernameAsync(username);

            if (user == null)
            {
                return NotFound($"User with username '{username}' not found.");
            }

            var response = new UserResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                RoleDescription = user.Role.ToString(),
                IsActive = user.IsActive,
                CreatedDate = user.CreatedDate,
                LastLoginDate = user.LastLoginDate,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.Phone,
                StudentId = user.StudentId,
                InstructorId = user.InstructorId,
                StudentName = user.Student?.FullName,
                InstructorName = user.Instructor?.FullName
            };

            return Ok(response);
        }

        // POST: api/Users/login
        [HttpPost("login")]
        public async Task<ActionResult<UserResponseDto>> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userRepository.GetByUsernameAsync(loginDto.Username);
            if (user == null || user.PasswordHash != loginDto.Password || !user.IsActive)
            {
                return Unauthorized("Invalid username or password.");
            }

            // Update last login date
            user.LastLoginDate = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            var response = new UserResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                RoleDescription = user.Role.ToString(),
                IsActive = user.IsActive,
                CreatedDate = user.CreatedDate,
                LastLoginDate = user.LastLoginDate,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.Phone,
                StudentId = user.StudentId,
                InstructorId = user.InstructorId,
                StudentName = user.Student?.FullName,
                InstructorName = user.Instructor?.FullName
            };

            return Ok(response);
        }

        // POST: api/Users/5/change-password
        [HttpPost("{id}/change-password")]
        public async Task<IActionResult> ChangePassword(int id, ChangePasswordDto changePasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound($"User with ID {id} not found.");
            }

            // Verify current password
            if (user.PasswordHash != changePasswordDto.CurrentPassword)
            {
                return BadRequest("Current password is incorrect.");
            }

            // Update password
            user.PasswordHash = changePasswordDto.NewPassword; // In production, this should be hashed

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}