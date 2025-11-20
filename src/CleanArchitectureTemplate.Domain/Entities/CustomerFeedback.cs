using CleanArchitectureTemplate.Domain.Commons;
using CleanArchitectureTemplate.Domain.Enums;

namespace CleanArchitectureTemplate.Domain.Entities;

/// <summary>
/// Phản hồi & khiếu nại của khách hàng
/// </summary>
public class CustomerFeedback : BaseEntity
{
    public Guid CustomerId { get; set; }
    public Guid? OrderId { get; set; }
    public Guid DealerId { get; set; }
    
    public string Subject { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int Rating { get; set; } // 1-5
    
    public FeedbackStatus FeedbackStatus { get; set; } = FeedbackStatus.Open;
    public ComplaintStatus? ComplaintStatus { get; set; } // Nếu là khiếu nại
    
    public string Response { get; set; } = string.Empty;
    public DateTime? ResponseDate { get; set; }
    public Guid? RespondedBy { get; set; }
    
    // Navigation properties
    public Customer Customer { get; set; } = null!;
    public Order? Order { get; set; }
    public Dealer Dealer { get; set; } = null!;
    public User? Responder { get; set; }
}
