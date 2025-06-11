namespace App.Core.Helpers
{
    public static class EtaEstimator
    {
        private const double MinValidSpeed = 0.5; // 0.5 м/с (1.8 км/ч) - порог "остановки"
        private const double DefaultLowSpeed = 2.0; // 2 м/с (7.2 км/ч) - скорость по умолчанию для пробок

        public static double? EstimateEtaSeconds(double? tCurrentSeconds, double tHistSeconds)
        {
            if(tCurrentSeconds == null && tHistSeconds == -1)
                return null; // no data available
            if(tCurrentSeconds == null)
                return tHistSeconds; // use historical data only
            if (tHistSeconds == -1)
                return tCurrentSeconds.Value; // use current data only
            var alpha = ComputeAlphaSmoothed(tCurrentSeconds.Value, tHistSeconds);
            return alpha * tCurrentSeconds.Value + (1 - alpha) * tHistSeconds;
        }

        public static double? EstimateEtaSeconds(double? tCurrentSeconds, double tHistSeconds, double alpha)
        {
            if (tCurrentSeconds == null && tHistSeconds == -1)
                return null; // no data available
            if (tCurrentSeconds == null)
                return tHistSeconds; // use historical data only
            if (tHistSeconds == -1)
                return tCurrentSeconds.Value; // use current data only
            alpha = Math.Clamp(alpha, 0.0, 0.7);
            return alpha * tCurrentSeconds.Value + (1 - alpha) * tHistSeconds;
        }

        public static double? EstimateEtaSeconds(
            double totalDistance, 
            double remainingDistance, 
            double curSpeed, 
            double tHistSeconds,
            double secondsFromDeparture)
        {
            // 1. Обработка некорректных входных данных
            if (totalDistance <= 0 || remainingDistance < 0 || (tHistSeconds <= 0 && curSpeed < MinValidSpeed))
                return null;

            // 2. Коррекция нулевой скорости
            double effectiveSpeed = curSpeed;

            if (curSpeed < MinValidSpeed)
            {
                // Если скорость очень мала, используем комбинированный подход:
                // - Берем 40% от исторической скорости
                // - Но не менее DefaultLowSpeed
                double historicalSpeed = totalDistance / tHistSeconds;
                effectiveSpeed = Math.Max(DefaultLowSpeed, historicalSpeed * 0.4);
            }

            // 3. Расчет коэффициента с учетом скорректированной скорости
            double weight = CalculateEtaCoefficient(
                totalDistance,
                remainingDistance);

            // 4. Расчет ETA
            double currentETA = remainingDistance / effectiveSpeed + secondsFromDeparture;

            return weight * currentETA + (1 - weight) * tHistSeconds;
        }

        private static double CalculateEtaCoefficient(
            double totalDistance,
            double remainingDistance,
            double minWeight = 0.1,
            double maxWeight = 0.7)
        {
            // Рассчитываем пройденную часть маршрута (0-1)
            double traveledRatio = 1 - (remainingDistance / totalDistance);

            // Логистическая функция для плавного перехода
            double k = 5; // Коэффициент крутизны перехода
            double weight = minWeight + (maxWeight - minWeight) /
                           (1 + Math.Exp(-k * (traveledRatio - 0.7)));

            return weight;
        }

        private static double ComputeAlphaSmoothed(double tCurrent, double tHist)
        {
            if (tHist == 0)
                return 0.5; // fallback

            var deviation = Math.Abs(tCurrent - tHist) / tHist;

            // Плавная интерполяция α от 0.3 до 0.9
            deviation = Math.Min(deviation, 1.0); // clamp

            double alphaMin = 0.3;
            double alphaMax = 0.9;

            return alphaMin + (alphaMax - alphaMin) * deviation;
        }
    }
}
