namespace Marketplace.EventSourcing;

public static class StringTools
{
    public static bool IsEmpty(this string value)
    {
        return string.IsNullOrWhiteSpace(value);
    }
}