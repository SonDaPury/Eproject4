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
    public class LessonController : ControllerBase
    {
        private readonly ILessonService _lessonService;
        private readonly IMapper _mapper;

        public LessonController(ILessonService lessonService, IMapper mapper)
        {
            _lessonService = lessonService;
            _mapper = mapper;
        }

        // POST: api/Lessons
        [HttpPost]
        public async Task<ActionResult<LessonDto>> CreateLesson([FromBody] LessonDto lessonDto)
        {
            if (lessonDto == null)
            {
                return BadRequest("Lesson data is required");
            }

            var lesson = _mapper.Map<Lesson>(lessonDto);
            var createdLesson = await _lessonService.CreateAsync(lesson);
            var createdLessonDto = _mapper.Map<LessonDto>(createdLesson);
            return CreatedAtAction(nameof(GetLesson), new { id = createdLessonDto.Id }, createdLessonDto);
        }

        // GET: api/Lessons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LessonDto>>> GetAllLessons()
        {
            var lessons = await _lessonService.GetAllAsync();
            var lessonDtos = _mapper.Map<List<LessonDto>>(lessons);
            return Ok(lessonDtos);
        }

        // GET: api/Lessons/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LessonDto>> GetLesson(int id)
        {
            var lesson = await _lessonService.GetByIdAsync(id);
            if (lesson == null)
            {
                return NotFound($"Lesson with ID {id} not found.");
            }
            var lessonDto = _mapper.Map<LessonDto>(lesson);
            return Ok(lessonDto);
        }

        // update
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLesson(int id, [FromBody] LessonDto lessonDto)
        {
            if (lessonDto == null)
            {
                return BadRequest("Invalid lesson data");
            }
            var data = _mapper.Map<Lesson>(lessonDto);
            var updatedLesson = await _lessonService.UpdateAsync(id, data);
            if (updatedLesson == null)
            {
                return NotFound($"Lesson with ID {id} not found.");
            }
            return Ok(_mapper.Map<LessonDto>(updatedLesson));
        }

        // DELETE: api/Lessons/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLesson(int id)
        {
            var success = await _lessonService.DeleteAsync(id);
            if (!success)
            {
                return NotFound($"Lesson with ID {id} not found.");
            }
            return Ok(new { message = "Delete successfuly!!!" });
        }
    }
}
