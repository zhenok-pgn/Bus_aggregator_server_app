using System.Globalization;
using System.Text.Json;

namespace App.Infrastructure.Services
{
    public class OsrmService
    {
        private readonly HttpClient _httpClient;

        public OsrmService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("OsrmClient");
        }

        public async Task<double?> GetDistanceInMetersAsync(List<(double Lat, double Lon)> coordinates)
        {
            if (coordinates.Count < 2)
                throw new ArgumentException("Минимум две точки необходимо для маршрута.");

            // Формируем строку с координатами
            var coordString = string.Join(';', coordinates.Select(c =>
                $"{c.Lon.ToString(CultureInfo.InvariantCulture)},{c.Lat.ToString(CultureInfo.InvariantCulture)}"));

            var url = $"route/v1/driving/{coordString}?overview=false";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return null;

            using var stream = await response.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<OsrmRouteResult>(stream, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            return result?.Routes?.FirstOrDefault()?.Distance;
        }
    }

    public class OsrmRouteResult
    {
        public string Code { get; set; } = default!;
        public OsmrRoute[] Routes { get; set; } = default!;
    }

    public class OsmrRoute
    {
        public double Duration { get; set; }
        public double Distance { get; set; }
        public Geometry Geometry { get; set; } = default!;
    }

    public class Geometry
    {
        public string Type { get; set; } = default!;
        public List<List<double>> Coordinates { get; set; } = default!;
    }
}
