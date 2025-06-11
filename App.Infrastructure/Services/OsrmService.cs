using Microsoft.Extensions.Caching.Memory;
using System.Globalization;
using System.Text.Json;

namespace App.Infrastructure.Services
{
    public class OsrmService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private readonly MemoryCacheEntryOptions _cacheOptions;
        private const int CacheExpirationMinutes = 60;

        public OsrmService(IHttpClientFactory httpClientFactory, IMemoryCache memoryCache)
        {
            _httpClient = httpClientFactory.CreateClient("OsrmClient");
            _cache = memoryCache;
            _cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(CacheExpirationMinutes)
            };
        }

        public async Task<double?> GetDistanceInMetersAsync(List<(double Lat, double Lon)> coordinates)
        {
            if (coordinates.Count < 2)
                throw new ArgumentException("Минимум две точки необходимо для маршрута.");

            // Создаем ключ кэша на основе координат
            var cacheKey = CreateCacheKey(coordinates);

            // Пытаемся получить результат из кэша
            if (_cache.TryGetValue(cacheKey, out double? cachedDistance))
            {
                return cachedDistance;
            }

            // Формируем строку с координатами
            var coordString = string.Join(';', coordinates.Select(c =>
                $"{c.Lon.ToString(CultureInfo.InvariantCulture)},{c.Lat.ToString(CultureInfo.InvariantCulture)}"));

            var url = $"route/v1/driving/{coordString}?overview=false";

            try
            {
                var response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                    return null;

                using var stream = await response.Content.ReadAsStreamAsync();
                var result = await JsonSerializer.DeserializeAsync<OsrmRouteResult>(stream, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                var distance = result?.Routes?.FirstOrDefault()?.Distance;

                // Сохраняем результат в кэш
                if (distance.HasValue)
                {
                    _cache.Set(cacheKey, distance, _cacheOptions);
                }

                return distance;
            }
            catch
            {
                // В случае ошибки возвращаем null, но не кэшируем
                return null;
            }
        }

        private string CreateCacheKey(List<(double Lat, double Lon)> coordinates)
        {
            // Создаем уникальный ключ на основе координат с округлением до 6 знаков
            var keyParts = coordinates.Select(c =>
                $"{Math.Round(c.Lat, 6).ToString(CultureInfo.InvariantCulture)}_" +
                $"{Math.Round(c.Lon, 6).ToString(CultureInfo.InvariantCulture)}");

            return "OsrmRoute_" + string.Join("|", keyParts);
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
