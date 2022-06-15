using NationalParkApi.Data;
using NationalParkApi.Interfaces;
using NationalParkApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NationalParkApi.Repositories
{
    public class ParkRepository : iParkRepository
    {
        private readonly ParkDbContext _db;

        public ParkRepository(ParkDbContext parkDbContext)
        {
            _db = parkDbContext;
        }

        public bool CreatePark(Park park)
        {
            _db.Parks.Add(park);
            return Save();
        }

        public bool DeletePark(Park park)
        {
            _db.Parks.Remove(park);
            return Save();
        }

        public Park GetPark(int parkId)
        {
            return _db.Parks.FirstOrDefault(c => c.Id == parkId);
        }

        public ICollection<Park> GetParks()
        {
            return _db.Parks.OrderBy(c => c.Name).ToList();
        }

        public bool ParkExists(string name)
        {
            bool val = _db.Parks.Any(c => c.Name.ToLower().Trim() == name.ToLower().Trim());
            return val;
        }

        public bool ParkExists(int id)
        {
            bool val = _db.Parks.Any(c => c.Id == id);
            return val;
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdatePark(Park park)
        {
            _db.Parks.Update(park);
            return Save();
        }
    }
}
