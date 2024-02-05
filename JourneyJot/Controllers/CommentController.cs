using AutoMapper;
using JourneyJot.Dto;
using JourneyJot.Intefaces;
using JourneyJot.Models;
using JourneyJot.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace JourneyJot.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CommentController : Controller
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public CommentController(ICommentRepository commentRepository, 
            IUserRepository userRepository,
            IPostRepository postRepository,
            IMapper mapper)
        {
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            _postRepository = postRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Comment>))]
        public IActionResult GetComments()
        {
            var comments = _mapper.Map<List<CommentDto>>(_commentRepository.GetAll());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(comments);
        }

        [HttpGet("{commentId}")]
        [ProducesResponseType(200, Type = typeof(Comment))]
        [ProducesResponseType(400)]
        public IActionResult GetComment(string commentId)
        {
            if (!(Guid.TryParse(commentId, out var guid) && _commentRepository.Exists(guid)))
                return NotFound();

            var comment = _mapper.Map<CommentDto>(_commentRepository.GetById(guid));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(comment);
        }

        [HttpPost]
        [ProducesResponseType(202)]
        [ProducesResponseType(400)]
        public IActionResult CreateComment([FromBody] CommentDtoCreate commentDtoCreate)
        {
            if (commentDtoCreate == null)
                return BadRequest(ModelState);

            if (!(Guid.TryParse(commentDtoCreate.AuthorId, out var uid) && _userRepository.Exists(uid)))
            {
                ModelState.AddModelError("", "User does not exist");
                return StatusCode(404, ModelState);
            }
            if (!(Guid.TryParse(commentDtoCreate.PostId, out var pid) && _postRepository.Exists(pid)))
            {
                ModelState.AddModelError("", "Post does not exist");
                return StatusCode(404, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var comment = _mapper.Map<Comment>(commentDtoCreate);

            if (!_commentRepository.Create(comment))
            {
                ModelState.AddModelError("", "Error while creating comment");
                return StatusCode(500, ModelState);
            }

            return Ok("Create Comment Success");
        }

        [HttpPut("{commentId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public IActionResult UpdateComment([FromBody] CommentDtoUpdate commentDtoUpdate, string commentId)
        {
            if (commentDtoUpdate == null)
                return BadRequest(ModelState);

            if (!(Guid.TryParse(commentId, out var guid) && _commentRepository.Exists(guid)))
                return NotFound();

            // Check if author id of comment and current author id matches
            var comment = _commentRepository.GetById(guid);
            if (commentDtoUpdate.AuthorId != comment.AuthorId.ToString())
            {
                ModelState.AddModelError("", "Forbidden action");
                return StatusCode(403, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            comment.Content = commentDtoUpdate.Content;

            if (!_commentRepository.Update(comment))
            {
                ModelState.AddModelError("", "Error while updating comment");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{commentId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public IActionResult DeleteComment(string commentId, [FromQuery] string authorId)
        {
            if (!(Guid.TryParse(commentId, out var guid) && _commentRepository.Exists(guid)))
                return NotFound();

            if (!(Guid.TryParse(authorId, out var userId) && _userRepository.Exists(userId)))
            {
                ModelState.AddModelError("", "User Not Found");
                return StatusCode(404, ModelState);
            }
            var comment = _commentRepository.GetById(guid);

            if (comment.AuthorId != userId)
            {
                ModelState.AddModelError("", "Forbidden action");
                return StatusCode(403, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_commentRepository.Delete(comment))
            {
                ModelState.AddModelError("", "Error while deleting comment");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
