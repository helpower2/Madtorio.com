using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FactorioServer.models
{
    public class FactorioServerData
    {
        public FactorioServerData(string factorioUri, string? name)
        {
            FactorioUri = factorioUri;
            Name = name;
        }

        public string FactorioUri { get; set; }
        public string? Name { get; set; }
        public string? FactorioVersion { get; set; }
        public bool IsUp { get; set; }
    }
}
