using AutoMapper;
using MagicVillage_API.Data;
using MagicVillage_API.Model;
using MagicVillage_API.Model.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVillage_API.Controllers
{
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        private readonly ILogger<VillaAPIController> _logger;
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public VillaAPIController(ILogger<VillaAPIController> logger, DataContext context, IMapper mapper)
        {
           _logger = logger;
           _context = context;
           _mapper = mapper;
        }

        [HttpGet(Name = "GetAllVilla")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
        {

            IEnumerable<Villa> villaList = await _context.Villas.ToListAsync();
            _logger.LogInformation("Getting all Vilas");
            return Ok(_mapper.Map<List<VillaDTO>>(villaList));

        }

        [HttpGet("id", Name = "GetVilla")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<VillaDTO>> GetVilla(int id)
        {
            var villa = await _context.Villas.FirstOrDefaultAsync(v => v.Id == id);

            if (villa == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<VillaDTO>(villa));
        }

        [HttpPost(Name = "CreateVilla")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<VillaDTO>> CreateVilla(VillaCreateDTO createDTO)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _context.Villas.FirstOrDefaultAsync(u => u.Name.ToLower() == createDTO.Name.ToLower()) != null) 
            {
                ModelState.AddModelError("Error", "Villa already Exists!");
                return BadRequest(ModelState);
            }

            if (createDTO == null)
            {
                return BadRequest(createDTO);
            }

            Villa model = _mapper.Map<Villa>(createDTO);

            await _context.Villas.AddAsync(model);
            await _context.SaveChangesAsync();

            return CreatedAtRoute("GetVilla", new { id = model.Id },  model);
        }

        [HttpDelete("id", Name = "DeleteVilla")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            
            if (id == 0)
            {
                return BadRequest();
            }

            var villa = await _context.Villas.FirstOrDefaultAsync(v => v.Id == id);

            if (villa == null)
            {
                return NotFound();
            }

            _context.Villas.Remove(villa);
            await _context.SaveChangesAsync();

            return NoContent();

        }

        [HttpPut("id", Name = "UpdateVilla")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateVilla(int id, VillaUpdateDTO updateDTO)
        {

            if (updateDTO == null || id != updateDTO.Id) 
            { 
                return BadRequest();
            }

            Villa model = _mapper.Map<Villa>(updateDTO);

            _context.Villas.Update(model);
            await _context.SaveChangesAsync();

            return NoContent();

        }

        [HttpPatch("id", Name = "UpdatePartialVilla")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }

            var villa = await _context.Villas.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);

            if (villa == null)
            {
                return BadRequest();
            }

            VillaUpdateDTO villaDTO = _mapper.Map<VillaUpdateDTO>(villa);

    
            patchDTO.ApplyTo(villaDTO, ModelState);

            Villa model = _mapper.Map<Villa>(villaDTO);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Villas.Update(model);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
