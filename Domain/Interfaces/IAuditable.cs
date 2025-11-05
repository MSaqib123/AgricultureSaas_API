// src/Domain/Entities/IAuditable.cs
namespace Domain.Entities;

public interface IAuditable
{
    DateTime CreatedAt { get; }
    string CreatedBy { get; }
    DateTime? UpdatedAt { get; }
    string? UpdatedBy { get; }
}