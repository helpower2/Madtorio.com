using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactorioServer.models
{
    public class FactorioServerOptions
    {
        /// <summary>
        ///     The URI of the server
        /// </summary>
        public Uri? RconUri { get; set; } = null;


        /// <summary>
        ///     The password of the RCON connection
        /// </summary>
        public string RconPassword { get; set; } = string.Empty;

        /// <summary>
        ///     The name of the server.
        ///     If set, this name will be added as a tag named <c>factorio_server_name</c> on all metrics.
        /// </summary>
        public string Name { get; set; }  = string.Empty;

        /// <summary>
        ///     If false, the commands will be executed on the factorio server using the <c>/c</c> command instead of the silent <c>/sc</c>.
        ///     Setting this to false can help diagnose issues. <br />
        ///     Defaults to true.
        /// </summary>
        public bool SilentCommands { get; set; } = true;
    }
}
