namespace Doppler.BigQueryMicroservice.Weather
{
    public class DataService
    {
        public string[] GetData() => new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
    }
}
