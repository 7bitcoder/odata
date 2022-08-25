using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Attributes;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace odata.Controllers;

public class WeatherForecastController : ODataController
{
    private const string baseUrl = "[controller]";
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [EnableQuery]
    [OdataDefaultParams]
    public WeatherForecast? Get([FromRoute] int key) => Data
        .Where(w => w.Id == key)
        .FirstOrDefault();

    [EnableQuery]
    [OdataDefaultParams]
    public IEnumerable<WeatherForecast> Get() => Data;

    [EnableQuery]
    public IActionResult Post([FromBody] WeatherForecast forecast)
    {
        Data.Add(forecast);
        return Created(forecast);
    }

    public IActionResult Put(int key, [FromBody] WeatherForecast forecast)
    {
        Data[key] = forecast;
        return Updated(forecast);
    }

    public IActionResult Patch(int key, [FromBody] Delta<WeatherForecast> forecast)
    {
        forecast.Patch(Data[key]);
        return Updated(forecast);
    }

    [HttpPost($"{baseUrl}/process-data")]
    public IActionResult ProcessData([FromBody] WeatherForecast forecast)
    {
        return Ok(forecast);
    }

    private List<WeatherForecast> Data = Enumerable.Range(1, 5)
        .Select(index => new WeatherForecast
        {
            Id = index,
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        }).ToList();
}
