using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace CityInfo.API.Controllers
{


    [Route("api/cities")]

    public class PointsOfInterestController:Controller
    {
        private ILogger<PointsOfInterestController> _logger;
        private IMailService _mailService;
        private ICityInfoRepository _cityInfoRepository;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger,IMailService mailService, ICityInfoRepository cityInfoRepository)
        {
            _logger = logger;
            _mailService = mailService;
            _cityInfoRepository = cityInfoRepository;
        }

        [HttpGet("{cityId}/pointsofinterest")]

        public IActionResult GetPointsOfInterest(int cityId)
        {
            try
            {

                if (!_cityInfoRepository.CityExists(cityId))
                {
                    _logger.LogInformation($"City with id {cityId} wasn't found when accessing points of interest");
                    return NotFound();
                }

                var pointsOfInterestForCity = _cityInfoRepository.GetPointsOfInterestsForCity(cityId);
                var pointsOfInterestForCityResults =
                    Mapper.Map<IEnumerable<PointOfInterestDto>>(pointsOfInterestForCity);

                return Ok(pointsOfInterestForCityResults);


                //////var pointsOfInterestForCityResults=new List<PointOfInterestDto>();

                //////foreach (var poi in pointsOfInterestForCity)
                //////{
                //////    pointsOfInterestForCityResults.Add(new PointOfInterestDto()
                //////    {
                //////        Id=poi.Id,
                //////        Name=poi.Name,
                //////        Description = poi.Description
                //////    });   

                //////}




                // var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
                //if (city == null)
                //{
                //    _logger.LogInformation($"City with id {cityId} wasn't found when accessing points of interest");
                //    return NotFound();
                //}
                //return Ok(city.PointsOfInterest);


            }



            catch (Exception ex)
            {
                _logger.LogCritical($"City with id  wasn't found when accessing points of interest {cityId}", ex);

                return StatusCode(500, "A problem happened while handling your request");
            }

            



        }




        [HttpGet("{cityId}/pointsofinterest/{id}", Name="GetPointOfInterest")]

        public IActionResult GetPointOfInteret(int cityId,int id)
        {
            if (!_cityInfoRepository.CityExists(cityId))
            {
                return NotFound();
            }

            var pointOfInterest = _cityInfoRepository.GetPointOfInterestForCity(cityId, id);

            if (pointOfInterest == null)
            {
                return NotFound();
            }

            var pointOfInterestResult = Mapper.Map<PointOfInterestDto>(pointOfInterest);

            return Ok(pointOfInterestResult);

            ////var pointOfInterestResult = new PointOfInterestDto()
            ////{
            ////    Id = pointOfInterest.Id,
            ////    Name = pointOfInterest.Name,
            ////    Description = pointOfInterest.Description

            ////};



            //var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            //if (city == null)
            //{
            //    return NotFound();
            //}

            //var pointOfInterest = city.PointsOfInterest.FirstOrDefault(p=> p.Id == id);

            //if (pointOfInterest == null)
            //{
            //    return NotFound();
            //}

            //return Ok(pointOfInterest);


        }




        [HttpPost("{cityId}/pointsofinterest")]

        public IActionResult CreatePointOfInterest(int cityId,
            [FromBody] PointOfInterestForCreationDto pointOfInterest)
        {
            if (pointOfInterest == null)
            {
                return BadRequest();
            }


            if (pointOfInterest.Description == pointOfInterest.Name)
            {
                ModelState.AddModelError("Description", "İçerik ile isim aynı olmasın");
            }


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_cityInfoRepository.CityExists(cityId))
            {
                return NotFound();
            }

            
            var finalPointOfInterest = Mapper.Map<Entities.PointOfInterest>(pointOfInterest);

            _cityInfoRepository.AddPointOfInterestForCity(cityId,finalPointOfInterest);

            if (!_cityInfoRepository.Save())
            {
                return StatusCode(500, "Hata oluştu");
            }

            var createdPointOfInterestToReturn = Mapper.Map<Models.PointOfInterestDto>(finalPointOfInterest);


            return CreatedAtRoute("GetPointOfInterest", new
            { cityId = cityId, id = createdPointOfInterestToReturn.Id}, createdPointOfInterestToReturn);



            //var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            //if (city == null)
            //{
            //    return NotFound();
            //}

            //var maxPointOfInteresetId = CitiesDataStore.Current.Cities.SelectMany(
            //    c => c.PointsOfInterest).Max(p => p.Id);

            //var finalPointOfInterest = new PointOfInterestDto()
            //{
            //    Id = ++maxPointOfInteresetId,
            //    Name = pointOfInterest.Name,
            //    Description = pointOfInterest.Description
            //};
            //city.PointsOfInterest.Add(finalPointOfInterest);




        }




        [HttpPut("{cityId}/pointsofinterest/{id}")]

        public IActionResult UpdatePointOfInterest(int cityId,int id,
            [FromBody] PointOfInterestForUpdateDto pointOfInterest)
        {
            if (pointOfInterest == null)
            {
                return BadRequest();
            }

            if (pointOfInterest.Description == pointOfInterest.Name)
            {
                ModelState.AddModelError("Description", "İçerik ile isim aynı olmasın.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }



            var pointOfInterestEntity = _cityInfoRepository.GetPointOfInterestForCity(cityId, id);
            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }

            Mapper.Map(pointOfInterest, pointOfInterestEntity);
            if (!_cityInfoRepository.Save())
            {
                return StatusCode(500, "Bir problem oluştu");
            }
            return NoContent();




            //var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            //if (city == null)
            //{
            //    return NotFound();
            //}


            //var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);

            //if (pointOfInterestFromStore == null)
            //{
            //    return NotFound();
            //}
            //pointOfInterestFromStore.Name = pointOfInterest.Name;
            //pointOfInterestFromStore.Description = pointOfInterest.Description;




        }




        [HttpPatch("{cityId}/pointsofinterest/{id}")]

        public IActionResult PartiallyUpdatePointOfInterest(int cityId,int id,
            [FromBody] JsonPatchDocument<PointOfInterestForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }  

            if (!_cityInfoRepository.CityExists(cityId))
            {
                return NotFound();
            }

            var pointOfInterestEntity = _cityInfoRepository.GetPointOfInterestForCity(cityId, id);
            if (pointOfInterestEntity == null)
            {
                return NotFound();
            } 

            var pointOfInterestToPatch = Mapper.Map<PointOfInterestForUpdateDto>(pointOfInterestEntity);

            patchDoc.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            if (pointOfInterestToPatch.Description == pointOfInterestToPatch.Name)
            {
                ModelState.AddModelError("Description", "İçrik ile başlık aynı olamaz.");
            }

            TryValidateModel(pointOfInterestToPatch);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

           
            Mapper.Map(pointOfInterestToPatch, pointOfInterestEntity);
            if (!_cityInfoRepository.Save())
            {
                return StatusCode(500, "Ne oluyor");
            }

            return NoContent();


            //var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            //if (city == null)
            //{
            //    return NotFound();
            //}
            //var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(c => c.Id == id);

            //if (pointOfInterestFromStore == null)
            //{
            //    return NotFound();
            //}


            //var pointOfInterestToPatch = new PointOfInterestForUpdateDto()

            //    {
            //        Name = pointOfInterestFromStore.Name,
            //        Description = pointOfInterestFromStore.Description

            //    };
            //pointOfInterestFromStore.Name = pointOfInterestToPatch.Name;
            //pointOfInterestFromStore.Description = pointOfInterestToPatch.Description;


        }


        [HttpDelete("{cityId}/pointsofinterest/{id}")]

        public IActionResult DeletePointOfInterest(int cityId,int id)
        {
            if (!_cityInfoRepository.CityExists(cityId))
            {
                return NotFound();
            }

            var pointOfInterestEntity = _cityInfoRepository.GetPointOfInterestForCity(cityId, id);

            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }

            _cityInfoRepository.DeletePointOfInterest(pointOfInterestEntity);

            if (!_cityInfoRepository.Save())
            {
                return StatusCode(500, "Ters giden bişeyler oldu.");
            }

            _mailService.Send("Point of interest deleted",
                $"Point of interest {pointOfInterestEntity.Name} with id {pointOfInterestEntity.Id} was deleted");

            return NoContent();





            //var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            //if (city == null)
            //{
            //    return NotFound();
            //}


            //var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(c => c.Id == id);

            //if (pointOfInterestFromStore == null)
            //{
            //    return NotFound();
            //}

            //city.PointsOfInterest.Remove(pointOfInterestFromStore);
            //_mailService.Send("Point of interest deleted",
            //    $"Point of interest {pointOfInterestFromStore.Name} with id {pointOfInterestFromStore.Id} was deleted");




        }






    }
}
