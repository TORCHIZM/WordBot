using Discord;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;
using Torchizm_Bot.API;
using Torchizm_Bot.Models;

namespace Torchizm_Bot.Modules
{
    public class WordGame
    {
        internal static async Task MesageReceived(SocketMessage message)
        {
            if (message.Author.IsBot) return;

            var channel = Cache.WordChannel.Where(x => x.Value == message.Channel.Id).Select(x => x.Value).FirstOrDefault();

            if (channel == message.Channel.Id && message.Content.First() != '.')
            {
                var flag = !message.Content.All(x => LetterController.Letters.Contains(x));

                // Mesaj geçersiz karakter içeriyor mu.
                if (flag)
                {
                    var embed = new EmbedBuilder()
                    {
                        Description = $"{message.Author.Mention}, mesajınız içermemesi gereken karakterler içeriyor.",
                        Color = new Color(255, 0, 0)
                    };

                    SelfDestructMessage.Send(message.Channel, embed: embed);
                    await message.DeleteAsync();

                    return;
                }

                // Son 40 mesajı getir
                var messages = await message.Channel.GetMessagesAsync(message, Direction.Before, 40).FlattenAsync();
                IMessage lastMessage = null;

                // Son 40 mesajı kontrol edip en son onaylanmış mesajı çek.
                foreach (var msg in messages)
                {
                    if (!msg.Author.IsBot && msg.Embeds.Count == 0 && msg.Content.First() != '.' && msg.Content.Last() != 'ğ')
                    {
                        lastMessage = msg;
                        break;
                    }
                }

                // Son kelimeyi yazan kişiyle yeni kelimeyi yazan kişi aynı mı.
                if (lastMessage.Author.Id == message.Author.Id)
                {
                    var embed = new EmbedBuilder()
                    {
                        Description = $"{message.Author.Mention}, aynı kişi üst üste yazamaz.",
                        Color = new Color(255, 0, 0)
                    };

                    SelfDestructMessage.Send(message.Channel, embed: embed);
                    await message.DeleteAsync();

                    return;
                }

                // Gelen son kelimenin son harfi ve yeni gelen kelimenin baş harfi birbirine eşit mi.
                if (lastMessage.Content.Last() != message.Content.First())
                {
                    var embed = new EmbedBuilder()
                    {
                        Description = $"{message.Author.Mention}, kelimeniz `{lastMessage.Content.Last()}` harfi ile başlamıyor.",
                        Color = new Color(255, 0, 0)
                    };

                    SelfDestructMessage.Send(message.Channel, embed: embed);
                    await message.DeleteAsync();

                    return;
                }

                // Aynı kelime son 40 kelime içerisinde kullanılmışsa yazdırtmıyor.
                if (messages.Where(x => x.Content == message.Content).FirstOrDefault() != null)
                {
                    var embed = new EmbedBuilder()
                    {
                        Description = $"`{message.Content}` zaten yakın zamanda kullanılmış.",
                        Color = new Color(255, 0, 0)
                    };

                    SelfDestructMessage.Send(message.Channel, embed: embed);
                    await message.DeleteAsync();

                    return;
                }

                // TDK APIsi yardımıyla kelime kontrol ediliyor.
                if (!await TDK.CheckWord(message.Content))
                {
                    var embed = new EmbedBuilder()
                    {
                        Description = $"{message.Content} TDK sözcüğünde bulunmuyor.",
                        Color = new Color(255, 0, 0)
                    };

                    SelfDestructMessage.Send(message.Channel, embed: embed);
                    await message.DeleteAsync();

                    return;
                }

                await message.AddReactionAsync(new Emoji("✔️"));

                int points = 1;
                User u = User.Get(message.Author.Id).FirstOrDefault();
                u.Points += points;
                u.Save();

                var serverId = Cache.WordChannel.Where(x => x.Value == message.Channel.Id).Select(x => x.Key).FirstOrDefault();
                var server = Server.Get(serverId).FirstOrDefault();
                server.RemainingToWin -= 1;
                server.Save();

                // Oyun sonu kontrolü
                if (message.Content.Last() == 'ğ')
                {
                    // Eğer son 30 kelime içerisinde oyun sonu olmuşsa oyun sonu yaptırtma.
                    if (server.RemainingToWin > 0)
                    {
                        var earlyWarningEmbed = new EmbedBuilder()
                        {
                            Description = $"{message.Content} kelimesi oyunun erken bitmesine sebep olyuor. `{server.RemainingToWin}` kelime sonra tekrar kullanılabilir olacak.",
                            Color = new Color(255, 0, 0)
                        };

                        SelfDestructMessage.Send(message.Channel, embed: earlyWarningEmbed);
                        await message.DeleteAsync();
                        return;
                    }

                    points = new Random().Next(40, 51);

                    var embed = new EmbedBuilder()
                    {
                        Title = "❔ Oyun Kesintisi!",
                        Description = $"Son kelimeden sonra yazılacak olası herhangi bir kelime kalmadı. Bu yüzden başlangıç harfi değiştirildi ve kelimenin sahibi bonus puan kazandı!",
                        Color = new Color(0, 255, 0),
                        Fields =
                        {
                            new EmbedFieldBuilder()
                            {
                                IsInline = true,
                                Name = "Başlangıç harfi",
                                Value = $"`{lastMessage.Content.Last()}`"
                            },
                            new EmbedFieldBuilder()
                            {
                                IsInline = true,
                                Name = "Kazandığı puan",
                                Value = points
                            },
                            new EmbedFieldBuilder()
                            {
                                IsInline = true,
                                Name = "Puan Sahibi",
                                Value = message.Author.Mention
                            },
                        }
                    };

                    await message.Channel.SendMessageAsync(embed: embed.Build());

                    u.Points += points;
                    u.Save();
                    server.RemainingToWin = 30;
                    server.Save();
                }
            }
        }
    }
}
