namespace Application.System.Search.Dtos
{
    public class SearchExecuteRequestDto
    {
        public int SearchID { get; set; }
        public string? SearchTerm { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? SortBy { get; set; }
        public bool SortDescending { get; set; }
        public int? UserId { get; set; }
        public string? AnotherCriteria { get; set; }
        public bool LoadAll { get; set; } = false;       
        public int? LoadLimit { get; set; } = null;
    }
}