namespace App.Infrastructure.Mapping
{
    public static class IdParser
    {
        public static int SafeParseId(string id)
        {
            return int.TryParse(id, out int result) ? result : 0;
        }
    }
}
