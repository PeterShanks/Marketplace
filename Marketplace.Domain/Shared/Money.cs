using System;
using System.Collections.Generic;
using Marketplace.EventSourcing;
using static Marketplace.Domain.Shared.DomainExceptions;

namespace Marketplace.Domain.Shared;

public class Money : ValueObject
{
    public const string DefaultCurrency = "EUR";

    protected Money(decimal amount, string currencyCode, ICurrencyLookup currencyLookup)
    {
        if (string.IsNullOrEmpty(currencyCode))
            throw new ArgumentNullException(nameof(currencyCode), "Currency code must be specified");

        var currency = currencyLookup.FindCurrency(currencyCode);
        if (!currency.InUse)
            throw new ArgumentException($"Currency {currencyCode} is not valid");

        if (decimal.Round(amount, currency.DecimalPlaces) != amount)
            throw new ArgumentOutOfRangeException(nameof(amount),
                $"Amount cannot have more than {currency.DecimalPlaces} decimals");

        Amount = amount;
        Currency = currency;
    }

    protected Money(decimal amount, Currency currency)
    {
        Amount = amount;
        Currency = currency;
    }

    protected Money()
    {
    }

    public decimal Amount { get; }
    public Currency Currency { get; }

    public static Money FromDecimal(decimal amount, string currency, ICurrencyLookup currencyLookup)
    {
        return new(amount, currency, currencyLookup);
    }

    public static Money FromString(string amount, string currency, ICurrencyLookup currencyLookup)
    {
        return new(decimal.Parse(amount), currency, currencyLookup);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }

    public Money Add(Money summand)
    {
        if (summand.Currency != Currency)
            throw new CurrencyMismatchException("Cannot sum amounts with different currencies");

        return new Money(Amount + summand.Amount, Currency);
    }

    public Money Subtract(Money subtrahend)
    {
        if (subtrahend.Currency != Currency)
            throw new CurrencyMismatchException("Cannot subtract amounts with different currencies");

        return new Money(Amount - subtrahend.Amount, Currency);
    }

    public static Money operator +(Money summand1, Money summand2)
    {
        return summand1.Add(summand2);
    }

    public static Money operator -(Money minuend, Money subtrahend)
    {
        return minuend.Subtract(subtrahend);
    }

    public override string ToString()
    {
        return $"{Currency.CurrencyCode}  {Amount}";
    }
}