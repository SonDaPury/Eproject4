using AutoMapper;
using backend.Data;
using backend.Dtos;
using backend.Entities;
using backend.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nest;
using System.Net.WebSockets;

namespace backend.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswerController : ControllerBase
    {
        private readonly IAnswerService _answerService;
        private readonly IMapper _mapper;
        private readonly IElasticClient _elasticClient;
        private readonly IElasticSearchRepository _elasticSearchRepository;

        public AnswerController(IElasticSearchRepository elasticSearchRepository, IAnswerService answerService, IMapper mapper, IElasticClient elasticClient)
        {
            _answerService = answerService;
            _mapper = mapper;
            _elasticClient = elasticClient;
            _elasticSearchRepository = elasticSearchRepository;
        }
        [HttpGet("AddsomeData")]
        public async Task<IActionResult> AddData()
        {
            //         var searchResponse = _elasticSearchRepository.GetData<TopicElasticSearch>(s => s
            //    .Index("sources_index")
            //    .Query(q => q
            //        .MatchAll()
            //    )
            //);
            var newTopic = new TopicElasticSearch
            {
                TopicId = 11,
                TopicName = "Science2334234234234",
                subTopics = new List<SubTopcElasticSearch>
    {
        new SubTopcElasticSearch
        {
            SubTopicId = 101,
            SubTopicName = "Physics22ewewqewewr",
            sources = new List<SourcesElasticSearch>
            {
                new SourcesElasticSearch
                {
                    Id = 1001,
                    Title = "Quantum Mechanics",
                    Description = "Introduction to Quantum Mechanics",
                    Thumbnail = "thumb1.jpg",
                    Slug = "quantum-mechanics",
                    Status = 1,
                    Benefit = "Understand the basics of quantum mechanics",
                    Video_intro = "video1.mp4",
                    Price = 49.99,
                    Rating = "4.5",
                    UserId = 2001
                }
            }
        }
    }
            };
            var addData = _elasticSearchRepository.AddData<TopicElasticSearch>(newTopic, "sources_index", newTopic.TopicId.ToString());
            return Ok(addData);
            }
        [HttpGet("check")]
        public async Task<IActionResult> CheckConnection()
        {
            var pingResponse = await _elasticClient.PingAsync();
            if (pingResponse.IsValid)
            {
                return Ok("Connect OK");
            }
            else
            {
                return StatusCode(500, "Failed .");
            }
        }

        // POST: api/Answers
        [HttpPost]
        public async Task<ActionResult<AnswerDto>> CreateAnswer([FromBody] AnswerDto answerDto)
        {
            if (answerDto == null)
            {
                return BadRequest(new { message = "Answer data is required" });
            }
            try
            {
            var answer = _mapper.Map<Answer>(answerDto);
            var createdAnswer = await _answerService.CreateAsync(answer);
            var createdAnswerDto = _mapper.Map<AnswerDto>(createdAnswer);
            return CreatedAtAction(nameof(GetAnswer), new { id = createdAnswerDto.Id }, createdAnswerDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }

        // GET: api/Answers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnswerDto>>> GetAllAnswers()
        {
            try
            {
            var answers = await _answerService.GetAllAsync();
            var answerDtos = _mapper.Map<List<AnswerDto>>(answers);
            return Ok(answerDtos);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/Answers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AnswerDto>> GetAnswer(int id)
        {
            var answer = await _answerService.GetByIdAsync(id);
            if (answer == null)
            {
                return NotFound(new { message = $"Answer with ID {id} not found." });
            }
            var answerDto = _mapper.Map<AnswerDto>(answer);
            return Ok(answerDto);
        }

        // PUT: api/Answers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAnswer(int id, [FromBody] AnswerDto answerDto)
        {
            if (answerDto == null)
            {
                return BadRequest(new { message = "Invalid answer data" });
            }

            var answer = _mapper.Map<Answer>(answerDto);
            var updatedAnswer = await _answerService.UpdateAsync(id, answer);
            if (updatedAnswer == null)
            {
                return NotFound(new { message = $"Answer with ID {id} n ot found." });
            }
            return Ok(_mapper.Map<AnswerDto>(updatedAnswer));
        }

        // DELETE: api/Answers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnswer(int id)
        {
            var success = await _answerService.DeleteAsync(id);
            if (!success)
            {
                return NotFound(new { message = $"Answer with ID {id} not found." });
            }
            return NoContent(); // Using NoContent for successful delete as it's more appropriate than Ok in REST terms.
        }
    }
}
