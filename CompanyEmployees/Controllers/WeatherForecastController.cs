using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Repository;

namespace CompanyEmployees.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private IRepositoryManager _repository;

        public WeatherForecastController(IRepositoryManager repository)
        {
            _repository = repository;

        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {

            return new string[] {"value1", "value2"};
        }
    }



    /*[HttpGet]
    public IEnumerable<WeatherForecast> Get()
    {
        var rng = new Random();
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
    }*/
}
