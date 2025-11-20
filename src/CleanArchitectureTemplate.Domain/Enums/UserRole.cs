namespace CleanArchitectureTemplate.Domain.Enums;

/// <summary>
/// User roles enumeration for Electric Vehicle Dealer Management System
/// </summary>
public enum UserRole
{
    /// <summary>
    /// System Administrator - Full access to all features
    /// </summary>
    Admin = 0,

    /// <summary>
    /// EVM Staff - Electric Vehicle Manufacturer staff
    /// </summary>
    EVMStaff = 1,

    /// <summary>
    /// EVM Manager - Electric Vehicle Manufacturer manager
    /// </summary>
    EVMManager = 2,

    /// <summary>
    /// Dealer Manager - Manages dealer operations
    /// </summary>
    DealerManager = 3,

    /// <summary>
    /// Dealer Staff - Sales staff at dealer
    /// </summary>
    DealerStaff = 4,

    /// <summary>
    /// Customer - End customer (optional for customer portal)
    /// </summary>
    Customer = 5
}

/// <summary>
/// Status enumeration for general use
/// </summary>
public enum Status
{
    /// <summary>
    /// Inactive status
    /// </summary>
    Inactive = 0,
    
    /// <summary>
    /// Active status
    /// </summary>
    Active = 1,
    
    /// <summary>
    /// Pending status
    /// </summary>
    Pending = 2,
    
    /// <summary>
    /// Suspended status
    /// </summary>
    Suspended = 3
}
