using NationalParkWeb.Interfaces;
using NationalParkWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace NationalParkWeb.Repositories
{
    public class ParkRepository : Repository<Park>, iParkRepository
    {
        private readonly IHttpClientFactory _clientFactory;

        public ParkRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }

    }
}
