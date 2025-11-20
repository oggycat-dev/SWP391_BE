using CleanArchitectureTemplate.Domain.Commons;

namespace CleanArchitectureTemplate.Domain.Entities;

/// <summary>
/// Khách hàng
/// </summary>
public class Customer : BaseEntity
{
    public string CustomerCode { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string IdNumber { get; set; } = string.Empty; // CMND/CCCD
    public DateTime? DateOfBirth { get; set; }
    
    public string Notes { get; set; } = string.Empty;
    
    public Guid CreatedByDealerId { get; set; } // Đại lý nào tạo khách hàng này
    
    // Navigation properties
    public Dealer CreatedByDealer { get; set; } = null!;
    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public ICollection<TestDrive> TestDrives { get; set; } = new List<TestDrive>();
    public ICollection<CustomerFeedback> Feedbacks { get; set; } = new List<CustomerFeedback>();
}
