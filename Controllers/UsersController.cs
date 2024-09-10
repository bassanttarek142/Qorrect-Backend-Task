using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Qorrect_Backend_Task.Dtos;
using Qorrect_Backend_Task.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Qorrect_Backend_Task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get total user count
        [HttpGet("count")]
        public async Task<IActionResult> GetUserCount()
        {
            var userCount = await _context.Users.CountAsync();
            return Ok(userCount);
        }

        // Create a new user
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto createUserDto)
        {
            if (createUserDto == null)
            {
                throw new ArgumentNullException(nameof(createUserDto));
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Fetch roles by their names
            var roles = await _context.Roles
                .Where(r => createUserDto.Roles.Contains(r.Name))
                .ToListAsync();

            if (roles.Count != createUserDto.Roles.Count)
            {
                var invalidRoles = createUserDto.Roles.Except(roles.Select(r => r.Name));
                return BadRequest($"Invalid roles: {string.Join(", ", invalidRoles)}");
            }

            var user = new User
            {
                FirstName = createUserDto.FirstName,
                LastName = createUserDto.LastName,
                Email = createUserDto.Email,
                MobileNumber = createUserDto.MobileNumber,
                BirthDate = createUserDto.BirthDate,
                Gender = createUserDto.Gender,
                Category1 = createUserDto.Category1,
                Category2 = createUserDto.Category2,
                Category3 = createUserDto.Category3,
                Category4 = createUserDto.Category4,
                Date = DateTime.Now,
                RegistrationDate = createUserDto.RegistrationDate,
                SubscriptionExpirationDate = createUserDto.SubscriptionExpirationDate,
                RegistrationCode = createUserDto.RegistrationCode,
                Identifier = createUserDto.Identifier,
                IsVerified = createUserDto.IsVerified,
                IsActive = createUserDto.IsActive,  // Set IsActive value from DTO
                Roles = roles
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email,
                user.IsVerified,
                user.IsActive,  
                Roles = user.Roles.Select(r => r.Name).ToList()
            });
        }

        // Get users with optional filters for IsVerified, IsActive, search, gender, and role
        [HttpGet]
        public async Task<IActionResult> GetUsers(
    [FromQuery] string? search = null,  
    [FromQuery] string? gender = null,  
    [FromQuery] string? role = null,    
    [FromQuery] bool? isVerified = null,
    [FromQuery] bool? isActive = null)
        {
            
            var usersQuery = _context.Users.Include(u => u.Roles).AsQueryable();

            
            if (!string.IsNullOrEmpty(search))
            {
                usersQuery = usersQuery.Where(u =>
                    EF.Functions.Like((u.FirstName + " " + u.LastName), $"%{search}%") || 
                    EF.Functions.Like(u.FirstName, $"%{search}%") ||                      
                    EF.Functions.Like(u.LastName, $"%{search}%") ||                       
                    EF.Functions.Like(u.Email, $"%{search}%"));                           
            }

            
            if (!string.IsNullOrEmpty(gender))
            {
                usersQuery = usersQuery.Where(u => u.Gender != null && u.Gender.ToLower() == gender.ToLower());
            }

           
            if (!string.IsNullOrEmpty(role))
            {
                usersQuery = usersQuery.Where(u => u.Roles.Any(r => r.Name.ToLower() == role.ToLower()));
            }

            
            if (isVerified.HasValue)
            {
                usersQuery = usersQuery.Where(u => u.IsVerified == isVerified.Value);
            }

            
            if (isActive.HasValue)
            {
                usersQuery = usersQuery.Where(u => u.IsActive == isActive.Value);
            }

            
            var userList = await usersQuery.Select(u => new
            {
                u.Id,
                u.FirstName,
                u.LastName,
                u.Email,
                u.MobileNumber,
                u.Gender,
                u.IsVerified,
                u.IsActive,
                Roles = u.Roles.Select(r => r.Name).ToList(),
                RoleCount = u.Roles.Count  // Count the number of roles
            }).ToListAsync();

            return Ok(userList);
        }


        // Get a user by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _context.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound($"User with ID {id} not found");
            }

            var userDto = new
            {
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email,
                user.MobileNumber,
                user.BirthDate,
                user.Gender,
                user.Category1,
                user.Category2,
                user.Category3,
                user.Category4,
                user.RegistrationDate,
                user.SubscriptionExpirationDate,
                user.RegistrationCode,
                user.Identifier,
                user.IsVerified,
                user.IsActive,
                Roles = user.Roles.Select(r => r.Name).ToList()
            };

            return Ok(userDto);
        }

        // Update a user by ID
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto updateUserDto)
        {
            if (updateUserDto == null)
            {
                return BadRequest("User data is null");
            }

            var user = await _context.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound($"User with ID {id} not found");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            user.FirstName = updateUserDto.FirstName ?? user.FirstName;
            user.LastName = updateUserDto.LastName ?? user.LastName;
            user.Email = updateUserDto.Email ?? user.Email;
            user.MobileNumber = updateUserDto.MobileNumber ?? user.MobileNumber;
            user.BirthDate = updateUserDto.BirthDate ?? user.BirthDate;
            user.Gender = updateUserDto.Gender ?? user.Gender;
            user.Category1 = updateUserDto.Category1 ?? user.Category1;
            user.Category2 = updateUserDto.Category2 ?? user.Category2;
            user.Category3 = updateUserDto.Category3 ?? user.Category3;
            user.Category4 = updateUserDto.Category4 ?? user.Category4;
            user.IsVerified = updateUserDto.IsVerified ?? user.IsVerified;
            user.IsActive = updateUserDto.IsActive ?? user.IsActive;
            user.RegistrationDate = updateUserDto.RegistrationDate ?? user.RegistrationDate;
            user.SubscriptionExpirationDate = updateUserDto.SubscriptionExpirationDate ?? user.SubscriptionExpirationDate;
            user.RegistrationCode = updateUserDto.RegistrationCode ?? user.RegistrationCode;
            user.Identifier = updateUserDto.Identifier ?? user.Identifier;

            if (updateUserDto.Roles != null && updateUserDto.Roles.Any())
            {
                var roles = await _context.Roles
                    .Where(r => updateUserDto.Roles.Contains(r.Name))
                    .ToListAsync();

                if (roles.Count != updateUserDto.Roles.Count)
                {
                    var invalidRoles = updateUserDto.Roles.Except(roles.Select(r => r.Name));
                    return BadRequest($"Invalid roles: {string.Join(", ", invalidRoles)}");
                }

                user.Roles.Clear();
                foreach (var role in roles)
                {
                    user.Roles.Add(role);
                }
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email,
                user.IsVerified,
                user.IsActive,
                Roles = user.Roles.Select(r => r.Name).ToList()
            });
        }

        // Delete a user by ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound($"User with ID {id} not found");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok($"User with ID {id} has been deleted.");
        }
    }
}
