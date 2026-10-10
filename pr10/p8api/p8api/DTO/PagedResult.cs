namespace p8api.DTOs
{
    public record PagedResult<T>(List<T> Items, int TotalCount);
}
