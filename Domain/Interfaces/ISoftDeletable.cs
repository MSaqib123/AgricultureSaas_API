// src/Domain/Entities/ISoftDeletable.cs
namespace Domain.Entities;

public interface ISoftDeletable
{
    bool IsDeleted { get; }
    DateTime? DeletedAt { get; }
}