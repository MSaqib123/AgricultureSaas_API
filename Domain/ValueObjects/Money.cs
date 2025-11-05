// src/Domain/ValueObjects/Money.cs
namespace Domain.ValueObjects;

public record Money(decimal Amount, string Currency = "PKR")
{
    public static Money Zero => new(0m, "PKR");

    public static Money From(decimal amount, string currency = "PKR")
        => new(amount, currency);

    public override string ToString() => $"{Amount:N2} {Currency}";
}