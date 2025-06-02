namespace App.Core.Helpers
{
    public static class EtaEstimator
    {
        public static double EstimateEtaSeconds(double tCurrentSeconds, double tHistSeconds)
        {
            var alpha = ComputeAlphaSmoothed(tCurrentSeconds, tHistSeconds);
            return alpha * tCurrentSeconds + (1 - alpha) * tHistSeconds;
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
