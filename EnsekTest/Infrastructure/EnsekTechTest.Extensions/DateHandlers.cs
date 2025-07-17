namespace EnsekTechTest.Extensions
{
    public static class DateHandlers
    {
        public static DateTime ToDateTime(this string orderDate)
        {
            bool isValidDate = DateTime.TryParseExact(orderDate, "ddd, dd MMM yyyy HH:mm:ss 'GMT'",
                                        System.Globalization.CultureInfo.InvariantCulture,
                                        System.Globalization.DateTimeStyles.AssumeUniversal,
                                        out var validDate);

            return isValidDate ? validDate : DateTime.Now;
        }
    }
}
