using AutoMapper;
using backend.Attributes;
using backend.Base;
using backend.Dtos;
using backend.Entities;
using backend.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [JwtAuthorize("user", "admin")]
    public class SourceController : ControllerBase
    {
        private readonly ISourceService _sourceService;
        private readonly IMapper _mapper;

        public SourceController(ISourceService sourceService, IMapper mapper)
        {
            _sourceService = sourceService;
            _mapper = mapper;
        }

        // POST: api/Sources
        [HttpPost]
        public async Task<ActionResult<SourceDto>> CreateSource([FromForm] SourceDto sourceDto)
        {
            if (sourceDto == null)
            {
                return BadRequest(new { message = "Source data is required" });
            }

            var source = _mapper.Map<Source>(sourceDto);
            var createdSource = await _sourceService.CreateAsync(source);
            var createdSourceDto = _mapper.Map<SourceDto>(createdSource);
            return CreatedAtAction(nameof(GetSource), new { id = createdSource.Id }, createdSourceDto);
        }

        // GET: api/Sources
        [HttpGet("pagination")]
        public async Task<ActionResult<IEnumerable<SourceDto>>> GetAllSources([FromQuery] Pagination pagination)
        {
            var (sources, totalCount) = await _sourceService.GetAllAsync(pagination);

            // Ánh xạ từ danh sách sources sang danh sách SourceDto
            var sourceDtos = _mapper.Map<List<SourceDto>>(sources);

            // Gửi lại response bao gồm cả danh sách SourceDto và tổng số lượng (nếu cần)
            return Ok(new { TotalCount = totalCount, Items = sourceDtos });
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SourceDto>>> GetAllSources()
        {
            var sources= await _sourceService.GetAllAsync();

            // Ánh xạ từ danh sách sources sang danh sách SourceDto
            var sourceDtos = _mapper.Map<List<SourceDto>>(sources);

            // Gửi lại response bao gồm cả danh sách SourceDto và tổng số lượng (nếu cần)
            return Ok( sourceDtos );
        }

        // GET: api/Sources/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SourceDto>> GetSource(int id)
        {
            var source = await _sourceService.GetByIdAsync(id);
            if (source == null)
            {
                return NotFound(new { message = $"Source with ID {id} not found." });
            }
            var sourceDto = _mapper.Map<SourceDto>(source);
            return Ok(sourceDto);
        }

        // PUT: api/Sources/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSource(int id, [FromForm] SourceDto sourceDto)
        {
            if (sourceDto == null)
            {
                return BadRequest(new { message = "Invalid Source data" });
            }
            var source = _mapper.Map<Source>(sourceDto);
            var updatedSource = await _sourceService.UpdateAsync(id, source);
            if (updatedSource == null)
            {
                return NotFound(new { message = $"Source with ID {id} not found." });
            }
            return Ok(_mapper.Map<SourceDto>(updatedSource));
        }

        // DELETE: api/Sources/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSource(int id)
        {
            var success = await _sourceService.DeleteAsync(id);
            if (!success)
            {
                return NotFound(new { message = $"Source with ID {id} not found." });
            }
            return NoContent(); // Using NoContent for successful delete as it's more appropriate than Ok in REST terms.
        }
    }
}
