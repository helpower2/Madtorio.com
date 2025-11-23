using FactorioServer.Interfaces;
using FactorioServer.models;
using FactorioSharp.Rcon;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactorioServer
{
    public class FactorioClientProvider : IDisposable, IFactorioClientProvider
    {
        FactorioRconClient? _client;
        readonly FactorioServerOptions _optionsMonitor;
        readonly ILogger<FactorioClientProvider> _logger;

        public FactorioClientProvider(FactorioServerOptions configuration, ILogger<FactorioClientProvider> logger)
        {
            if (configuration is null)
                throw new ArgumentNullException(nameof(configuration));
            if (configuration.RconUri == null || configuration.RconPassword == null)
                throw new InvalidOperationException("Could not determine server URI or RCON password");
            if (logger is null)
                throw new ArgumentNullException(nameof(logger));
            _optionsMonitor = configuration;
            _logger = logger;
        }

        /// <summary>
        ///     Get a <see cref="FactorioRconClient" /> connected to the server.
        /// </summary>
        /// <remarks>
        ///     if a client can be reused, it will be.
        /// </remarks>
        public async Task<GetConnectedClientResult> TryGetConnectedclient()
        {
            var _options = _optionsMonitor;
            if (_options.RconUri == null || _options.RconPassword == null)
            {
                throw new InvalidOperationException("Could not determine server URI or RCON password");
            }

            try
            {
                if (_client is { Connected: true })
                {
                    return GetConnectedClientResult.Success(_options.RconUri, _client);
                }

                if (_client != null)
                {
                    _logger.LogDebug("Connection to {uri} has been lost, reconnection attempt...", _options.RconUri);
                    _client.Dispose();
                }
                else
                {
                    _logger.LogDebug("Connection attempt to {uri}...", _options.RconUri);
                }
                _client = new FactorioRconClient(_options.RconUri.Host, _options.RconUri.Port) { Silent = _options.SilentCommands };

                if (await _client.ConnectAsync(_options.RconPassword))
                {
                    _logger.LogDebug("Connected to {uri}.", _options.RconUri);
                    return GetConnectedClientResult.Success(_options.RconUri, _client);
                }

                _logger.LogDebug("Connection to {uri} failed.", _options.RconUri);

                _client.Dispose();
                _client = null;
                return GetConnectedClientResult.Failure(_options.RconUri, $"Connection or authentication to {_options.RconUri} failed, double check the host, port and password.");
            }
            catch (Exception exn)
            {
                return GetConnectedClientResult.Failure(_options.RconUri, exn);
            }
        }

        public void Dispose()
        {
            _client?.Dispose();
            GC.SuppressFinalize(this);
        }


        public class GetConnectedClientResult
        {
            public bool Succeeded => Client != null;
            public Uri Uri { get; }
            public FactorioRconClient? Client { get; }
            public string? FailureReason { get; }
            public Exception? Exception { get; }

            GetConnectedClientResult(Uri uri, FactorioRconClient? client, string? failureReason, Exception? exception)
            {
                Uri = uri;
                Client = client;
                FailureReason = failureReason;
                Exception = exception;
            }

            public static GetConnectedClientResult Success(Uri uri, FactorioRconClient client) => new(uri, client, null, null);
            public static GetConnectedClientResult Failure(Uri uri, Exception exception) => new(uri, null, exception.Message, exception);
            public static GetConnectedClientResult Failure(Uri uri, string reason) => new(uri, null, reason, null);
        }
    }

}
