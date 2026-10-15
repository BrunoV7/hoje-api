using Hoje.Domain.Common;

namespace Hoje.Tests.Domain;

public class MoneyTests
{
    private static Money Brl(long cents) => new("BRL", cents);

    [Fact]
    public void Negative_amounts_are_allowed()
    {
        var money = Brl(-150);

        Assert.Equal(-150, money.Cents);
        Assert.True(money.IsNegative);
    }

    [Fact]
    public void Subtract_can_go_negative()
    {
        var result = Brl(100) - Brl(250);

        Assert.Equal(Brl(-150), result);
        Assert.True(result.IsNegative);
    }

    [Fact]
    public void Add_works()
    {
        Assert.Equal(Brl(200), Brl(150) + Brl(50));
    }

    [Fact]
    public void Same_cents_and_currency_are_equal()
    {
        Assert.Equal(Brl(100), Brl(100));
        Assert.True(Brl(100) == Brl(100));
    }

    [Fact]
    public void Currency_is_normalized_to_upper_case()
    {
        Assert.Equal("BRL", new Money("brl", 100).Currency);
    }

    [Theory]
    [InlineData("")]
    [InlineData("BR")]
    [InlineData("REAIS")]
    [InlineData("B1L")]
    public void Invalid_currency_throws(string currency)
    {
        Assert.Throws<ArgumentException>(() => new Money(currency, 100));
    }

    [Fact]
    public void Mixing_currencies_throws()
    {
        Assert.Throws<ArgumentException>(() => Brl(100) + new Money("USD", 100));
    }

    [Fact]
    public void Comparison_operators_work()
    {
        Assert.True(Brl(100) < Brl(200));
        Assert.True(Brl(200) > Brl(100));
        Assert.True(Brl(-50) < Brl(0));
        Assert.True(Brl(100) <= Brl(100));
    }

    [Fact]
    public void Negate_flips_the_sign()
    {
        Assert.Equal(Brl(-100), -Brl(100));
    }

    [Fact]
    public void Add_overflow_throws()
    {
        Assert.Throws<OverflowException>(() => Brl(long.MaxValue) + Brl(1));
    }

    [Theory]
    [InlineData(100, 3)]
    [InlineData(99, 100)]
    [InlineData(1, 3)]
    [InlineData(0, 5)]
    [InlineData(-100, 3)]
    [InlineData(1000, 10)]
    public void Allocate_parts_sum_to_total_and_differ_by_at_most_one_cent(long total, int parts)
    {
        var result = Brl(total).Allocate(parts);

        Assert.Equal(parts, result.Count);
        Assert.Equal(total, result.Sum(m => m.Cents));
        Assert.True(result.Max(m => m.Cents) - result.Min(m => m.Cents) <= 1);
    }

    [Fact]
    public void Allocate_puts_extra_cents_on_the_first_parts()
    {
        var result = Brl(10000).Allocate(3);

        Assert.Equal([Brl(3334), Brl(3333), Brl(3333)], result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Allocate_with_invalid_parts_throws(int parts)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Brl(100).Allocate(parts));
    }
}