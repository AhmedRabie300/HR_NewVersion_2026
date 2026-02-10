namespace VenusHR.Application.Common.DTOs.Login;

public class UserGroupDto
{
    public int Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? ArbName { get; set; }
    public string? EngName { get; set; }
    //public bool IsPrimary { get; set; }
    //public DateTime JoinedDate { get; set; }
}