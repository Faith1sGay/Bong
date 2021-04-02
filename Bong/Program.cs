using Bong.Services.Config;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Bong
{

    class Program
    {
        public static Config Config = null;
        public DiscordSocketClient client;
        public CommandService commands;
        public IServiceProvider services;
        static void Main(string[] args)
        {
            new Program().MainAsync().GetAwaiter().GetResult();

        }
        internal async Task MainAsync()
        {

            Config = JsonSerializer.Deserialize<Config>(File.ReadAllText("../../../Bong/Config.json"));
            client = new DiscordSocketClient();
            await client.LoginAsync(TokenType.Bot, Config.Token);
            await client.StartAsync();
            client.Ready += () =>
            {
                client.SetGameAsync("🕧");
                Console.WriteLine("Ready!");
                return Task.CompletedTask;
            };
            commands = new CommandService();
            services = new ServiceCollection()
                .AddSingleton(client)
                .AddSingleton(commands)
                .AddSingleton<CommandHandler>()
                .BuildServiceProvider();
            await services.GetRequiredService<CommandHandler>().InstallCommandsAsync();
            await Task.Delay(Timeout.Infinite);
        }
        sealed class CommandHandler
        {
            readonly DiscordSocketClient client;
            readonly CommandService commands;

            public CommandHandler(DiscordSocketClient _client, CommandService _commands)
            {
                commands = _commands;
                client = _client;
            }

            public async Task InstallCommandsAsync()
            {
                client.MessageReceived += HandleCommandAsync;
                await commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), services: null);
            }

            private async Task HandleCommandAsync(SocketMessage messageParam)
            {
                var message = messageParam as SocketUserMessage;
                if (message == null) return;
                int argPos = 0;
                if (!(message.HasStringPrefix(Config.Prefix, ref argPos) || message.HasMentionPrefix(client.CurrentUser, ref argPos))) return;
                if (message.Author.IsBot) return;
                var context = new SocketCommandContext(client, message);
                var result = await commands.ExecuteAsync(
                    context: context,
                    argPos: argPos,
                    services: null);
            }
        }
    }
}