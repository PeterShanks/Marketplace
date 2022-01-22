namespace Marketplace.Ads.Domain.ClassifiedAds;

public static class PictureRules
{
    public static bool HasCorrectSize(this Picture picture)
    {
        return picture is not null
               && picture.Size.Width >= 800
               && picture.Size.Height >= 600;
    }
}