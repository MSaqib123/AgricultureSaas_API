// src/Domain/Entities/ISoftDeletable.cs
namespace SaaS.MaundCalculator.Domain.Entities;

public interface ISoftDeletable
{
    bool IsDeleted { get; }
    DateTime? DeletedAt { get; }
}