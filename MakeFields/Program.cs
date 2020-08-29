using System;
using System.IO;
using Treasure.GameLogic;
using Treasure.GameLogic.State.Field;

namespace MakeFields
{
    class Program
    {
        static void Main(string[] args)
        {
            var @params = new GameFieldParams(10, 10, 1, 3, 6, 3, 0.1, false);
            var seed = 42;
            var rnd = new Random(seed);

            var directory = "fields";

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            
            for (var i = 0; i < 100; i++)
            {
                var field = new GameFieldGenerator(@params, rnd).Generate();
                var filename = $"{directory}/field_{i + 1:000}.txt";
                
                File.WriteAllText(filename, field.Stringify());
            }
        }
    }
}