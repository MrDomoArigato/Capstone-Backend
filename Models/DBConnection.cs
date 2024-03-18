
namespace CapstoneApi.Data;

public class DBConnection
{
    public required string Host { get; init; }

    public required string Port { get; init; }

    public required string Username { get; init; }

    public required string Password { get; init; }

    public required string Database { get; init; }
}