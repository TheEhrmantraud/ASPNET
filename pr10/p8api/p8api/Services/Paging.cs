namespace p8api.Services
{
    public static class Paging
    {
        public const int DefaultPageSize = 10;
        public const int MaxPageSize = 100;

        public static (int Page, int PageSize) Normalize(int page, int pageSize)
        {
            if (page < 1)
            {
                page = 1;
            }

            if (pageSize < 1 || pageSize > MaxPageSize)
            {
                pageSize = DefaultPageSize;
            }

            return (page, pageSize);
        }
    }
}
