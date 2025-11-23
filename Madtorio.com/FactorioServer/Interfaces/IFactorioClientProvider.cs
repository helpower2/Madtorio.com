using FactorioServer.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FactorioServer.FactorioClientProvider;

namespace FactorioServer.Interfaces
{
    public interface IFactorioClientProvider
    {
        Task<GetConnectedClientResult> TryGetConnectedclient();
       
    }
}
