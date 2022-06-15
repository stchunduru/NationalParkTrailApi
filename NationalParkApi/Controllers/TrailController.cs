using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NationalParkApi.Models;
using NationalParkApi.Models.Dtos;
using NationalTrailApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NationalParkApi.Controllers
{
    [Route("api/v{version:apiVersion}/trails")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "TrailApiDoc")]
    [ProducesResponseType(400)]
    public class TrailController : Controller
    {
        private readonly iTrailRepository _repo;
        private readonly IMapper _mapper;

        public TrailController(iTrailRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets all trails
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetTrails")]
        [ProducesResponseType(200, Type = typeof(List<TrailDto>))]
        public IActionResult GetTrails()
        {
            var repoTrails = _repo.GetTrails();
            var trailDtos = new List<TrailDto>();

            foreach (var trail in repoTrails)
            {
                trailDtos.Add(_mapper.Map<TrailDto>(trail));
            }

            return Ok(trailDtos);
        }

        /// <summary>
        /// Gets all trails from park
        /// </summary>
        /// <param name="parkId"></param>
        /// <returns></returns>
        [HttpGet("[action]/{parkId:int}", Name = "GetTrailsInPark")]
        [ProducesResponseType(200, Type = typeof(List<TrailDto>))]
        public IActionResult GetTrailsInPark(int parkId)
        {
            var repoTrails = _repo.GetTrailsInPark(parkId);
            var trailDtos = new List<TrailDto>();

            foreach (var trail in repoTrails)
            {
                trailDtos.Add(_mapper.Map<TrailDto>(trail));
            }

            return Ok(trailDtos);
        }

        /// <summary>
        /// Gets single trail with int id
        /// </summary>
        /// <param name="trailId"></param>
        /// <returns></returns>
        [HttpGet("{trailId:int}", Name = "GetTrail")]
        [ProducesResponseType(200, Type = typeof(TrailDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetTrail(int trailId)
        {
            var repoTrail = _repo.GetTrail(trailId);
            if (repoTrail == null)
            {
                return NotFound();
            }
            var trailDto = _mapper.Map<TrailDto>(repoTrail);
            return Ok(trailDto);
        }


        /// <summary>
        /// Creates a new trail
        /// </summary>
        /// <param name="trail"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(TrailDto))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult CreateTrail([FromBody] TrailCreateDto trail)
        {
            if (trail == null)
            {
                return BadRequest(ModelState);
            }

            if (_repo.TrailExists(trail.Name))
            {
                ModelState.AddModelError("", "Trail Exists!");
                return StatusCode(404, ModelState);
            }


            var repoTrail = _mapper.Map<Trail>(trail);
            if (!_repo.CreateTrail(repoTrail))
            {
                ModelState.AddModelError("", "Unable to create trail!");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetTrail", new { trailId = repoTrail.Id }, repoTrail);
        }

        /// <summary>
        /// Updates an existing trail
        /// </summary>
        /// <param name="trailId"></param>
        /// <param name="trail"></param>
        /// <returns></returns>
        [HttpPatch("{trailId:int}", Name = "UpdateTrail")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult UpdateTrail(int trailId, [FromBody] TrailUpdateDto trail)
        {
            if (trail == null)
            {
                return BadRequest(ModelState);
            }
            if (!_repo.TrailExists(trailId))
            {
                ModelState.AddModelError("", "Trail does not exist");
                return StatusCode(404, ModelState);
            }
            var repoTrail = _mapper.Map<Trail>(trail);
            repoTrail.Id = trailId;

            if (!_repo.UpdateTrail(repoTrail))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }
            return Ok();
        }

        /// <summary>
        /// Deletes a trail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}", Name = "DeleteTrail")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public IActionResult DeleteTrail(int id)
        {
            if (!_repo.TrailExists(id))
            {
                return NotFound();
            }
            var repoTrail = _repo.GetTrail(id);
            if (!_repo.DeleteTrail(repoTrail))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }

    }
}
