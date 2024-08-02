using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace WebApi.UploadFile.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost(Name = "PostWeatherForecast")]
        public async Task<IActionResult> Post([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is empty");

            try
            {
                using (var stream = file.OpenReadStream())
                using (var reader = new StreamReader(stream))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = await reader.ReadLineAsync();

                        if (line != null)
                            ProcessLine(line);
                    }
                }

                return Ok("File processed successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private void ProcessLine(string line) => Console.WriteLine(line);
    }
}
