using backend.Entities;
using backend.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class SerialController : ControllerBase
    {
        private readonly ISerialService _serialService;

        public SerialController(ISerialService serialService)
        {
            _serialService = serialService;
        }

        // GET: api/Serials
        [HttpGet]
        public async Task<IActionResult> GetAllSerials()
        {
            var serials = await _serialService.GetAllAsync();
            return Ok(serials);
        }

        // GET: api/Serials/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSerialById(int id)
        {
            var serial = await _serialService.GetByIdAsync(id);
            if (serial == null)
            {
                return NotFound();
            }
            return Ok(serial);
        }

        // POST: api/Serials
        [HttpPost]
        public async Task<IActionResult> CreateSerial([FromBody] Serial serial)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdSerial = await _serialService.CreateAsync(serial);
            return CreatedAtAction(nameof(GetSerialById), new { id = createdSerial.Id }, createdSerial);
        }

        // PUT: api/Serials/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSerial(int id, [FromBody] Serial serial)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updatedSerial = await _serialService.UpdateAsync(id, serial);
            if (updatedSerial == null)
            {
                return NotFound();
            }
            return Ok(updatedSerial);
        }

        // DELETE: api/Serials/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSerial(int id)
        {
            var success = await _serialService.DeleteAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
