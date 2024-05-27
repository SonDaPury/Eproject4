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
    public class SubTopicController : ControllerBase
    {
        private readonly ISubTopicService _subTopicService;
        private readonly IMapper _mapper;
        private readonly IElasticSearchRepository _elasticsearchRepository;

        public SubTopicController(ISubTopicService subTopicService, IMapper mapper, IElasticSearchRepository elasticSearchRepository)
        {
            _subTopicService = subTopicService;
            _mapper = mapper;
            _elasticsearchRepository = elasticSearchRepository;
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
            var createdSubTopic = await _subTopicService.CreateAsync(subTopic);
            var createdSubTopicDto = _mapper.Map<SubTopicDto>(createdSubTopic);
            _elasticsearchRepository.UpdateScript(createdSubTopic.Id.ToString(), u => u.Index("sources_index")
            .Script(s => s.Source("ctx._source.subTopics.add(params.subtopic)")
            .Params(p => p.Add("subTopics", new SubTopcElasticSearch
            {
                SubTopicId = subTopicDto.Id,
                SubTopicName = subTopicDto.SubTopicName,
                sources = new List<SourcesElasticSearch>()
            }))));
            return CreatedAtAction(nameof(GetSubTopic), new { id = createdSubTopic.Id }, createdSubTopicDto);
        }

        // GET: api/SubTopics
        [HttpGet("pagination")]
        public async Task<ActionResult<IEnumerable<SubTopicDto>>> GetAllSubTopics([FromQuery] Pagination pagination)
        {
            var subTopics = await _subTopicService.GetAllAsync(pagination);
            var subTopicDtos = _mapper.Map<List<SubTopicDto>>(subTopics.Item1);
            return Ok(new { TotalCount = subTopics.Item2, Items = subTopicDtos });
        }

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
            var scriptSource = @"
    for (int i = 0; i < ctx._source.subTopics.size(); i++) {
        if (ctx._source.subTopics[i].SubTopicId == params.SubTopicId) {
            ctx._source.subTopics[i].SubTopicName = params.SubTopicName;
        }
    }";

            var scriptParams = new Dictionary<string, object>
{
    { "SubTopicId", updatedSubTopic.Id },
    { "SubTopicName", updatedSubTopic.SubTopicName }
};

            var updateResponse = _elasticsearchRepository.UpdateScript(updatedSubTopic.TopicId.ToString(), u => u
                .Index("sources_index")
                .Script(s => s
                    .Source(scriptSource)
                    .Params(scriptParams)
                )
            );

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
            var script = "ctx._source.subTopics.removeIf(subTopic -> subTopic.SubTopicId == params.Id)";
            bool check = _elasticsearchRepository.UpdateScript(id.ToString(), u => u.Index("sources_index")
               .Script(s => s.Source(script).Params(p => p.Add("Id", id))));

            return NoContent(); // Using NoContent for successful delete as it's more appropriate than Ok in REST terms.
        }
    }
}
