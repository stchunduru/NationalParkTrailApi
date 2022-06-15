using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NationalParkApi.Interfaces;
using NationalParkApi.Mapping;
using NationalParkApi.Models;
using NationalParkApi.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NationalParkApi.Controllers
{
    [Route("api/v{version:apiVersion}/parks")]
    //[Route("api/Parks")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "ParkApiDoc")]
    [ProducesResponseType(400)]
    public class ParkController : Controller
    {
        private readonly iParkRepository _repo;
        private readonly IMapper _mapper;

        public ParkController(iParkRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets all parks
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetParks")]
        [ProducesResponseType(200, Type = typeof(List<ParkDto>))]
        public IActionResult GetParks()
        {
            var repoParks = _repo.GetParks();
            var parkDtos = new List<ParkDto>();

            foreach (Park park in repoParks)
            {
                parkDtos.Add(_mapper.Map<ParkDto>(park));
            }

            return Ok(parkDtos);
        }

        /// <summary>
        /// Gets single park with int id
        /// </summary>
        /// <param name="parkId"></param>
        /// <returns></returns>
        [HttpGet("{parkId:int}", Name = "GetPark")]
        [ProducesResponseType(200, Type = typeof(ParkDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetPark(int parkId)
        {
            var repoPark = _repo.GetPark(parkId);
            if(repoPark == null)
            {
                return NotFound();
            }
            var parkDto = _mapper.Map<ParkDto>(repoPark);
            return Ok(parkDto);
        }


        /// <summary>
        /// Creates a new park
        /// </summary>
        /// <param name="park"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(ParkDto))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult CreatePark([FromBody] ParkDto park)
        {
            if(park == null)
            {
                return BadRequest(ModelState);
            }

            if(_repo.ParkExists(park.Name))
            {
                ModelState.AddModelError("", "Park Exists!");
                return StatusCode(404, ModelState);
            }


            var repoPark = _mapper.Map<Park>(park);
            if (!_repo.CreatePark(repoPark))
            {
                ModelState.AddModelError("", "Unable to create park!");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetPark", new { version = HttpContext.GetRequestedApiVersion().ToString(),  parkId = repoPark.Id }, repoPark);
        }

        /// <summary>
        /// Updates an existing park
        /// </summary>
        /// <param name="parkId"></param>
        /// <param name="park"></param>
        /// <returns></returns>
        [HttpPatch("{parkId:int}", Name = "UpdatePark")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult UpdatePark(int parkId, [FromBody] ParkDto park)
        {
            if(park == null)
            {
                return BadRequest(ModelState);
            }
            if (!_repo.ParkExists(parkId))
            {
                ModelState.AddModelError("", "Park does not exist");
                return StatusCode(404, ModelState);
            }
            var repoPark = _mapper.Map<Park>(park);
            repoPark.Id = parkId;

            if (!_repo.UpdatePark(repoPark))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }
            return Ok();
        }

        /// <summary>
        /// Deletes a park
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}", Name = "DeletePark")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public IActionResult DeletePark(int id)
        {
            if (!_repo.ParkExists(id))
            {
                return NotFound();
            }
            var repoPark = _repo.GetPark(id);
            if (!_repo.DeletePark(repoPark))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }
    }
}
