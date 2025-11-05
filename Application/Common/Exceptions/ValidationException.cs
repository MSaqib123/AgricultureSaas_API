// src/Application/Common/Exceptions/ValidationException.cs
using FluentValidation.Results;

namespace SaaS.MaundCalculator.Application.Common.Exceptions;

public class ValidationException : Exception
{
    public ValidationException(IEnumerable<ValidationFailure> failures)
        : base("One or more validation failures have occurred.")
    {
        Errors = failures.Select(f => new ValidationError(f.PropertyName, f.ErrorMessage));
    }

    public IEnumerable<ValidationError> Errors { get; }
}

public record ValidationError(string PropertyName, string ErrorMessage);