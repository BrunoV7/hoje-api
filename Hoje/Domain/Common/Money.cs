namespace Hoje.Domain.Common;

public sealed record Money : IComparable<Money>
{
    public string Currency { get; }
    public long Cents { get; }

    public bool IsZero => Cents == 0;
    public bool IsNegative => Cents < 0;

    public Money(string currency, long cents)
    {
        if (string.IsNullOrEmpty(currency)
            || currency.Length != 3
            || !currency.All(char.IsAsciiLetter))
            throw new ArgumentException(
                "Currency must be a 3-letter ISO code (e.g. BRL).",
                nameof(currency));

        Currency = currency.ToUpperInvariant();
        Cents = cents; // negatives are allowed: overspending, overdraft, card debt
    }

    public static Money Zero(string currency) => new(currency, 0);

    public Money Add(Money other)
    {
        EnsureSameCurrency(other);
        return new Money(Currency, checked(Cents + other.Cents));
    }

    public Money Subtract(Money other)
    {
        EnsureSameCurrency(other);
        return new Money(Currency, checked(Cents - other.Cents));
    }

    public Money Multiply(int multiplier) =>
        new(Currency, checked(Cents * multiplier));

    public Money Negate() =>
        new(Currency, checked(-Cents));

    /// <summary>
    /// Splits this amount into pieces that sum exactly
    /// to the original. Extra cents go to the first parts, so no two parts
    /// differ by more than one cent. Example: R$100,00 / 3 = 33,34 + 33,33 + 33,33.
    /// </summary>
    public IReadOnlyList<Money> Allocate(int parts)
    {
        if (parts <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(parts), "Parts must be greater than zero.");

        var baseCents = Cents / parts;
        var remainder = Cents % parts;      
        var extra = Math.Sign(remainder);    
        var partsWithExtra = Math.Abs(remainder);

        var result = new Money[parts];
        for (var i = 0; i < parts; i++)
        {
            var cents = baseCents + (i < partsWithExtra ? extra : 0);
            result[i] = new Money(Currency, cents);
        }

        return result;
    }

    /// Ratio between two amounts, e.g. how much of a budget was used.
    public decimal Ratio(Money other)
    {
        EnsureSameCurrency(other);

        if (other.IsZero)
            throw new DivideByZeroException();

        return (decimal)Cents / other.Cents;
    }

    public int CompareTo(Money? other)
    {
        EnsureSameCurrency(other);
        return Cents.CompareTo(other.Cents);
    }

    public static Money operator +(Money left, Money right) => left.Add(right);
    public static Money operator -(Money left, Money right) => left.Subtract(right);
    public static Money operator -(Money money) => money.Negate();

    public static bool operator <(Money left, Money right) => left.CompareTo(right) < 0;
    public static bool operator >(Money left, Money right) => left.CompareTo(right) > 0;
    public static bool operator <=(Money left, Money right) => left.CompareTo(right) <= 0;
    public static bool operator >=(Money left, Money right) => left.CompareTo(right) >= 0;

    private void EnsureSameCurrency([System.Diagnostics.CodeAnalysis.NotNull] Money? other)
    {
        ArgumentNullException.ThrowIfNull(other);

        if (Currency != other.Currency)
            throw new ArgumentException("Currencies must be equal.", nameof(other));
    }
}