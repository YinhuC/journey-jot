﻿using AutoMapper;
using JourneyJot.Dto;
using JourneyJot.Intefaces;
using JourneyJot.Models;
using Microsoft.AspNetCore.Mvc;

namespace JourneyJot.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public PostController(IPostRepository postRepository, IUserRepository userRepository ,IMapper mapper)
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Post>))]
        public IActionResult GetPosts()
        {
            var posts = _mapper.Map<List<PostDto>>(_postRepository.GetAll());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(posts);
        }

        [HttpGet("{postId}")]
        [ProducesResponseType(200, Type = typeof(Post))]
        [ProducesResponseType(400)]
        public IActionResult GetPost(string postId)
        {
            if (!(Guid.TryParse(postId, out var guid) && _postRepository.Exists(guid)))
                return NotFound();

            var post = _mapper.Map<PostDto>(_postRepository.GetById(guid));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(post);
        }

        [HttpGet("{postId}/comments")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Comment>))]
        [ProducesResponseType(400)]
        public IActionResult GetPostComments(string postId)
        {
            if (!(Guid.TryParse(postId, out var guid) && _postRepository.Exists(guid)))
                return NotFound();

            var comments = _mapper.Map<List<CommentDto>>(_postRepository.GetPostComments(guid));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(comments);
        }

        [HttpGet("{postId}/categories")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        [ProducesResponseType(400)]
        public IActionResult GetPostCategories(string postId)
        {
            if (!(Guid.TryParse(postId, out var guid) && _postRepository.Exists(guid)))
                return NotFound();

            var categories = _mapper.Map<List<CategoryDto>>(_postRepository.GetPostCategories(guid));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(categories);
        }

        [HttpGet("{postId}/tags")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Tag>))]
        [ProducesResponseType(400)]
        public IActionResult GetPostTags(string postId)
        {
            if (!(Guid.TryParse(postId, out var guid) && _postRepository.Exists(guid)))
                return NotFound();

            var tags = _mapper.Map<List<TagDto>>(_postRepository.GetPostTags(guid));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(tags);
        }

        [HttpPost]
        [ProducesResponseType(202)]
        [ProducesResponseType(400)]
        public IActionResult CreatePost([FromBody] PostDtoCreate postDtoCreate)
        {
            if (postDtoCreate == null)
                return BadRequest(ModelState);

            if (!(Guid.TryParse(postDtoCreate.AuthorId, out var uid) && _userRepository.Exists(uid)))
            {
                ModelState.AddModelError("", "User does not exist");
                return StatusCode(404, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var post = _mapper.Map<Post>(postDtoCreate);

            if (!_postRepository.Create(post))
            {
                ModelState.AddModelError("", "Error while persisting to databse");
                return StatusCode(500, ModelState);
            }

            return Ok("Create Post Success");
        }
    }
}
