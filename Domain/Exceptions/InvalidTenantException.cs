// src/Domain/Exceptions/InvalidTenantException.cs
namespace Domain.Exceptions;

public class InvalidTenantException : DomainException
{
    public InvalidTenantException(string message) : base(message) { }
}