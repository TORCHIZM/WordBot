using System.Collections.Generic;

using Torchizm_Bot.Database;
using Torchizm_Bot.Modules;

namespace Torchizm_Bot.Models
{
    public class Server
    {
        public int ID { get; set; }
        public ulong UID { get; set; }
        public ulong WordChannel { get; set; }
        public int RemainingToWin { get; set; }

        // Parameterless constructor for System.Activator assembly
        public Server()
        {
        }

        public void Save()
        {
            MySQL.Execute($"UPDATE servers SET uid={UID}, wordChannel={WordChannel}, remainingToWin={RemainingToWin} WHERE uid={UID}");

            Cache.WordChannel.Remove(UID);
            Cache.WordChannel.Add(UID, WordChannel);

            Cache.WinConstants.Remove(UID);
            Cache.WinConstants.Add(UID, RemainingToWin);
        }

        public static List<Server> Get(ulong Uid)
        {
            return MySQL.Select<Server>($"SELECT * FROM servers WHERE uid={Uid}");
        }

        public static List<Server> GetAll()
        {
            return MySQL.Select<Server>("SELECT * FROM servers");
        }

        public static void New(ulong uid, ulong wordChannel)
        {
            MySQL.Execute($"INSERT INTO servers (uid, wordChannel, remainingToWin) VALUES ({uid}, {wordChannel}, 30)");

            Cache.WordChannel.Add(uid, wordChannel);
            Cache.WinConstants.Add(uid, 30);
        }
    }
}
