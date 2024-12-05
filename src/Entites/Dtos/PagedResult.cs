namespace Entities.Dtos
{
    public class PagedResult<T> :Response<List<T>>
    {
        public int Page { get; set; }
        public int ResultCount { get; set; }
        public int ResultsPerPage { get; set; }
    }
}
