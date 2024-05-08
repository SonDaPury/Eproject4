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
    public class AnswerController : ControllerBase
    {
        private readonly IAnswerService _answerService;
        private readonly IMapper _mapper;

        public AnswerController(IAnswerService answerService, IMapper mapper)
        {
            _answerService = answerService;
            _mapper = mapper;
        }

        // POST: api/Answers
        [HttpPost]
        public async Task<ActionResult<AnswerDto>> CreateAnswer([FromBody] AnswerDto answerDto)
        {
            if (answerDto == null)
            {
                return BadRequest("Answer data is required");
            }

            var answer = _mapper.Map<Answer>(answerDto);
            var createdAnswer = await _answerService.CreateAsync(answer);
            var createdAnswerDto = _mapper.Map<AnswerDto>(createdAnswer);
            return CreatedAtAction(nameof(GetAnswer), new { id = createdAnswerDto.Id }, createdAnswerDto);
        }

        // GET: api/Answers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnswerDto>>> GetAllAnswers()
        {
            var answers = await _answerService.GetAllAsync();
            var answerDtos = _mapper.Map<List<AnswerDto>>(answers);
            return Ok(answerDtos);
        }

        // GET: api/Answers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AnswerDto>> GetAnswer(int id)
        {
            var answer = await _answerService.GetByIdAsync(id);
            if (answer == null)
            {
                return NotFound($"Answer with ID {id} not found.");
            }
            var answerDto = _mapper.Map<AnswerDto>(answer);
            return Ok(answerDto);
        }

        // PUT: api/Answers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAnswer(int id, [FromBody] AnswerDto answerDto)
        {
            if (answerDto == null || id != answerDto.Id)
            {
                return BadRequest("Invalid answer data");
            }

            var answer = _mapper.Map<Answer>(answerDto);
            var updatedAnswer = await _answerService.UpdateAsync(id, answer);
            if (updatedAnswer == null)
            {
                return NotFound($"Answer with ID {id} not found.");
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
                return NotFound($"Answer with ID {id} not found.");
            }
            return NoContent(); // Using NoContent for successful delete as it's more appropriate than Ok in REST terms.
        }
    }
}
