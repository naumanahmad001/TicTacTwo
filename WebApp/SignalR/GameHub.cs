using Microsoft.AspNetCore.SignalR;

namespace WebApp.SignalR
{
    public class GameHub : Hub
    {
        public async Task JoinGame(string playerName)
        {
            await Clients.All.SendAsync("PlayerJoined", playerName);
        }

        public async Task UpdateState(string gameState)
        {
            // Broadcast updated game state to all players
            await Clients.Others.SendAsync("ReceiveState", gameState);
        }

        public async Task NotifyMove(string message)
        {
            await Clients.All.SendAsync("MoveNotification", message);
        }
    }
}
