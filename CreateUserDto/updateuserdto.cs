public class UpdateUserDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string MobileNumber { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? Gender { get; set; }
    public string? Category1 { get; set; }
    public string? Category2 { get; set; }
    public string? Category3 { get; set; }
    public string? Category4 { get; set; }
    public DateTime? RegistrationDate { get; set; }
    public DateTime? SubscriptionExpirationDate { get; set; }
    public string? RegistrationCode { get; set; }
    public string? Identifier { get; set; }
    public bool? IsVerified { get; set; }
    public List<string> Roles { get; set; }
    public bool? IsActive { get; set; }  // Include this property
}
