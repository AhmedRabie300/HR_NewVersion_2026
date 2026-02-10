 namespace VenusHR.Application.Common.DTOs.Groups;

public class CreateGroupDto
{
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? ArabicName { get; set; }
    public string? Description { get; set; }
    public bool? IsActive { get; set; } = true;
}

 
 