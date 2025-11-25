namespace Madtorio.com.ApiService.Models
{
    /// <summary>
    /// this class holds information about the factorio server
    /// </summary>
    public class ServerInfo
    {
        public string Uri { get; set; }
        public string? Name { get; set; }
        public string? FactorioVersion { get; set; }
        public bool IsUp { get; set; } = false;
        public int OnlinePlayerCount { get; set; }
        public string Mapstring { get; set; }
        public int Totalplayers { get; set; }
        public List<string> playersOnline { get; set; } = new();
    }
}
