using CleanArchitectureTemplate.Domain.Enums;

namespace CleanArchitectureTemplate.Application.Common.DTOs.CustomerFeedbacks;

public class CustomerFeedbackDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public Guid? OrderId { get; set; }
    public string? OrderNumber { get; set; }
    public Guid DealerId { get; set; }
    public string DealerName { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int Rating { get; set; }
    public FeedbackStatus FeedbackStatus { get; set; }
    public ComplaintStatus? ComplaintStatus { get; set; }
    public string Response { get; set; } = string.Empty;
    public DateTime? ResponseDate { get; set; }
    public Guid? RespondedBy { get; set; }
    public string? ResponderName { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class SubmitFeedbackRequest
{
    public Guid CustomerId { get; set; }
    public Guid? OrderId { get; set; }
    public Guid DealerId { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int Rating { get; set; }
    public bool IsComplaint { get; set; }
}

public class RespondToFeedbackRequest
{
    public string Response { get; set; } = string.Empty;
}

public class UpdateFeedbackStatusRequest
{
    public FeedbackStatus? FeedbackStatus { get; set; }
    public ComplaintStatus? ComplaintStatus { get; set; }
}

