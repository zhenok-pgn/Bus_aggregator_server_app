namespace App.Application.DTO
{
    public class TripEtaDTO
    {
        public required string TripId { get; set; }                          // Идентификатор рейса
        public DateTimeOffset CurrentTime { get; set; }           // Текущее время (с сервера)
        public required List<StopEtaDTO> StopEtas { get; set; }
    }

    public class StopEtaDTO
    {
        public required string StopId { get; set; }                          // Идентификатор остановки
        public required string StopName { get; set; }                        // Название остановки
        public double Latitude { get; set; }                        // Широта остановки
        public double Longitude { get; set; }
        public double TimezoneOffset { get; set; }            // Смещение часового пояса в секундах
        public DateTimeOffset EstimatedArrival { get; set; }      // Ожидаемое прибытие
        public TimeSpan? Delay { get; set; }                      // Задержка, если есть
    }
}
