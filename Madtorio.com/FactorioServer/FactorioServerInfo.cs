using FactorioSharp.Rcon;
using FactorioServer.models;
using FactorioServer.Interfaces;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using FactorioServer.Models;
using System.Dynamic;

namespace FactorioServer
{
    public class FactorioServerInfo
    {

        FactorioServerData _serverData;
        IFactorioClientProvider _clientProvider;
        FactorioServerOptions _factorioServerOptions;
        readonly ILogger<FactorioServerInfo> _logger;

        public FactorioServerInfo(IFactorioClientProvider client, FactorioServerOptions factorioServerOptions, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<FactorioServerInfo>();
            if (client == null)
                throw new InvalidOperationException("Client provider is not initialized.");
            if (factorioServerOptions.RconPassword == null || factorioServerOptions.RconUri == null)
                throw new InvalidOperationException("Factorio server options are not initialized.");
            _clientProvider =  client;
            _factorioServerOptions = factorioServerOptions;
        }
        public async Task<FactorioRconClient> GetClient()
        {
            var clientResualt = await _clientProvider.TryGetConnectedclient();
            if (!clientResualt.Succeeded)
            {
                throw new InvalidOperationException(clientResualt.FailureReason);
            }
            if(clientResualt.Client is not null)
            {
                return clientResualt.Client;
            }
            throw new InvalidOperationException("Client is null.");
        }

        public async Task<FactorioServerData> GetFacotrioServerData()
        {
            var client = await GetClient();
            Dictionary<string, string> mods = await client.ReadAsync(g => g.Game.ActiveMods) ?? new Dictionary<string, string>();
            string activeModsString = string.Join(", ", mods.Select(e => $"{e.Key} v{e.Value}"));
            if (!mods.TryGetValue("base", out string? baseVersion))
            {
                _logger.LogWarning("Could not determine base version, active mods are: {mods}", activeModsString);
                baseVersion = "unknown";
            }
            else
            {
                _logger.LogInformation("Factorio version: {version}", baseVersion);
                _logger.LogInformation("Active mods: {mods}", activeModsString);
            }
            _serverData = new FactorioServerData(_factorioServerOptions.RconUri.Host, _factorioServerOptions.Name)
            {
                FactorioVersion = baseVersion,
                IsUp = true
            };
            return _serverData;

        }

        public async Task<FactorioModData[]> GetFactorioModData()
        {

            var client = await GetClient();
            Dictionary<string, string> mods = await client.ReadAsync(g => g.Game.ActiveMods) ?? new Dictionary<string, string>();
            string activeModsString = string.Join(", ", mods.Select(e => $"{e.Key} v{e.Value}"));
            if (!mods.TryGetValue("base", out string? baseVersion))
            {
                _logger.LogWarning("Could not determine base version, active mods are: {mods}", activeModsString);
                baseVersion = "unknown";
            }
            else
            {
                _logger.LogInformation("Factorio version: {version}", baseVersion);
                _logger.LogInformation("Active mods: {mods}", activeModsString);
            }
            var Mods = new List<FactorioModData>();
            foreach (var mod in mods)
            {
                Mods.Add(new FactorioModData
                {
                    Version = mod.Value,
                    IsActive = true
                });
            }

            return Mods.ToArray<FactorioModData>();
        }

        public async Task<string> GetMapstring()
        {
            var client = await GetClient();
            var mapString = await client.ReadAsync(g => g.Game.GetMapExchangeString());
            return mapString ?? string.Empty;
        }
        public async Task<List<string>> Getplayers()
        {
            var client = await GetClient();
            List<string> players = new();
            uint totalPlayerCount = await client.ReadAsync(g => g.Game.Players.Length);
            for (int index = 0; index < totalPlayerCount; index++)
            {
                string? player = await client.ReadAsync((g, i) => g.Game.Players[i + 1].Name, (uint)index);
                if (player == null) continue;

                players.Add(player);
            }
            return players;
        }

        public async Task<List<string>> GetConnectedPlayers()
        {
            var client = await GetClient();
            int connectedPlayerCount = await client.ReadAsync(g => g.Game.ConnectedPlayers.Count);

            List<string> connectedPlayers = new();
            for (int index = 0; index < connectedPlayerCount; index++)
            {
                string? player = await client.ReadAsync((g, i) => g.Game.ConnectedPlayers[i + 1].Name, index);
                if (player == null) continue;

                connectedPlayers.Add(player);
            }
            return connectedPlayers;
        }

    }
}
