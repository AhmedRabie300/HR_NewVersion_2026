 namespace VenusHR.Application.Common.DTOs.Menus
{
    public class MenuDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string EngName { get; set; } = string.Empty;
        public string ArbName { get; set; } = string.Empty;
        public string? ArbName4S { get; set; }
        public int? ParentId { get; set; }
        public string? Shortcut { get; set; }
        public int? Rank { get; set; }
        public int? FormId { get; set; }
        public int? ObjectId { get; set; }
        public int? ViewFormId { get; set; }
        public bool IsHide { get; set; }
        public string? Image { get; set; }
        public int? ViewType { get; set; }   

         public List<MenuDto> Children { get; set; } = new();

         public FormDto? Form { get; set; }
    }
}