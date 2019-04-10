using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using Discord;
using Discord.Commands;

namespace DiscordBot.Core.Commands
{
    public class Ping : ModuleBase<SocketCommandContext/*comes from MessageReceived*/>
    {
        //specifying a command in discord
        [Command("Ping"), Alias("Check", "ping"), Summary("Test command")]
        public async Task ReplyPing()
        {
            await Context.Channel.SendMessageAsync("ara ara, I am working now");
        }

        [Command("baka"), Alias("Baka", "BAKA", "bAKA"), Summary("Baka command")]
        public async Task ReplyBaka()
        {
            await Context.Channel.SendMessageAsync($"No, {Context.User.Mention} is");
        }

        [Command("stuck"), Alias("sensei", "Stuck", "Sensei", "Mentor", "mentor")]
        public async Task MentionSensei()
        {
            await Context.Channel.SendMessageAsync($"{Context.Guild.GetRole(561965462918004737).Mention}");
        }
    }
}
