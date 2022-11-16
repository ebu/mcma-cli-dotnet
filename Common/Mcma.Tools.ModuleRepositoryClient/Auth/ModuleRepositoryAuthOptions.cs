internal class ModuleRepositoryAuthOptions
{
    public string LoginUrl { get; set; } = "https://modules.mcma.io/login";
    
    public string TokenUrl { get; set; } = "https://auth.mcma.io/token";

    public string WebSocketCallbackUrl { get; set; } = "wss://auth-ws.mcma.io";

    public string ClientId { get; set; } = "52stnrvpeb0b3d0176ilv4e17u";
}