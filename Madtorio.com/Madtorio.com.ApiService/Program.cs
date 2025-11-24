using FactorioServer;
using FactorioServer.Interfaces;
using FactorioServer.models;
using FactorioSharp.Rcon;
using Madtorio.com.Web.Api.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using OneOf.Types;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);


// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();
FactorioServerOptions factorioServerOptions = new();
builder.Configuration.GetSection(nameof(FactorioServerOptions)).Bind(factorioServerOptions);
builder.Services.AddSingleton(factorioServerOptions);
builder.Services.AddSingleton<IFactorioClientProvider, FactorioClientProvider>();
builder.Services.AddSingleton<FactorioServerInfo>();

// Add services to the container.
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

app.MapGet("/FactorioServerData", async (FactorioServerInfo factorioServerInfo) => { 
    var factorioServerData = await factorioServerInfo.GetFacotrioServerData(); 
    if (!factorioServerData.IsUp)
    {
        return Results.Problem("Could not retrieve factorio server info", statusCode: 500);
    }
    return Results.Ok(factorioServerData);
});

app.MapGet("/serverinfo", async (FactorioServerOptions serveroptions, FactorioServerInfo factorioServerInfo) =>
{
    
    if (serveroptions.RconUri == null || serveroptions.RconPassword == null)
    {
        return Results.Problem("Server URI or RCON password is not configured", statusCode: 500);
    }
    var serverInfo = new ServerInfo() { Uri = serveroptions.RconUri.Host};
    //serverInfo.Uri = serveroptions.RconUri.Host;
    {
        Debug.WriteLine("/serverinfo");
            serverInfo.Mapstring = await factorioServerInfo.GetMapstring();
            //int connectedPlayerCount = await client.ReadAsync(g => g.Game.ConnectedPlayers.Count);
            List<string> connectedPlayers = await factorioServerInfo.GetConnectedPlayers();
            serverInfo.OnlinePlayerCount = connectedPlayers.Count;
            serverInfo.playersOnline = connectedPlayers;
            serverInfo.IsUp = true;

        
    }

    return Results.Ok(serverInfo);
});


app.MapDefaultEndpoints();

app.Run();