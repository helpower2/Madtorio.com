using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactorioServer.Models
{
    class FactorioGameData
    {
        public FactorioGameTimeData Time { get; } = new();

        public ConcurrentDictionary<string, FactorioPlayerData> Players { get; } = new();
        public ConcurrentDictionary<string, FactorioModData> Mods { get; } = new();

        public string MapString { get; set; } = string.Empty;
    }
}
