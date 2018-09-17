using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.API.Entities;
using CityInfo.API.Models;

namespace CityInfo.API
{
    public static class CityInfoExtensions
    {
        public static void EnsureSeedDataForContext(this CityInfoContext context)
        {
            if (context.Cities.Any())
            {
                return;
            }

            var cities = new List<City>()
            {
                new City()
                {
                   
                    Name = "Ankara",
                    Description = "bla",

                    PointsOfInterest=new List<PointOfInterest>()
                    {
                        new PointOfInterest()
                        {
                          
                            Name="Yenimahalle",
                            Description="bla"
                        },

                        new PointOfInterest()
                        {
                           
                            Name="Batıkent",
                            Description="bla"
                        }

                     }
                },

                new City()
                {
                   
                    Name = "Malatya",
                    Description = "bla bla",

                    PointsOfInterest=new List<PointOfInterest>()
                    {
                        new PointOfInterest()
                        {
                           
                            Name="Battalgazi",
                            Description="bla bla"
                        },

                        new PointOfInterest()
                        {
                           
                            Name="Darende",
                            Description="bla bla"
                        }

                     }
                },

                new City()
                {
                   
                    Name = "Mersin",
                    Description = "bla bla bla",

                    PointsOfInterest=new List<PointOfInterest>()
                    {
                        new PointOfInterest()
                        {

                            Name="Anamur",
                            Description="bla bla bla"
                        },

                        new PointOfInterest()
                        {

                            Name="Akdeniz",
                            Description="bla bla bla"
                        }

                     }
                }
           };

            context.Cities.AddRange(cities);
            context.SaveChanges();


        }


   }
}
