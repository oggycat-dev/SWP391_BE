namespace CleanArchitectureTemplate.Application.Common.DTOs.Dealers;

public class DealerStaffDto
{
    public Guid Id { get; set; }
    public Guid DealerId { get; set; }
    public string DealerName { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public bool IsManager { get; set; }
    public DateTime JoinDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsActive { get; set; }
}

public class CreateDealerStaffRequest
{
    public Guid DealerId { get; set; }
    public Guid UserId { get; set; }
    public string Position { get; set; } = string.Empty;
    public bool IsManager { get; set; }
    public DateTime JoinDate { get; set; }
}

public class UpdateDealerStaffRequest
{
    public string Position { get; set; } = string.Empty;
    public bool IsManager { get; set; }
    public bool IsActive { get; set; }
}
