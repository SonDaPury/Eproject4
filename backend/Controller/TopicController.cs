using AutoMapper;
using backend.Attributes;
using backend.Base;
using backend.Dtos;
using backend.Entities;
using backend.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    //[JwtAuthorize("user", "admin")]
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
        [AllowAnonymous]
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
        [HttpGet("pagination")]
        //[JwtAuthorize("user")]
        public async Task<IActionResult> GetAllTopics([FromQuery]Pagination pagination)
        {
            var topics = await _topicService.GetAllAsync(pagination);
            var topicDto = _mapper.Map<List<TopicDto>>(topics.Item1);
            return Ok(new { TotalCount = topics.Item2, Items = topicDto });
        }
        [HttpGet]
        //[JwtAuthorize("user")]
        public async Task<IActionResult> GetAllTopics()
        {
            var topics = await _topicService.GetAllAsync();
            var topicDto = _mapper.Map<List<TopicDto>>(topics);
            return Ok(new { Items = topicDto });
        }
        // GET: api/Topics/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Topic>> GetTopic(int id)
        {
            var topic = await _topicService.GetByIdAsync(id);
            var topicDto = _mapper.Map<TopicDto>(topic);
            if (topic == null)
            {
                return NotFound(new { message = $"Topic with ID {id} not found." });
            }
            return Ok(topicDto);
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
