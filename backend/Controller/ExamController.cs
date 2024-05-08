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
    public class ExamController : ControllerBase
    {
        private readonly IExamService _examService;
        private readonly IMapper _mapper;

        public ExamController(IExamService examService, IMapper mapper)
        {
            _examService = examService;
            _mapper = mapper;
        }

        // POST: api/Exams
        [HttpPost]
        public async Task<ActionResult<ExamDto>> CreateExam([FromBody] ExamDto examDto)
        {
            if (examDto == null)
            {
                return BadRequest("Exam data is required");
            }

            var exam = _mapper.Map<Exam>(examDto);
            var createdExam = await _examService.CreateAsync(exam);
            var createdExamDto = _mapper.Map<ExamDto>(createdExam);
            return CreatedAtAction(nameof(GetExam), new { id = createdExamDto.Id }, createdExamDto);
        }

        // GET: api/Exams
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExamDto>>> GetAllExams()
        {
            var exams = await _examService.GetAllAsync();
            var examDtos = _mapper.Map<List<ExamDto>>(exams);
            return Ok(examDtos);
        }

        // GET: api/Exams/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ExamDto>> GetExam(int id)
        {
            var exam = await _examService.GetByIdAsync(id);
            if (exam == null)
            {
                return NotFound($"Exam with ID {id} not found.");
            }
            var examDto = _mapper.Map<ExamDto>(exam);
            return Ok(examDto);
        }

        // PUT: api/Exams/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExam(int id, [FromBody] ExamDto examDto)
        {
            if (examDto == null )
            {
                return BadRequest("Invalid exam data");
            }

            var exam = _mapper.Map<Exam>(examDto);
            var updatedExam = await _examService.UpdateAsync(id, exam);
            if (updatedExam == null)
            {
                return NotFound($"Exam with ID {id} not found.");
            }
            return Ok(_mapper.Map<ExamDto>(updatedExam));
        }

        // DELETE: api/Exams/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExam(int id)
        {
            var success = await _examService.DeleteAsync(id);
            if (!success)
            {
                return NotFound($"Exam with ID {id} not found.");
            }
            return NoContent(); // Using NoContent for successful delete as it's more appropriate than Ok in REST terms.
        }
    }
}
