using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using APIClassRoom.Model;
using APIClassRoom.Storage;

namespace APIClassRoom.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserStorage _userStorage;

        public UserController(UserStorage userStorage)
        {
            _userStorage = userStorage;
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
            var users = await _userStorage.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("teachers")]
        public async Task<ActionResult<List<User>>> GetTeachers()
        {
            var users = await _userStorage.GetAllUsersAsync();
            var teachers = users.FindAll(u => u.Role == UserRole.Teacher);
            return Ok(teachers);
        }

        [HttpGet("students")]
        public async Task<ActionResult<List<User>>> GetStudents()
        {
            var users = await _userStorage.GetAllUsersAsync();
            var students = users.FindAll(u => u.Role == UserRole.Student);
            return Ok(students);
        }
    }
}