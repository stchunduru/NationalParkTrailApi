using NationalParkApi.Models;
using NationalParkApi.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NationalParkApi.Interfaces
{
    public interface iParkRepository
    {
        ICollection<Park> GetParks();
        Park GetPark(int parkId);
        bool ParkExists(string name);
        bool ParkExists(int id);
        bool CreatePark(Park park);
        bool UpdatePark(Park park);
        bool DeletePark(Park park);
        bool Save();
    }
}
