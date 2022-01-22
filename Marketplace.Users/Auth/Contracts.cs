namespace Marketplace.Users.Auth;

public static class Contracts
{
    public record Login(string Username, string Password);
}