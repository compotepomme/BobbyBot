using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Testing
{
    class Program
    {
        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            Console.Out.Write("Test\n\n");




            /*
            using (StreamReader sr = new StreamReader("testjson_read.txt"))
            {
                string line = sr.ReadToEnd();
                Console.Out.WriteLine(line);
                */
            /*Product product2 = new Product()
            {
                Name = "Apple",
                ExpiryDate = new DateTime(2008, 12, 28),
                Price = 3,
                Sizes = new string[] { "Small", "Medium", "Large" }
            };

            string output = JsonConvert.SerializeObject(product2);
            Console.Out.Write("Serialize" + output + "\n");
            Product deserializedProduct = JsonConvert.DeserializeObject<Product>(output);
            Console.Out.Write("DeSerialize" + deserializedProduct.Name + "\n");
            */
            /*
            Product deserializedProduct2 = JsonConvert.DeserializeObject<Product>(line);
            Console.Out.WriteLine(deserializedProduct2.Name);
            Console.Out.WriteLine(deserializedProduct2.ExpiryDate);
            Console.Out.WriteLine(deserializedProduct2.Price);
            foreach (string s in deserializedProduct2.Sizes)
            {
                Console.Out.WriteLine(s);
            }
        }*/
            /*
            Product product = new Product()
            {
                Name = "Apple",
                ExpiryDate = new DateTime(2008, 12, 28),
                Price = 5,
                Sizes = new string[] { "Small", "Medium", "Large" }
            };
            product.ExpiryDate = new DateTime(2008, 12, 28);

            Console.Out.Write(product.ExpiryDate + "\n");

            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;

            using(StreamWriter sw = new StreamWriter(@"testjson.txt"))
            using(JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, product);
                // {"ExpiryDate":new Date(1230375600000),"Price":0}
            }

            Console.Out.Write(product.ExpiryDate);
            
                //-----------------
                Product product2 = new Product()
            {
                Name = "Apple",
                ExpiryDate = new DateTime(2008, 12, 28),
                Price = 5,
                Sizes = new string[] { "Small", "Medium", "Large" }
            };

            string output = JsonConvert.SerializeObject(product2);
            //{
            //  "Name": "Apple",
            //  "ExpiryDate": "2008-12-28T00:00:00",
            //  "Price": 5,
            //  "Sizes": [
            //    "Small",
            //    "Medium",
            //    "Large"
            //  ]
            //}
            Console.Out.Write("Serialize" + output + "\n");
            Product deserializedProduct = JsonConvert.DeserializeObject<Product>(output);
            Console.Out.Write("DeSerialize" + deserializedProduct + "\n");
            */

            Reply rep = new Reply()
            {
                command = new Dictionary<string, string>()
            };

            rep.command = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText("JSONDico.txt"));
            rep.SetCommandItem("!ping", "!pong");
            rep.SetCommandItem("WESH", "MAGUEULE");

            string input;
            string[] learn;
            do
            {
                input = Console.ReadLine();
                if (input.StartsWith("!learn "))
                {
                    learn = input.Split('"');
                    foreach (string l in learn)
                    {
                        Console.Out.WriteLine(l);
                    }
                    rep.SetCommandItem(learn[1], learn[3]);
                }
                else
                {
                    Console.Out.WriteLine(rep.GetReply(input));
                }
            }
            while (!input.Equals("STOP"));

            File.WriteAllText("JSONDico.txt", JsonConvert.SerializeObject(rep.command));

            await Task.Delay(-1);
        }
    }

    internal class Product
    {
        private DateTime expiryDate;
        private string name;
        private int price;
        private string[] sizes;

        public DateTime ExpiryDate { get => expiryDate; set => expiryDate = value; }
        public string[] Sizes { get => sizes; set => sizes = value; }
        public int Price { get => price; set => price = value; }
        public string Name { get => name; set => name = value; }
    }
    
    internal class Reply
    {
        private Dictionary<string, string> command;

        //private Dictionary<string, string> Command { get => command; set => command = value; }

        public string SetCommandItem (string com, string item)
        {
            string c = com.ToLower();
            string ret;
            if(!command.ContainsKey(c))
            {
                command.Add(c, item);
                ret = "Command : " + com + " added with value : " + item;
            }
            else
            {
                ret = "Command : " + com + " already registered.";
            }
            return ret;
        }
        
        public string GetReply (string com)
        {
            string c = com.ToLower();
            string ret;
            if (command.ContainsKey(c))
            {
                ret = command[c];
            }
            else
            {
                ret = "ERROR COMMAND UNKNOWN";
            }
            return ret;
        }
    }
}
