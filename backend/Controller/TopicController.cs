using AutoMapper;
using backend.Attributes;
using backend.Dtos;
using backend.Entities;
using backend.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicController : ControllerBase
    {
        private readonly ITopicService _topicService;
        private readonly IMapper _mapper;
        public TopicController(ITopicService topicService, IMapper mapper)
        {
            _topicService = topicService;
            _mapper = mapper;
        }

        // POST: api/Topics
        [HttpPost]
        public async Task<ActionResult<Topic>> CreateTopic([FromBody] TopicDto topicDto)
        {
            if (topicDto == null)
            {
                return BadRequest(new { message = "Topic data is required" });
            }
            var data = _mapper.Map<Topic>(topicDto);
            var createdTopic = await _topicService.CreateAsync(data);
            return CreatedAtAction("GetTopic", new { id = createdTopic.Id }, createdTopic);
        }

        // GET: api/Topics
        [HttpGet]
        [JwtAuthorize("user")]
        public async Task<IActionResult> GetAllTopics()
        {
            var topics = await _topicService.GetAllAsync();
            var topicDto = _mapper.Map<List<TopicDto>>(topics);
            return Ok(topicDto);
        }

        // GET: api/Topics/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Topic>> GetTopic(int id)
        {
            var topic = await _topicService.GetByIdAsync(id);
            if (topic == null)
            {
                return NotFound(new { message = $"Topic with ID {id} not found." });
            }
            return Ok(topic);
        }

        // PUT: api/Topics/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTopic(int id, [FromBody] TopicDto topicDto)
        {
            if (topicDto == null )
            {
                return BadRequest(new { message = "Invalid topic data" });
            }
            var data = _mapper.Map<Topic>(topicDto);
            var updatedTopic = await _topicService.UpdateAsync(id, data);
            if (updatedTopic == null)
            {
                return NotFound(new { message = $"Topic with ID {id} not found." });
            }
            return Ok(updatedTopic);
        }

        // DELETE: api/Topics/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTopic(int id)
        {
            var success = await _topicService.DeleteAsync(id);
            if (!success)
            {
                return NotFound(new { message = $"Topic with ID {id} not found." });
            }
            return Ok(new {mesage = "delete successfuly !!!"});
        }
    }
}
