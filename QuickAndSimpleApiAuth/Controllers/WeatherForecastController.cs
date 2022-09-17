using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace QuickAndSimpleApiAuth.Controllers;

[ApiController]
[Route("[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries =
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    [Authorize(Roles = "Manager")]
    [HttpGet("GetWeatherManager")]
    public IEnumerable<WeatherForecast> GetWeatherManager() => GetRandomForecast();

    [Authorize(Roles = "Manager, Admin")]
    [HttpGet("GetWeatherManagerAndAdmin")]
    public IEnumerable<WeatherForecast> GetWeatherManagerAndAdmin() => GetRandomForecast();

    [Authorize]
    [HttpGet("GetWeatherAll")]
    public IEnumerable<WeatherForecast> GetWeatherAll() => GetRandomForecast();

    [Authorize(Roles = "Reader")]
    [HttpGet("GetWeatherReader")]
    public IEnumerable<WeatherForecast> GetWeatherReader() => GetRandomForecast();

    private static IEnumerable<WeatherForecast> GetRandomForecast()
    {
        var result = Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        }).ToArray();

        return result;
    }
}