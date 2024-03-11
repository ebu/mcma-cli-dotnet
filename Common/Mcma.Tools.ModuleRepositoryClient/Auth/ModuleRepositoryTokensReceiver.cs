using System.Net.WebSockets;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Mcma.Tools.ModuleRepositoryClient.Auth;

internal class ModuleRepositoryTokensReceiver : IModuleRepositoryTokensReceiver
{
    private ClientWebSocket ClientWebSocket { get; } = new();

    private async Task<JObject> ReceiveJsonAsync(CancellationToken cancellationToken)
    {
        var messageBytes = new List<byte>();
        
        ValueWebSocketReceiveResult result;
        do
        {
            var buffer = new Memory<byte>(new byte[10240]);
            result = await ClientWebSocket.ReceiveAsync(buffer, cancellationToken);
            messageBytes.AddRange(buffer.ToArray());
        }
        while (!result.EndOfMessage);
        
        var respText = Encoding.UTF8.GetString(messageBytes.ToArray());
        
        return JObject.Parse(respText);
    }

    public async Task<string> StartAsync(string wssEndpoint, CancellationToken cancellationToken)
    {
        await ClientWebSocket.ConnectAsync(new Uri(wssEndpoint), cancellationToken);

        await ClientWebSocket.SendAsync(
            Encoding.UTF8.GetBytes(JObject.FromObject(new { action = "GetConnectionId" }).ToString()),
            WebSocketMessageType.Text,
            true,
            cancellationToken);

        var respJson = await ReceiveJsonAsync(cancellationToken);

        if (respJson["connectionId"]?.Value<string>() is not string connectionId)
            throw new Exception($"Failed to start tokens receiver. '{nameof(connectionId)}' is null");

        return connectionId;
    }

    public async Task<ModuleRepositoryAuthTokens> WaitForTokensAsync(CancellationToken cancellationToken)
    {
        var respJson = await ReceiveJsonAsync(cancellationToken);

        return respJson.ToObject<ModuleRepositoryAuthTokens>();
    }
}