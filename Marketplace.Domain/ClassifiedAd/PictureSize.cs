using System;
using System.Collections.Generic;
using Marketplace.EventSourcing;

namespace Marketplace.Domain.ClassifiedAd;

public class PictureSize : ValueObject
{
    public PictureSize(int width, int height)
    {
        if (width <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(width),
                "Picture width must be a positive number");

        if (height <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(height),
                "Picture height must be a positive number");

        Width = width;
        Height = height;
    }

    internal PictureSize()
    {
    }

    public int Width { get; internal set; }
    public int Height { get; internal set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Width;
        yield return Height;
    }
}