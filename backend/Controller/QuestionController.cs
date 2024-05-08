using AutoMapper;
using backend.Dtos;
using backend.Entities;
using backend.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;
        private readonly IMapper _mapper;

        public QuestionController(IQuestionService questionService, IMapper mapper)
        {
            _questionService = questionService;
            _mapper = mapper;
        }

        // POST: api/Questions
        [HttpPost]
        public async Task<ActionResult<QuestionDto>> CreateQuestion([FromBody] QuestionDto questionDto)
        {
            if (questionDto == null)
            {
                return BadRequest("Question data is required");
            }

            var question = _mapper.Map<Question>(questionDto);
            var createdQuestion = await _questionService.CreateAsync(question);
            var createdQuestionDto = _mapper.Map<QuestionDto>(createdQuestion);
            return CreatedAtAction(nameof(GetQuestion), new { id = createdQuestionDto.Id }, createdQuestionDto);
        }

        // GET: api/Questions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuestionDto>>> GetAllQuestions()
        {
            var questions = await _questionService.GetAllAsync();
            var questionDtos = _mapper.Map<List<QuestionDto>>(questions);
            return Ok(questionDtos);
        }

        // GET: api/Questions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<QuestionDto>> GetQuestion(int id)
        {
            var question = await _questionService.GetByIdAsync(id);
            if (question == null)
            {
                return NotFound($"Question with ID {id} not found.");
            }
            var questionDto = _mapper.Map<QuestionDto>(question);
            return Ok(questionDto);
        }

        // PUT: api/Questions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuestion(int id, [FromBody] QuestionDto questionDto)
        {
            if (questionDto == null)
            {
                return BadRequest("Invalid question data");
            }

            var question = _mapper.Map<Question>(questionDto);
            var updatedQuestion = await _questionService.UpdateAsync(id, question);
            if (updatedQuestion == null)
            {
                return NotFound($"Question with ID {id} not found.");
            }
            return Ok(_mapper.Map<QuestionDto>(updatedQuestion));
        }

        // DELETE: api/Questions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            var success = await _questionService.DeleteAsync(id);
            if (!success)
            {
                return NotFound($"Question with ID {id} not found.");
            }
            return NoContent(); // Using NoContent for successful delete as it's more appropriate than Ok in REST terms.
        }
    }
}
