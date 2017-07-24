﻿using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace BobbyBot
{
    public class Program
    {
        private DiscordSocketClient _client;

        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            _client = new DiscordSocketClient();

            _client.Log += Log;
            _client.MessageReceived += MessageReceived;

            string token = "MzM4OTU2MTgwNTYwODA1ODg4.DFesaw.IbkfrUMw9s__akiZI7yRovMzYUY"; // Remember to keep this private!
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private async Task MessageReceived(SocketMessage message)
        {
            switch (message.Content) {
                case "!ping":
                    await message.Channel.SendMessageAsync("Pong !");
                    break;
                case "!pong":
                    await message.Channel.SendMessageAsync("Pung !");
                    break;
                case "!pung":
                    await message.Channel.SendMessageAsync("Ta gueule.");
                    break;
                case "!Kevin":
                    await message.Channel.SendMessageAsync("Rageux !");
                    break;
                case "!Close":
                    await message.Channel.SendMessageAsync("Sale Tank !");
                    break;
                case "!Norfl":
                    await message.Channel.SendMessageAsync("Tuto touchpad !");
                    break;
                case "!Compote":
                    await message.Channel.SendMessageAsync("Beau Gosse !");
                    break;
                case "!Tamere":
                    await message.Channel.SendMessageAsync("Ton pere !");
                    break;
            }
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
