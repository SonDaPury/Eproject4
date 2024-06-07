namespace backend.Data
{
    public static class CreateCacheKey
    {
        public static string BuildUserCacheKey(int userId) => $"buck-us{userId}";
    }
}
