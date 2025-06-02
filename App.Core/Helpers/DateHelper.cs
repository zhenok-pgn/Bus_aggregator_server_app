using App.Core.Enums;

namespace App.Core.Helpers
{
    public static class DateHelper
    {
        public static List<DateOnly> GetValidTripDates(
            DateOnly patternStart,
            DateOnly patternEnd,
            DaysOfWeekFlags daysOfWeek,
            DateOnly requestFrom,
            DateOnly requestTo)
        {
            // 1. Определяем общий допустимый диапазон
            var effectiveStart = new[] { patternStart, requestFrom }.Max();
            var effectiveEnd = new[] { patternEnd, requestTo }.Min();

            if (effectiveStart > effectiveEnd)
                return new List<DateOnly>();

            // 2. Генерируем все даты в диапазоне и фильтруем по дням недели
            return GetDatesInRange(effectiveStart, effectiveEnd)
                .Where(date => IsDayIncluded(date, daysOfWeek))
                .ToList();
        }

        public static bool IsDayIncluded(DateOnly date, DaysOfWeekFlags daysFilter)
        {
            int dayShift = (int)date.DayOfWeek - 1;
            if (dayShift == -1) dayShift = 6;

            return (daysFilter & (DaysOfWeekFlags)(1 << dayShift)) != 0;
        }

        public static IEnumerable<DateOnly> GetDatesInRange(
            DateOnly start,
            DateOnly end,
            DaysOfWeekFlags? daysFilter = null)
        {
            for (var date = start; date <= end; date = date.AddDays(1))
            {
                if (daysFilter == null || IsDayIncluded(date, daysFilter.Value))
                    yield return date;
            }
        }
    }
}
