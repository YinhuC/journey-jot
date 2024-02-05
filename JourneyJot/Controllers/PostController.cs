using AutoMapper;
using JourneyJot.Dto;
using JourneyJot.Intefaces;
using JourneyJot.Models;
using JourneyJot.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

namespace JourneyJot.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITagRepository _tagRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;

        public PostController(IPostRepository postRepository, 
            IUserRepository userRepository,
            ICategoryRepository categoryRepository,
            ITagRepository tagRepository,
            ICommentRepository commentRepository,
            IMapper mapper)
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
            _categoryRepository = categoryRepository;
            _tagRepository = tagRepository;
            _commentRepository = commentRepository;
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

            // Check user exists
            if (!(Guid.TryParse(postDtoCreate.AuthorId, out var pid) && _userRepository.Exists(pid)))
            {
                ModelState.AddModelError("", "User does not exist");
                return StatusCode(404, ModelState);
            }

            // Check categories and tags exist
            var invalidCategories = postDtoCreate?.Categories?.Where(cname => !_categoryRepository.ExistsByName(cname));
            if (invalidCategories.Any())
            {
                ModelState.AddModelError("InvalidCategories", "One or more categories do not exist");
                ModelState.AddModelError("InvalidCategoriesList", string.Join(",", invalidCategories));
                return StatusCode(404, ModelState);
            }
            var invalidTags = postDtoCreate?.Tags?.Where(tname => !_tagRepository.ExistsByName(tname));
            if (invalidTags.Any())
            {
                ModelState.AddModelError("InvalidTags", "One or more tags do not exist");
                ModelState.AddModelError("InvalidTagsList", string.Join(",", invalidTags));
                return StatusCode(404, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var post = _mapper.Map<Post>(postDtoCreate);

            // Add PostCategories
            ICollection<PostCategory> postCategoryList = new List<PostCategory>();
            foreach (var cname in postDtoCreate.Categories)
            {
                var postCategory = new PostCategory()
                {
                    Post = post,
                    Category = _categoryRepository.GetCategoryByName(cname)
                };
                postCategoryList.Add(postCategory);
            }
            post.PostCategories = postCategoryList;

            // Add PostTags
            ICollection<PostTag> postTagList = new List<PostTag>();
            foreach (var tname in postDtoCreate.Tags)
            {
                var postTag = new PostTag()
                {
                    Post = post,
                    Tag = _tagRepository.GetTagByName(tname)
                };
                postTagList.Add(postTag);
            }
            post.PostTags = postTagList;

            if (!_postRepository.Create(post))
            {
                ModelState.AddModelError("", "Error while creating post");
                return StatusCode(500, ModelState);
            }

            return Ok("Create Post Success");
        }

        [HttpPut("{postId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public IActionResult UpdatePost([FromBody] PostDtoUpdate postDtoUpdate, string postId)
        {
            if (postDtoUpdate == null)
                return BadRequest(ModelState);

            if (!(Guid.TryParse(postId, out var guid) && _postRepository.Exists(guid)))
                return NotFound();

            // Check if author id of comment and current author id matches
            var post = _postRepository.GetById(guid);
            if (postDtoUpdate.AuthorId != post.AuthorId.ToString())
            {
                ModelState.AddModelError("", "Forbidden action");
                return StatusCode(403, ModelState);
            }

            if (postDtoUpdate.Categories != null)
            {
                var invalidCategories = postDtoUpdate?.Categories?.Where(cname => !_categoryRepository.ExistsByName(cname));
                if (invalidCategories.Any())
                {
                    ModelState.AddModelError("InvalidCategories", "One or more categories do not exist");
                    ModelState.AddModelError("InvalidCategoriesList", string.Join(",", invalidCategories));
                    return StatusCode(404, ModelState);
                }
                var currCategories = _postRepository.GetPostCategories(guid);
                // Add PostCategories
                ICollection<PostCategory> postCategoryList = new List<PostCategory>();
                foreach (var cname in postDtoUpdate.Categories)
                {
                    var category = _categoryRepository.GetCategoryByName(cname);
                    if (!currCategories.Any(c => c.Id == category.Id))
                    {
                        var postCategory = new PostCategory()
                        {
                            Post = post,
                            Category = category
                        };
                        postCategoryList.Add(postCategory);
                    }
                }
                post.PostCategories = postCategoryList;
            }

            if (postDtoUpdate.Tags != null)
            {
                var invalidTags = postDtoUpdate?.Tags?.Where(tname => !_tagRepository.ExistsByName(tname));
                if (invalidTags.Any())
                {
                    ModelState.AddModelError("InvalidTags", "One or more tags do not exist");
                    ModelState.AddModelError("InvalidTagsList", string.Join(",", invalidTags));
                    return StatusCode(404, ModelState);
                }
                var currTags = _postRepository.GetPostTags(guid);
                // Add PostTags
                ICollection<PostTag> postTagList = new List<PostTag>();
                foreach (var tname in postDtoUpdate.Tags)
                {
                    var tag = _tagRepository.GetTagByName(tname);
                    if (!currTags.Any(t => t.Id == tag.Id))
                    {
                        var postTag = new PostTag()
                        {
                            Post = post,
                            Tag = tag
                        };
                        postTagList.Add(postTag);
                    }
                }
                post.PostTags = postTagList;
            }

            if (postDtoUpdate.Content != null)
                post.Content = postDtoUpdate.Content;

            if (postDtoUpdate.Title != null)
                post.Title = postDtoUpdate.Title;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_postRepository.Update(post))
            {
                ModelState.AddModelError("", "Error while updating post");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{postId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public IActionResult DeletePost(string postId, [FromQuery] string authorId)
        {
            if (!(Guid.TryParse(postId, out var guid) && _postRepository.Exists(guid)))
                return NotFound();

            if (!(Guid.TryParse(authorId, out var userId) && _userRepository.Exists(userId)))
            {
                ModelState.AddModelError("", "User Not Found");
                return StatusCode(404, ModelState);
            }
            var post = _postRepository.GetById(guid);
            var comments = _postRepository.GetPostComments(guid);

            if (post.AuthorId != userId)
            {
                ModelState.AddModelError("", "Forbidden action");
                return StatusCode(403, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_commentRepository.DeleteComments(comments))
            {
                ModelState.AddModelError("", "Error while deleting comments");
                return StatusCode(500, ModelState);
            }

            if (!_postRepository.Delete(post))
            {
                ModelState.AddModelError("", "Error while deleting post");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

    }
}
