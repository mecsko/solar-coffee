namespace SolarCoffee.Services
{
    public class ServiceResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public DateTime Time { get; set; }
        public T? Data { get; set; }
    }
}
