using System.Collections.Generic;
using Torchizm_Bot.Models;

namespace Torchizm_Bot.Modules
{
    public static class Cache
    {
        public static Dictionary<ulong, ulong> WordChannel = new Dictionary<ulong, ulong>();
        public static Dictionary<ulong, int> WinConstants = new Dictionary<ulong, int>();

        public static void Initialize()
        {
            var servers = Server.GetAll();

            foreach (var server in servers)
            {
                WordChannel.Add(server.UID, server.WordChannel);
                WinConstants.Add(server.UID, server.RemainingToWin);
            }
        }
    }
}
