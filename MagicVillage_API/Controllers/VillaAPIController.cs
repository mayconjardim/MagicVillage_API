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

        public VillaAPIController(ILogger<VillaAPIController> logger, DataContext context)
        {
           _logger = logger;
           _context = context;
        }

        [HttpGet(Name = "GetAllVilla")]
        [ProducesResponseType(200)]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            _logger.LogInformation("Getting all Vilas");
            return Ok(_context.Villas.ToList());

        }

        [HttpGet("id", Name = "GetVilla")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            var villa = _context.Villas.FirstOrDefault(v => v.Id == id);

            if (villa == null)
            {
                return NotFound();
            }

            return Ok(villa);
        }

        [HttpPost(Name = "CreateVilla")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult<VillaDTO> CreateVilla(VillaDTO villaDTO)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_context.Villas.FirstOrDefault(u => u.Name.ToLower() == villaDTO.Name.ToLower()) != null) 
            {
                ModelState.AddModelError("Error", "Villa already Exists!");
                return BadRequest(ModelState);
            }

            if (villaDTO == null)
            {
                return BadRequest(villaDTO);
            }

            if (villaDTO.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            Villa model = new()
            {
                Amenity = villaDTO.Amenity,
                Details = villaDTO.Details,
                ImageUrl = villaDTO.ImageUrl,
                Id = villaDTO.Id,
                Name = villaDTO.Name,
                Occupancy = villaDTO.Occupancy,
                Rate = villaDTO.Rate,
                Sqft = villaDTO.Sqft
            };

            _context.Villas.Add(model);
            _context.SaveChanges();

            return CreatedAtRoute("GetVilla", new { id = villaDTO.Id },  villaDTO);
        }

        [HttpDelete("id", Name = "DeleteVilla")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult DeleteVilla(int id)
        {
            
            if (id == 0)
            {
                return BadRequest();
            }

            var villa = _context.Villas.FirstOrDefault(v => v.Id == id);

            if (villa == null)
            {
                return NotFound();
            }

            _context.Villas.Remove(villa);
            _context.SaveChanges();

            return NoContent();

        }

        [HttpPut("id", Name = "UpdateVilla")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult UpdateVilla(int id, VillaDTO villaDTO)
        {

            if (villaDTO == null || id != villaDTO.Id) 
            { 
                return BadRequest();
            }

            Villa model = new()
            {
                Amenity = villaDTO.Amenity,
                Details = villaDTO.Details,
                ImageUrl = villaDTO.ImageUrl,
                Id = villaDTO.Id,
                Name = villaDTO.Name,
                Occupancy = villaDTO.Occupancy,
                Rate = villaDTO.Rate,
                Sqft = villaDTO.Sqft
            };

            _context.Villas.Update(model);
            _context.SaveChanges();

            return NoContent();

        }

        [HttpPatch("id", Name = "UpdatePartialVilla")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }

            var villa = _context.Villas.AsNoTracking().FirstOrDefault(v => v.Id == id);

            if (villa == null)
            {
                return BadRequest();
            }

            VillaDTO villaDTO = new()
            {
                Amenity = villa.Amenity,
                Details = villa.Details,
                ImageUrl = villa.ImageUrl,
                Id = villa.Id,
                Name = villa.Name,
                Occupancy = villa.Occupancy,
                Rate = villa.Rate,
                Sqft = villa.Sqft
            };

            patchDTO.ApplyTo(villaDTO, ModelState);

            Villa model = new()
            {
                Amenity = villaDTO.Amenity,
                Details = villaDTO.Details,
                ImageUrl = villaDTO.ImageUrl,
                Id = villaDTO.Id,
                Name = villaDTO.Name,
                Occupancy = villaDTO.Occupancy,
                Rate = villaDTO.Rate,
                Sqft = villaDTO.Sqft
            };

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Villas.Update(model);
            _context.SaveChanges();

            return NoContent();
        }

    }
}
