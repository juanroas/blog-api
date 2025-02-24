using Blog.Domain.Models;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

namespace Blog.Infrastructure.Services
{
    public class NotificationService
    {
        private readonly ConcurrentDictionary<string, WebSocket> _connections = new();

        public async Task HandleWebSocketConnection(string userId, WebSocket socket)
        {
            _connections.TryAdd(userId, socket);
            Console.WriteLine($"Usuário {userId} conectado ao WebSocket. Total de conexões ativas: {_connections.Count}");


            try
            {
                var buffer = new byte[1024 * 4];
                while (socket.State == WebSocketState.Open)
                {
                    var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        _connections.TryRemove(userId, out _);

                        Console.WriteLine($"Usuário {userId} desconectado. Total de conexões ativas: {_connections.Count}");

                        await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro no WebSocket: {ex.Message}");
                _connections.TryRemove(userId, out _);
            }
        }


        public async Task NotifyNewPost(string message)
        {
            Console.WriteLine($"Tentando notificar {_connections.Count} usuários.");
            var messageBytes = Encoding.UTF8.GetBytes(message);

            foreach (var (userId, socket) in _connections)
            {
                if (socket.State == WebSocketState.Open)
                {
                    await socket.SendAsync(
                        new ArraySegment<byte>(messageBytes),
                        WebSocketMessageType.Text,
                        true,
                        CancellationToken.None
                    );
                    Console.WriteLine($"Notificação enviada para {userId}");
                }
                else
                {
                    _connections.TryRemove(userId, out _);
                    Console.WriteLine($"Removendo conexão inativa: {userId}");
                }
            }
        }
    }
}
