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
    [ApiVersion("2.0")] 
    [ApiController]
    //[ApiExplorerSettings(GroupName = "ParkApiDoc")]
    [ProducesResponseType(400)]
    public class ParkControllerV2 : Controller
    {
        private readonly iParkRepository _repo;
        private readonly IMapper _mapper;

        public ParkControllerV2(iParkRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets all parks
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetOneSinglePark")]
        [ProducesResponseType(200, Type = typeof(List<ParkDto>))]
        public IActionResult GetOneSinglePark()
        {
            var repoPark = _repo.GetParks().FirstOrDefault();
            var park = _mapper.Map<Park>(repoPark);

            return Ok(park);
        }
    }
}
