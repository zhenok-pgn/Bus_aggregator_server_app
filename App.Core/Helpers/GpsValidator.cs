namespace App.Core.Helpers
{
    public static class GpsValidator
    {
        private const double EarthRadiusMeters = 6371000; // Средний радиус Земли (м)
        private const double MaxSpeedMps = 55; // Допустимая скорость в м/с
        private const double MaxJumpDistanceMeters = 5000; // Максимально допустимый скачок в метрах

        public static bool IsValidCoordinates(double lat, double lon)
        {
            return lat >= -90 && lat <= 90 && lon >= -180 && lon <= 180;
        }

        public static bool IsRealisticJump(double lat1, double lon1, double lat2, double lon2)
        {
            var distance = Haversine(lat1, lon1, lat2, lon2);
            return distance <= MaxJumpDistanceMeters;
        }

        public static bool IsValidSpeed(double lat1, double lon1, long time1, double lat2, double lon2, long time2)
        {
            var distance = Haversine(lat1, lon1, lat2, lon2);
            var timeDeltaSec = Math.Abs((time2 - time1) / 1000.0);

            if (timeDeltaSec < 1) return false; // защита от деления на ноль и слишком частых точек

            var speed = distance / timeDeltaSec; // м/с

            return speed <= MaxSpeedMps;
        }

        public static double Haversine(double lat1, double lon1, double lat2, double lon2)
        {
            double dLat = ToRadians(lat2 - lat1);
            double dLon = ToRadians(lon2 - lon1);
            double a = Math.Pow(Math.Sin(dLat / 2), 2) +
                       Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                       Math.Pow(Math.Sin(dLon / 2), 2);
            double c = 2 * Math.Asin(Math.Sqrt(a));
            return EarthRadiusMeters * c;
        }

        private static double ToRadians(double angle) => angle * Math.PI / 180.0;
    }
}
