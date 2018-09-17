using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Models;
using CityInfo.API.Services;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class CitiesController : Controller
    {
        private ICityInfoRepository _cityInfoRepository;

        public CitiesController(ICityInfoRepository cityInfoRepository)
        {
            _cityInfoRepository = cityInfoRepository;
        }


        [HttpGet()]

        public IActionResult GetCities()
        {
            
            var cityEntities = _cityInfoRepository.GetCities();
            var results = Mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities);
            return Ok(results);


            //return Ok(CitiesDataStore.Current.Cities);
            //////var results = new List<CityWithoutPointsOfInterestDto>();

            //////foreach (var cityEntity in cityEntities)
            //////{
            //////    results.Add(new CityWithoutPointsOfInterestDto
            //////    {
            //////        Id = cityEntity.Id,
            //////        Name = cityEntity.Name,
            //////        Description = cityEntity.Description

            //////    });
            //////}

        }




        [HttpGet("{id}")]

        public IActionResult GetCity(int id, bool includePointsOfInterest =true)
        {
            var city = _cityInfoRepository.GetCity(id, includePointsOfInterest);

            if (city == null)
            {
                return NotFound();
            }

            if (includePointsOfInterest)
            {
                var cityResult = Mapper.Map<CityDto>(city);

                return Ok(cityResult);

                //var cityResult = new CityDto()
                //{
                //    Id = city.Id,
                //    Name = city.Name,
                //    Description = city.Description

                //};

                //foreach (var poi in city.PointsOfInterest)
                //{
                //    cityResult.PointsOfInterest.Add(
                //        new PointOfInterestDto()
                //        {
                //            Id = poi.Id,
                //            Name = poi.Name,
                //            Description = poi.Description

                //        });
                //}

            }

            var cityWithoutPointOfInterestResult = Mapper.Map<CityWithoutPointsOfInterestDto>(city);

            return Ok(cityWithoutPointOfInterestResult);

            ////{
            ////    Id=city.Id,
            ////    Name = city.Name,
            ////    Description = city.Description

            ////};




            //find city
            //var cityToReturn = CitiesDataStore.Current.Cities.FirstOrDefault(c=>c.Id==id);
            //if (cityToReturn == null)
            //{
            //    return NotFound();
            //}
            //    return Ok(cityToReturn);

        }







    }

}