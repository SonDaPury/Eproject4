﻿using AutoMapper;
using backend.Dtos;
using backend.Entities;
using backend.Service.ElasticSearch;
using backend.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubTopicController : ControllerBase
    {
        private readonly ISubTopicService _subTopicService;
        private readonly IMapper _mapper;
        private readonly ISubtopicsElasticSearch subtopicsElasticSearch;

        public SubTopicController(ISubTopicService subTopicService, IMapper mapper, ISubtopicsElasticSearch _subtopicsElasticSearch)
        {
            _subTopicService = subTopicService;
            _mapper = mapper;
            subtopicsElasticSearch = _subtopicsElasticSearch;
        }

        // POST: api/SubTopics
        [HttpPost]
        public async Task<ActionResult<SubTopicDto>> CreateSubTopic([FromBody] SubTopicDto subTopicDto)
        {
            if (subTopicDto == null)
            {
                return BadRequest(new { message = "SubTopic data is required" });
            }
            var subTopic = _mapper.Map<SubTopic>(subTopicDto);
            /*  var createdSubTopic = await _subTopicService.CreateAsync(subTopic);
              var createdSubTopicDto = _mapper.Map<SubTopicDto>(createdSubTopic);*/
            //return CreatedAtAction(nameof(GetSubTopic), new { id = createdSubTopic.Id }, createdSubTopicDto);
            var check = subtopicsElasticSearch.AddSources(subTopicDto);
            return Ok(check);
        }

        // GET: api/SubTopics
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubTopicDto>>> GetAllSubTopics()
        {
            var subTopics = await _subTopicService.GetAllAsync();
            var subTopicDtos = _mapper.Map<List<SubTopicDto>>(subTopics);
            return Ok(subTopicDtos);
        }

        // GET: api/SubTopics/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SubTopicDto>> GetSubTopic(int id)
        {
            var subTopic = await _subTopicService.GetByIdAsync(id);
            if (subTopic == null)
            {
                return NotFound(new { message = $"SubTopic with ID {id} not found." });
            }
            var subTopicDto = _mapper.Map<SubTopicDto>(subTopic);
            return Ok(subTopicDto);
        }

        // PUT: api/SubTopics/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubTopic(int id, [FromBody] SubTopicDto subTopicDto)
        {
            if (subTopicDto == null)
            {
                return BadRequest(new { message = "Invalid SubTopic data" });
            }
            var subTopic = _mapper.Map<SubTopic>(subTopicDto);
            var updatedSubTopic = await _subTopicService.UpdateAsync(id, subTopic);
            if (updatedSubTopic == null)
            {
                return NotFound(new { message = $"SubTopic with ID {id} not found." });
            }
            return Ok(_mapper.Map<SubTopicDto>(updatedSubTopic));
        }

        // DELETE: api/SubTopics/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubTopic(int id)
        {
            var success = await _subTopicService.DeleteAsync(id);
            if (!success)
            {
                return NotFound(new { message = $"SubTopic with ID {id} not found." });
            }
            return NoContent(); // Using NoContent for successful delete as it's more appropriate than Ok in REST terms.
        }
    }
}
