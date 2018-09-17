using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;
using Remotion.Linq.Clauses;

namespace CityInfo.API.Services
{
    public class CityInfoRepository:ICityInfoRepository
    {
        private CityInfoContext _context;

        public CityInfoRepository(CityInfoContext context)
        {
            _context = context;
        }

        public bool CityExists(int cityId)
        {
            return _context.Cities.Any(c => c.Id == cityId);
        }


        public void AddPointOfInterestForCity(int cityId, PointOfInterest pointOfInterest)
        {
            var city = GetCity(cityId, false);
            city.PointsOfInterest.Add(pointOfInterest);
        }



        public IEnumerable<City> GetCities()
        {
            return _context.Cities.OrderBy(c => c.Name).ToList();
        }


        public City GetCity(int cityId,bool includePointsOfInterest)
        {
            if (includePointsOfInterest)
            {
                return _context.Cities.Include(c => c.PointsOfInterest)
                    .FirstOrDefault(c => c.Id == cityId);

                //return _context.Cities.Include(c => c.PointsOfInterest)
                //.Where(c => c.Id == cityId).FirstOrDefault();

            }
            return _context.Cities.FirstOrDefault(c => c.Id == cityId);
            //return _context.Cities.Where(c => c.Id == cityId).FirstOrDefault();

        }



        public PointOfInterest GetPointOfInterestForCity(int cityId, int pointOfInterestId)
        {
            return _context.PointsOfInterest.FirstOrDefault(p=>p.CityId == cityId && p.Id == pointOfInterestId);
            //return _context.PointsOfInterest.Where(p=>p.CityId == cityId && p.Id == pointOfInterestId).FirstOrDefault();
            
        }

       


        public IEnumerable<PointOfInterest> GetPointsOfInterestsForCity(int cityId)
        {
            return _context.PointsOfInterest.Where(p => p.CityId == cityId).ToList();
        }

        public void DeletePointOfInterest(PointOfInterest pointOfInterest)
        {
            _context.PointsOfInterest.Remove(pointOfInterest);
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        
    }
}
