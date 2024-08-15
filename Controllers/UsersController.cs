using Microsoft.AspNetCore.Mvc;
using robot_controller_api.Models;
using robot_controller_api.Persistence;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace robot_controller_api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserDataAccess _userDataAccess;
        private readonly PasswordHasher<UserModel> _passwordHasher;
        public UsersController(IUserDataAccess userDataAccess, PasswordHasher<UserModel> passwordHasher)
        {
            _userDataAccess = userDataAccess;
            _passwordHasher = passwordHasher;
        }

        // 1. GET /users
        [HttpGet]
        public IEnumerable<UserModel> GetAllUsers()
        {
            return _userDataAccess.GetUsers();
        }

        // 2. GET /users/admin
        [HttpGet("admin")]
        public IEnumerable<UserModel> GetAdminUsers()
        {
            return _userDataAccess.GetUsers().Where(u => u.Role == "Admin");
        }

        // 3. GET /users/{id}
        [HttpGet("{id}", Name = "GetUser")]
        public IActionResult GetUserById(int id)
        {
            var user = GetAllUsers().FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // 4. POST /users
        [HttpPost]
        public IActionResult AddUser(UserModel newUser)
        {
            if (newUser == null)
            {
                return BadRequest("The new user cannot be null.");
            }
            if (GetAllUsers().Any(u => u.Email == newUser.Email))
            {
                return Conflict("A user with this email already exists.");
            }
            var password = "sit331password";
            var user = new UserModel(
                Id: newUser.Id,
                email: newUser.Email,
                firstName: newUser.FirstName,
                lastName: newUser.LastName,
                passwordHash: newUser.PasswordHash,
                description: newUser.Description,
                role: newUser.Role,
                createdDate: DateTime.Now,
                modifiedDate: DateTime.Now
            );
            var hasher = new PasswordHasher<UserModel>();
            var pwHash = hasher.HashPassword(user, password);
            var pwVerificationResult = hasher.VerifyHashedPassword(user, pwHash, password);
            user.PasswordHash = pwHash;
            _userDataAccess.InsertUser(user);
            return CreatedAtRoute("GetUser", new { id = user.Id }, user);
        }

        // 5. PUT /users/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, UserModel updatedUser)
        {
            var user = GetAllUsers().FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            if (updatedUser == null)
            {
                return BadRequest("The updated user cannot be null.");
            }

            user.FirstName = updatedUser.FirstName;
            user.LastName = updatedUser.LastName;
            user.Description = updatedUser.Description;
            user.Role = updatedUser.Role;
            user.ModifiedDate = DateTime.Now;
            _userDataAccess.UpdateUser(user);
            return NoContent();
        }

        // 6. DELETE /users/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = GetAllUsers().FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            _userDataAccess.DeleteUser(id);
            return NoContent();
        }

        // 7. PATCH /users/{id}
        [HttpPatch("{id}")]
        public IActionResult UpdateUserEmailAndPassword(int id, LoginModel loginModel)
        {
            var user = GetAllUsers().FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            if (loginModel == null)
            {
                return BadRequest("The updated user cannot be null.");
            }
            user.Email = loginModel.Email;
            var hasher = new PasswordHasher<UserModel>();
            var pwHash = hasher.HashPassword(user, loginModel.Password);
            user.PasswordHash = pwHash;
            var pwVerificationResult = hasher.VerifyHashedPassword(user, pwHash, user.PasswordHash);
            user.ModifiedDate = DateTime.Now;
            _userDataAccess.UpdateUser(user);
            return NoContent();
        }
    }
}
