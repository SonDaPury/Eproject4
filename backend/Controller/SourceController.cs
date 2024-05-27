using AutoMapper;
using backend.Attributes;
using backend.Base;
using backend.Data;
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
        private readonly IElasticSearchRepository _elasticsearchRepository;

        public SourceController(ISourceService sourceService, IMapper mapper, IElasticSearchRepository elasticSearchRepository)
        {
            _sourceService = sourceService;
            _mapper = mapper;
            _elasticsearchRepository = elasticSearchRepository;
        }

        // POST: api/Sources
        [HttpPost]
        public async Task<ActionResult<SourceDto>> CreateSource([FromBody] SourceDto sourceDto)
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
            var sources = await _sourceService.GetAllAsync();

            // Ánh xạ từ danh sách sources sang danh sách SourceDto
            var sourceDtos = _mapper.Map<List<SourceDto>>(sources);

            // Gửi lại response bao gồm cả danh sách SourceDto và tổng số lượng (nếu cần)
            return Ok(sourceDtos);
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
        public async Task<IActionResult> UpdateSource(int id, [FromBody] SourceDto sourceDto)
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
            var script = @"
  for (int i = 0; i < ctx._source.subTopics.size(); i++) {
    if (ctx._source.subTopics[i].SubTopicId == params.SubTopicId) {
      for (int j = 0; j < ctx._source.subTopics[i].sources.size(); j++) {
        if (ctx._source.subTopics[i].sources[j].Id == params.id) {
          ctx._source.subTopics[i].sources[j].Title = params.Title;
          ctx._source.subTopics[i].sources[j].Description = params.Description;
          ctx._source.subTopics[i].sources[j].Thumbnail = params.Thumbnail;
          ctx._source.subTopics[i].sources[j].Slug = params.Slug;
          ctx._source.subTopics[i].sources[j].Status = params.Status;
          ctx._source.subTopics[i].sources[j].Benefit = params.Benefit;
          ctx._source.subTopics[i].sources[j].Video_intro = params.Video_intro;
          ctx._source.subTopics[i].sources[j].Price = params.Price;
          ctx._source.subTopics[i].sources[j].Rating = params.Rating;
          ctx._source.subTopics[i].sources[j].UserId = params.UserId;
          break;
        }
      }
      break;
    }
  }
";

            var scriptParams = new Dictionary<string, object>
{
    { "SubTopicId", 102 },
    { "id", 1008 },
    { "Title", "Demo5 Title" },
    { "Description", "Demo2 Description" },
    { "Thumbnail", "demo-thumbnail.jpg" },
    { "Slug", "demo3-slug" },
    { "Status", 2 },
    { "Benefit", "Demo Benefit" },
    { "Video_intro", "demo-intro.mp4" },
    { "Price", 0 },
    { "Rating", "3" },
    { "UserId", 2003 }
};

            var updateResponse = _elasticsearchRepository.UpdateScript(id.ToString(), u => u
               .Index("sources_index")
               .Script(s => s
                  .Source(script)
                  .Params(scriptParams)
               )
            );

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
