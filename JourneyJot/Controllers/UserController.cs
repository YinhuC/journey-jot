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
    public class UserController: Controller
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
            if(!(Guid.TryParse(userId, out var guid) && _userRepository.Exists(guid)))
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

    }
}

