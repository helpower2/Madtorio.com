using FactorioServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactorioServer.models
{
    class FactorioData
    {
        public FactorioData(string uri, string? name)
        {
            Server = new FactorioServerData(uri, name);
        }

        public FactorioServerData Server { get; }
        public FactorioGameData Game { get; } = new();
    }
}
