using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using PenSorter.BL;
using PenSorter.Generator;

namespace PenSorter
{
    class Program
    {
        private static FileGenerator _generator= new FileGenerator();

        static void Main(string[] args)
        {
            var helpText = "Write action to exec. Possible actions: \r\n" +
                         "gen - generate new file with pens data; \r\n" +
                         "sort - sort last file (by date modified) \r\n" +
                         "exit - exit app";
            Console.WriteLine(helpText);
            var needExit = false;
            do
            {
                var action = Console.ReadLine();

                switch (action?.ToLowerInvariant())
                {
                    case "gen":
                        Console.WriteLine("Write down number if files to generate");
                        var readLine = Console.ReadLine();
                        var isParsed = int.TryParse(readLine, out var filesCount);
                        if (!isParsed)
                        {
                            Console.WriteLine($"Expected number of files but got:{readLine}");
                            break;
                        }
                        GenerateData(filesCount);
                        break;
                    case "sort":
                        SortData();
                        break;
                    case "help":
                        Console.WriteLine(helpText);
                        break;
                    case "exit":
                        needExit = true;
                        break;
                    default:
                        Console.WriteLine("Unknown command. For list of commands write down 'help'.To quit write 'exit'");
                        break;
                }
                Console.WriteLine("Command executed. Waiting for new command");
            } while ((!needExit));
        }

        private static void GenerateData(int numberOfFiles)
        {
            var rnd = new Random();
            //TODO can be static object
            foreach (var _ in Enumerable.Range(0,numberOfFiles))
            {
                var size = rnd.Next(1, 500);
                var data = _generator.GenerateDataFile(size);
                Console.WriteLine($"Generated file: {data}");
            }
        }

        private static void SortData()
        {
            Console.WriteLine("Write down number of sorters");
            var readLine = Console.ReadLine();
            var isParsed = int.TryParse(readLine, out var sortersCount);
            if (!isParsed)
            {
                Console.WriteLine($"Expected number of sorters but got:{readLine}");
                return;
            }

            var sw = Stopwatch.StartNew();
            var pallets = _generator.GetPallets();
            Console.WriteLine($"Got pallets data in {sw.Elapsed.TotalMilliseconds}");
            sw.Restart();
            var sortingTable = new SortingTable(sortersCount);
            sortingTable.Sort(pallets, 4).ContinueWith(t=>
            {
                Console.WriteLine($"Formed {t.Result.Count} packs.");
                Console.WriteLine($"Total cost of packs: {t.Result.Count*10} packs.");
                return Task.CompletedTask;
            }).ContinueWith(t =>
            {
                sw.Stop();
                Console.WriteLine($"Sorted pallets data in {sw.Elapsed.TotalMilliseconds}");
            }).Wait();
            Console.WriteLine($"Found {pallets.Count} pallets");

        }
    }
}
