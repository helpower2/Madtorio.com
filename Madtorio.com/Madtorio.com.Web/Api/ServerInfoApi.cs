namespace Madtorio.com.Web.Api
{
    using Madtorio.com.Web.Api.Models;
    public class ServerInfoApi(HttpClient httpClient)
    {
        public async Task<ServerInfo> GetServerInfoAsync(CancellationToken cancellationToken = default)
        {
            var serverinfo = await httpClient.GetFromJsonAsync<ServerInfo>("/serverinfo", cancellationToken);
            if (serverinfo is not null)
            {
                return serverinfo;
            }
            return new ServerInfo();
        }
    }
}
