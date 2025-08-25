namespace IF.ThreadPilot.Core.Infrastructure.Configuration.Options
{
    public class ApiOptions
    {
        public VehicleOptions Vehicle { get; set; }
    }

    public class VehicleOptions
    {
        public string BaseUrl { get; set; }
    }
}
