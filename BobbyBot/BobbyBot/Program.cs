using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
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
            if (!message.Author.IsBot)
            {
                string mess = message.Content.Trim();
                Reply rep = new Reply("../../../JSONDicoCommand.txt", "../../../JSONDicoWord.txt");
                System.Threading.Thread.Sleep(500);
                string mess_lower = (message.Content.Trim()).ToLower();

                if (mess.StartsWith("!learn "))
                {

                    Console.Out.WriteLine(message.Content);
                    string[] learn;
                    learn = mess.Split('"');
                    Console.Out.WriteLine(learn.Length);
                    if (learn.Length > 4)
                    {
                       //if (!(learn[1] == null || learn[3] == null)) {
                        string temp = rep.SetCommandItem(learn[1].ToLower(), learn[3]);
                        Console.Out.WriteLine(temp);
                        rep.SaveCommandToFile("../../../JSONDicoCommand.txt");
                    }
                    else
                    {
                        await message.Channel.SendMessageAsync("!learn need 2 arguments. \nex : !learn \"Qui est Robin ?\" \"Robin est le plus beau\"");
                    }
                }
                else if (mess.StartsWith("!word "))
                {
                    string[] word;
                    word = mess.Split('"');
                    if (word.Length > 3)
                    {
                        System.Console.WriteLine("Before Regex : " + word[1]);
                        Regex.Replace(word[1], @"[^\w\s]", "");
                        System.Console.WriteLine("After Regex : " + word[1]);
                        //!(learn[1] == null || learn[3] == null)) {
                        string temp = rep.SetWordItem(word[1].ToLower(), word[3]);
                        Console.Out.WriteLine(temp);
                        rep.SaveWordToFile("../../../JSONDicoWord.txt");
                    }
                    else
                    {
                        await message.Channel.SendMessageAsync("!word need 2 arguments. \nex : !word \"poivron\" \"Berk ! J'aime pas les poivrons !\"");
                    }
                }
                else if (mess_lower.StartsWith("bonjour") || mess_lower.StartsWith("salut") || mess_lower.StartsWith("coucou") || mess_lower.StartsWith("hello")
                        || mess_lower.StartsWith("yo") || mess_lower.StartsWith("plop"))
                {
                    await message.Channel.SendMessageAsync("Yo " + message.Author.Username + " !");
                }
                else
                {
                    string reply = rep.GetCommandReply(mess_lower);
                    if (!reply.Equals("ERROR COMMAND UNKNOWN"))
                    {
                        await message.Channel.SendMessageAsync(reply);
                    }
                    else if (!rep.GetWordReply(mess_lower).Equals("ERROR COMMAND UNKNOWN"))
                    {
                        await message.Channel.SendMessageAsync(rep.GetWordReply(mess_lower));
                    }
                }
            }
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        internal class Reply
        {
            private Dictionary<string, string> command;
            private Dictionary<string, string> word;
            //public Dictionary<string, string> Command { get => command; set => command = value; }

            public Reply()
            {
                command = new Dictionary<string, string>();
                word = new Dictionary<string, string>();
            } 

            public Reply(string filepathCommand, string filepathWord)
            {
                command = new Dictionary<string, string>();
                command = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(filepathCommand));

                word = new Dictionary<string, string>();
                word = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(filepathWord));
            }

            // Command Methods

            public void LoadCommandFromFile(string filepath)
            {
                command = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(filepath));
            }

            public void SaveCommandToFile (string filepath)
            {
                File.WriteAllText(filepath, JsonConvert.SerializeObject(command));
            }
            
            public string SetCommandItem(string com, string item)
            {
                string ret;
                if (!command.ContainsKey(com))
                {
                    command.Add(com, item);
                    ret = "Command : \"" + com + "\" added with value : \"" + item + "\"";
                }
                else
                {
                    ret = "Command : \"" + com + "\" already registered.";
                }
                //Console.Out.WriteLine(ret);
                return ret;
            }

            public string GetCommandReply(string com)
            {
                string ret;
                if (command.ContainsKey(com))
                {
                    ret = command[com];
                }
                else
                {
                    ret = "ERROR COMMAND UNKNOWN";
                }
                //Console.Out.WriteLine(ret);
                return ret;
            }

            // Word Methods

            public void LoadWordFromFile(string filepath)
            {
                word = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(filepath));
            }

            public void SaveWordToFile(string filepath)
            {
                File.WriteAllText(filepath, JsonConvert.SerializeObject(word));
            }


            public string SetWordItem(string wor, string item)
            {
                string ret;
                if (!word.ContainsKey(wor))
                {
                    word.Add(wor, item);
                    ret = "Word : \"" + wor + "\" added with value : \"" + item + "\"";
                }
                else
                {
                    ret = "Word : \"" + wor + "\" already registered.";
                }
                //Console.Out.WriteLine(ret);
                return ret;
            }

            public string GetWordReply(string wor)
            {
                string ret = "ERROR COMMAND UNKNOWN";
                string[] wo;
                wo = wor.Split(' ');
                foreach (string w in wo)
                {
                    if (word.ContainsKey(w))
                    {
                        ret = "";
                        ret = word[w];
                        break;
                    }
                }
                /*
                if (word.ContainsKey(wor))
                {
                    ret = word[wor];
                }
                else
                {
                    ret = "ERROR COMMAND UNKNOWN";
                }
                //Console.Out.WriteLine(ret);*/
                return ret;
            }
        }
    }
}
