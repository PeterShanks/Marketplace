using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Marketplace.EventSourcing;
using static System.String;

namespace Marketplace.Domain.ClassifiedAd;

public class ClassifiedAdTitle : ValueObject
{
    internal ClassifiedAdTitle(string value)
    {
        Value = value;
    }

    // Satisfy the serialization requirements 
    protected ClassifiedAdTitle()
    {
    }

    public string Value { get; }

    public static ClassifiedAdTitle FromString(string title)
    {
        CheckValidity(title);
        return new ClassifiedAdTitle(title);
    }

    public static ClassifiedAdTitle FromHtml(string htmlTitle)
    {
        var supportedTagsReplaced = htmlTitle
            .Replace("<i>", "*")
            .Replace("</i>", "*")
            .Replace("<b>", "**")
            .Replace("</b>", "**");

        var value = Regex.Replace(supportedTagsReplaced, "<.*?>", Empty);
        CheckValidity(value);
        return new ClassifiedAdTitle(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator string(ClassifiedAdTitle title)
    {
        return title.Value;
    }

    private static void CheckValidity(string value)
    {
        if (IsNullOrEmpty(value))
            throw new ArgumentNullException(
                nameof(value),
                "Title cannot be null");

        switch (value.Length)
        {
            case < 10:
                throw new ArgumentOutOfRangeException(
                    nameof(value),
                    "Title cannot be shorter than 10 characters");

            case > 100:
                throw new ArgumentOutOfRangeException(nameof(value),
                    "Title cannot be longer than 100 characters");
        }
    }
}