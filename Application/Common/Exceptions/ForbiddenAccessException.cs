// src/Application/Common/Exceptions/ForbiddenAccessException.cs
namespace SaaS.MaundCalculator.Application.Common.Exceptions;

public class ForbiddenAccessException : Exception
{
    public ForbiddenAccessException() : base("Access forbidden.") { }
}