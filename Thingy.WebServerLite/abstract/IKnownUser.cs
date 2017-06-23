namespace Thingy.WebServerLite
{
    public interface IKnownUser
    {
        string IpAddress { get; set; }
        string Password { get; }
        string[] Roles { get; }
        string UserId { get; }
    }
}