namespace MatveevVadimKt_42_22
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }
    }
    public class NewSummaryDto
    {
        public string NewSummary { get; set; }
    }

}
