using MagicVillage_API.Data;
using MagicVillage_API.Model.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVillage_API.Controllers
{
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        private readonly ILogger<VillaAPIController> _logger;

        public VillaAPIController(ILogger<VillaAPIController> logger)
        {
           _logger = logger;
        }

        [HttpGet(Name = "GetAllVilla")]
        [ProducesResponseType(200)]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            _logger.LogInformation("Getting all Vilas");
            return Ok(VillaStore.villaList);

        }

        [HttpGet("id", Name = "GetVilla")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);

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

            if (VillaStore.villaList.FirstOrDefault(u => u.Name.ToLower() == villaDTO.Name.ToLower()) != null) 
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

            villaDTO.Id = VillaStore.villaList.Count() + 1;
            VillaStore.villaList.Add(villaDTO);
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

            var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);

            if (villa == null)
            {
                return NotFound();
            }

            VillaStore.villaList.Remove(villa);

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

            var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            villa.Name = villaDTO.Name;
            villa.Sqft = villaDTO.Sqft;
            villa.Occupancy = villaDTO.Occupancy;

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

            var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            
            if (villa == null)
            {
                return BadRequest();
            }

            patchDTO.ApplyTo(villa, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return NoContent();
        }

    }
}
