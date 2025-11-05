// src/Domain/Entities/IAuditable.cs
namespace SaaS.MaundCalculator.Domain.Entities;

public interface IAuditable
{
    DateTime CreatedAt { get; }
    string CreatedBy { get; }
    DateTime? UpdatedAt { get; }
    string? UpdatedBy { get; }
}