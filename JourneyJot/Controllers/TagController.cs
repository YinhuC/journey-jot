﻿using AutoMapper;
using JourneyJot.Dto;
using JourneyJot.Intefaces;
using JourneyJot.Models;
using JourneyJot.Repository;
using Microsoft.AspNetCore.Mvc;

namespace JourneyJot.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TagController : Controller
    {

        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;

        public TagController(ITagRepository tagRepository, IMapper mapper)
        {
            _tagRepository = tagRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Tag>))]
        public IActionResult GetTags()
        {
            var Tags = _mapper.Map<List<TagDto>>(_tagRepository.GetAll());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Tags);
        }

        [HttpGet("{tagId}")]
        [ProducesResponseType(200, Type = typeof(Tag))]
        [ProducesResponseType(400)]
        public IActionResult GetTag(string tagId)
        {
            if (!(Guid.TryParse(tagId, out var guid) && _tagRepository.Exists(guid)))
                return NotFound();

            var Tag = _mapper.Map<TagDto>(_tagRepository.GetById(guid));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Tag);
        }

        [HttpGet("{tagId}/posts")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Post>))]
        [ProducesResponseType(400)]
        public IActionResult GetPostByTag(string tagId)
        {
            if (!(Guid.TryParse(tagId, out var guid) && _tagRepository.Exists(guid)))
                return NotFound();

            var posts = _mapper.Map<List<PostDto>>(_tagRepository.GetPostByTag(guid));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(posts);
        }

        [HttpPost]
        [ProducesResponseType(202)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        public IActionResult CreateTag([FromBody] TagDtoCreate tagDtoCreate)
        {
            if (tagDtoCreate == null)
                return BadRequest(ModelState);

            var name = _tagRepository.GetTagByName(tagDtoCreate.Name);

            if (name != null)
            {
                ModelState.AddModelError("", "Tag already exists");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var tag = _mapper.Map<Tag>(tagDtoCreate);

            if (!_tagRepository.Create(tag))
            {
                ModelState.AddModelError("", "Error while creating tag");
                return StatusCode(500, ModelState);
            }

            return Ok("Create Tag Success");
        }

        [HttpPut("{tagId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateTag([FromBody] TagDtoUpdate tagDtoUpdate, string tagId)
        {
            if (tagDtoUpdate == null)
                return BadRequest(ModelState);

            if (!(Guid.TryParse(tagId, out var guid) && _tagRepository.Exists(guid)))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var tag = _mapper.Map<Tag>(tagDtoUpdate);
            tag.Id = guid;

            if (!_tagRepository.Update(tag))
            {
                ModelState.AddModelError("", "Error while updating tag");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{tagId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteTag(string tagId)
        {
            if (!(Guid.TryParse(tagId, out var guid) && _tagRepository.Exists(guid)))
                return NotFound();

            var tag = _tagRepository.GetById(guid);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_tagRepository.Delete(tag))
            {
                ModelState.AddModelError("", "Error while deleting tag");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
