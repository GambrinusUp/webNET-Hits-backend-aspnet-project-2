namespace FoodDelivery.Models.DTO
{
    public class WeatherForecastDTO
    {
        public DateTime Date { get; set; }
        public int TemperatureC { get; set; }
        public int TemeratureF => 32 + (int)(TemperatureC / 0.5556);
        public string? Summary { get; set; }
    }
}
