// src/Domain/Entities/AppUser.cs
namespace Domain.Entities;

public class AppUser : Entity<Guid>
{
    public string Email { get; private set; } = null!;
    public string FullName { get; private set; } = null!;
    public Guid? TenantId { get; private set; }
    public string? ProfilePictureUrl { get; private set; }

    private AppUser() { }

    public static AppUser Create(string email, string fullName, Guid? tenantId = null)
    {
        return new AppUser
        {
            Id = Guid.NewGuid(),
            Email = email,
            FullName = fullName,
            TenantId = tenantId
        };
    }

    public void UpdateProfilePicture(string url)
    {
        ProfilePictureUrl = url;
    }
}