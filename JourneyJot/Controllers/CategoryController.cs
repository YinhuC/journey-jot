using AutoMapper;
using JourneyJot.Dto;
using JourneyJot.Intefaces;
using JourneyJot.Models;
using JourneyJot.Repository;
using Microsoft.AspNetCore.Mvc;

namespace JourneyJot.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        public IActionResult GetCategories()
        {
            var categories = _mapper.Map<List<CategoryDto>>(_categoryRepository.GetAll());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(categories);
        }

        [HttpGet("{categoryId}")]
        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType(400)]
        public IActionResult GetCategory(string categoryId)
        {
            if (!(Guid.TryParse(categoryId, out var guid) && _categoryRepository.Exists(guid)))
                return NotFound();

            var category = _mapper.Map<CategoryDto>(_categoryRepository.GetById(guid));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(category);
        }

        [HttpGet("{categoryId}/posts")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Post>))]
        [ProducesResponseType(400)]
        public IActionResult GetPostByCategory(string categoryId)
        {
            if (!(Guid.TryParse(categoryId, out var guid) && _categoryRepository.Exists(guid)))
                return NotFound();

            var posts = _mapper.Map<List<PostDto>>(_categoryRepository.GetPostsByCategory(guid));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(posts);
        }

        [HttpPost]
        [ProducesResponseType(202)]
        [ProducesResponseType(400)]
        public IActionResult CreateCategory([FromBody] CategoryDtoCreate categoryDtoCreate)
        {
            if (categoryDtoCreate == null)
                return BadRequest(ModelState);

            var name = _categoryRepository.GetCategoryByName(categoryDtoCreate.Name);

            if (name != null)
            {
                ModelState.AddModelError("", "Category already exists");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = _mapper.Map<Category>(categoryDtoCreate);

            if (!_categoryRepository.Create(category))
            {
                ModelState.AddModelError("", "Error while persisting to databse");
                return StatusCode(500, ModelState);
            }

            return Ok("Create Category Success");
        }
    }
}
