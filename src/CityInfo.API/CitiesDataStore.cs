using CityInfo.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API
{


    public class CitiesDataStore
    {
        public static CitiesDataStore Current { get; } = new CitiesDataStore();
        public List<CityDto> Cities { get; set; }

        public CitiesDataStore()
        {
            Cities = new List<CityDto>
            {
                new CityDto()
                {
                    Id = 1,
                    Name = "Ankara",
                    Description = "bla",

                    PointsOfInterest=new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id=1,
                            Name="Yenimahalle",
                            Description="bla"
                        },

                        new PointOfInterestDto()
                        {
                            Id=2,
                            Name="Batıkent",
                            Description="bla"
                        }

                     }
                },

                new CityDto()
                {
                    Id = 2,
                    Name = "Malatya",
                    Description = "bla bla",

                    PointsOfInterest=new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id=1,
                            Name="Battalgazi",
                            Description="bla bla"
                        },

                        new PointOfInterestDto()
                        {
                            Id=2,
                            Name="Darende",
                            Description="bla bla"
                        }

                     }
                },

                new CityDto()
                {
                    Id = 3,
                    Name = "Mersin",
                    Description = "bla bla bla",

                    PointsOfInterest=new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id=1,
                            Name="Anamur",
                            Description="bla bla bla"
                        },

                        new PointOfInterestDto()
                        {
                            Id=2,
                            Name="Akdeniz",
                            Description="bla bla bla"
                        }

                     }
                }

           };
        }
            
      



   }                 
}
