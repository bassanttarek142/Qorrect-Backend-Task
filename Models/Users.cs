using Qorrect_Backend_Task.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class User
{
    public int Id { get; set; }

    [MaxLength(400)]
    public string FirstName { get; set; }

    [MaxLength(400)]
    public string LastName { get; set; }

    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; }

    [Phone]
    [MaxLength(15)]
    public string MobileNumber { get; set; }

    public DateTime Date { get; set; }

    public DateTime? BirthDate { get; set; }

    [MaxLength(50)]
    public string? Gender { get; set; }

    [MaxLength(100)]
    public string? Category1 { get; set; }

    [MaxLength(100)]
    public string? Category2 { get; set; }

    [MaxLength(100)]
    public string? Category3 { get; set; }

    [MaxLength(100)]
    public string? Category4 { get; set; }

    public DateTime? RegistrationDate { get; set; }

    public DateTime? SubscriptionExpirationDate { get; set; }

    [MaxLength(10)]
    public string? RegistrationCode { get; set; }

    public string? Identifier { get; set; }

    public bool IsVerified { get; set; }

    public bool IsActive { get; set; }  

    [JsonIgnore]
    public ICollection<Role> Roles { get; set; }

  
    public IEnumerable<string> RoleNames => Roles?.Select(r => r.Name);
}
