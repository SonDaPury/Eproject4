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
    public class OptionController : ControllerBase
    {
        private readonly IOptionService _optionService;
        private readonly IMapper _mapper;

        public OptionController(IOptionService optionService, IMapper mapper)
        {
            _optionService = optionService;
            _mapper = mapper;
        }

        // POST: api/Options
        [HttpPost]
        public async Task<ActionResult<OptionDto>> CreateOption([FromBody] OptionDto optionDto)
        {
            if (optionDto == null)
            {
                return BadRequest("Option data is required");
            }

            var option = _mapper.Map<Option>(optionDto);
            var createdOption = await _optionService.CreateAsync(option);
            var createdOptionDto = _mapper.Map<OptionDto>(createdOption);
            return CreatedAtAction(nameof(GetOption), new { id = createdOptionDto.Id }, createdOptionDto);
        }

        // GET: api/Options
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OptionDto>>> GetAllOptions()
        {
            var options = await _optionService.GetAllAsync();
            var optionDtos = _mapper.Map<List<OptionDto>>(options);
            return Ok(optionDtos);
        }

        // GET: api/Options/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OptionDto>> GetOption(int id)
        {
            var option = await _optionService.GetByIdAsync(id);
            if (option == null)
            {
                return NotFound($"Option with ID {id} not found.");
            }
            var optionDto = _mapper.Map<OptionDto>(option);
            return Ok(optionDto);
        }

        // PUT: api/Options/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOption(int id, [FromBody] OptionDto optionDto)
        {
            if (optionDto == null)
            {
                return BadRequest("Invalid option data");
            }

            var option = _mapper.Map<Option>(optionDto);
            var updatedOption = await _optionService.UpdateAsync(id, option);
            if (updatedOption == null)
            {
                return NotFound($"Option with ID {id} not found.");
            }
            return Ok(_mapper.Map<OptionDto>(updatedOption));
        }

        // DELETE: api/Options/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOption(int id)
        {
            var success = await _optionService.DeleteAsync(id);
            if (!success)
            {
                return NotFound($"Option with ID {id} not found.");
            }
            return NoContent(); // Using NoContent for successful delete as it's more appropriate than Ok in REST terms.
        }
    }
}
