using System;
using AutoMapper;
using JourneyJot.Dto;
using JourneyJot.Intefaces;
using JourneyJot.Models;
using Microsoft.AspNetCore.Mvc;

namespace JourneyJot.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        public IActionResult GetUsers()
        {
            var users = _mapper.Map<List<UserDto>>(_userRepository.GetAll());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(users);
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(400)]
        public IActionResult GetUser(string userId)
        {
            if (!(Guid.TryParse(userId, out var guid) && _userRepository.Exists(guid)))
                return NotFound();

            var user = _mapper.Map<UserDto>(_userRepository.GetById(guid));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(user);
        }

        [HttpGet("{userId}/posts")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(400)]
        public IActionResult GetPostsByUser(string userId)
        {
            if (!(Guid.TryParse(userId, out var guid) && _userRepository.Exists(guid)))
                return NotFound();

            var posts = _mapper.Map<List<PostDto>>(_userRepository.GetPostsByUser(guid));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(posts);
        }

        [HttpGet("{userId}/comments")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Comment>))]
        [ProducesResponseType(400)]
        public IActionResult GetCommentsByUser(string userId)
        {
            if (!(Guid.TryParse(userId, out var guid) && _userRepository.Exists(guid)))
                return NotFound();

            var comments = _mapper.Map<List<CommentDto>>(_userRepository.GetCommentsByUser(guid));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(comments);
        }

        [HttpPost]
        [ProducesResponseType(202)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        public IActionResult CreateUser([FromBody] UserDtoCreate userDtoCreate)
        {
            if (userDtoCreate == null)
                return BadRequest(ModelState);

            var username = _userRepository.GetByUsername(userDtoCreate.Username);
            var email = _userRepository.GetByEmail(userDtoCreate.Email);

            if (username != null)
            {
                ModelState.AddModelError("", "Username already exists");
                return StatusCode(422, ModelState);
            }
            if (email != null)
            {
                ModelState.AddModelError("", "Email already exists in database");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = _mapper.Map<User>(userDtoCreate);

            if (!_userRepository.Create(user))
            {
                ModelState.AddModelError("", "Error while persisting to database");
                return StatusCode(500, ModelState);
            }

            return Ok("Create User Success");
        }

        [HttpPut("{userId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateUser([FromBody] UserDtoUpdate userDtoUpdate, string userId)
        {
            if (userDtoUpdate == null)
                return BadRequest(ModelState);

            if (!(Guid.TryParse(userId, out var guid) && _userRepository.Exists(guid)))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = _mapper.Map<User>(userDtoUpdate);
            user.Id = guid;

            if (!_userRepository.Update(user))
            {
                ModelState.AddModelError("", "Error while persisting to database");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

    }
}

