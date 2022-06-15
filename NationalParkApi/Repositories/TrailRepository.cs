using Microsoft.EntityFrameworkCore;
using NationalParkApi.Data;
using NationalParkApi.Interfaces;
using NationalParkApi.Models;
using NationalTrailApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NationalParkApi.Repositories
{
    public class TrailRepository : iTrailRepository
    {
        private readonly ParkDbContext _db;

        public TrailRepository(ParkDbContext db)
        {
            _db = db;
        }
        public bool CreateTrail(Trail trail)
        {
            _db.Trails.Add(trail);
            return Save();
        }

        public bool DeleteTrail(Trail trail)
        {
            _db.Trails.Remove(trail);
            return Save();
        }

        public Trail GetTrail(int trailId)
        {
            return _db.Trails.Include(c => c.Park).FirstOrDefault(c => c.Id == trailId);
        }

        public ICollection<Trail> GetTrails()
        {
            return _db.Trails.OrderBy(c => c.Name).ToList();
        }

        public ICollection<Trail> GetTrailsInPark(int id)
        {

            return _db.Trails.Include(c => c.Park).Where(c => c.ParkId == id).ToList();
        }

        public bool TrailExists(string name)
        {
            bool val = _db.Trails.Any(c => c.Name.ToLower().Trim() == name.ToLower().Trim());
            return val;
        }

        public bool TrailExists(int id)
        {
            bool val = _db.Trails.Any(c => c.Id == id);
            return val;
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateTrail(Trail park)
        {
            _db.Trails.Update(park);
            return Save();
        }
    }
}
