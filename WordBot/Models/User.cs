using System.Collections.Generic;
using System.Linq;

using Torchizm_Bot.Database;

namespace Torchizm_Bot.Models
{
    public class User
    {
        public int ID { get; set; }
        public ulong UID { get; set; }
        public int Points { get; set; }

        // Parameterless constructor for System.Activator assembly
        public User()
        {
        }

        public void Save()
        {
            MySQL.Execute($"UPDATE users SET uid={UID}, points={Points} WHERE uid={UID}");
        }

        public static List<User> Get(ulong Uid)
        {
            var user = MySQL.Select<User>($"SELECT * FROM users WHERE uid={Uid}");

            if (user.FirstOrDefault() == null)
                New(Uid, 0);
            else
                return user;

            return new List<User>() {
                new User() {
                    UID = Uid,
                    Points = 0
                }
            };
        }

        public static List<User> GetAll()
        {
            return MySQL.Select<User>("SELECT * FROM users");
        }

        public static void New(ulong uid, int points)
        {
            MySQL.Execute($"INSERT INTO users (UID, points) VALUES ({uid}, {points})");
        }
    }
}
