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
        public async Task RunTestCommand()
        {
            await Context.Channel.SendMessageAsync("ara ara, I am working now");
            //test
        }
    }
}
