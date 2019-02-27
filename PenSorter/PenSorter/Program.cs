using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using PenSorter.BL;
using PenSorter.Core;
using PenSorter.Generator;

namespace PenSorter
{
    class Program
    {
        private static FileGenerator _generator= new FileGenerator();
        private static ILogger _logger;
        static void Main(string[] args)
        {
#if DEBUG
            _logger = new CompositeLogger(new List<ILogger>(){new DebugLogger(), new ConsoleLogger()});
#else
            _logger = new CompositeLogger(new List<ILogger>(){new NLogger(), new ConsoleLogger()});
#endif
            var helpText = "Write action to exec. Possible actions: \r\n" +
                         "gen - generate new file with pens data; \r\n" +
                         "sort - sort last file (by date modified) \r\n" +
                         "exit - exit app";
            _logger.Info(helpText);
            var needExit = false;
            do
            {
                var action = Console.ReadLine();

                switch (action?.ToLowerInvariant())
                {
                    case "gen":
                        _logger.Info("Write down number if files to generate");
                        var readLine = Console.ReadLine();
                        var isParsed = int.TryParse(readLine, out var filesCount);
                        if (!isParsed)
                        {
                            _logger.Info($"Expected number of files but got:{readLine}");
                            break;
                        }
                        GenerateData(filesCount);
                        break;
                    case "sort":
                        SortData();
                        break;
                    case "help":
                        _logger.Info(helpText);
                        break;
                    case "exit":
                        needExit = true;
                        break;
                    default:
                        _logger.Info("Unknown command. For list of commands write down 'help'.To quit write 'exit'");
                        break;
                }
                _logger.Info("Command executed. Waiting for new command");
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
                _logger.Info($"Generated file: {data}");
            }
        }

        private static void SortData()
        {
            _logger.Info("Write down number of sorters");
            var readLine = Console.ReadLine();
            var isParsed = int.TryParse(readLine, out var sortersCount);
            if (!isParsed)
            {
                _logger.Info($"Expected number of sorters but got:{readLine}");
                return;
            }

            var sw = Stopwatch.StartNew();
            var pallets = _generator.GetPallets();
            _logger.Info($"Got pallets data in {sw.Elapsed.TotalMilliseconds}ms");
            sw.Restart();
            var sortingTable = new SortingTable(sortersCount, 2000, _logger);
            _logger.Info($"Found {pallets.Count} pallets");
            _logger.Info($"Total pens: {pallets.Sum(t=>t.PensColorCodes.Count)}");
            sortingTable.Sort(pallets, 4).ContinueWith(t=>
            {
                _logger.Info($"Formed {t.Result.Count} packs.");
                _logger.Info($"Total cost of packs: {t.Result.Count*10}$.");
                return Task.CompletedTask;
            }).ContinueWith(t =>
            {
                sw.Stop();
                _logger.Info($"Sorted pallets data in {sw.Elapsed.TotalMilliseconds}ms");
            }).Wait();
            _logger.Info($"Sorting ended");
        }
    }
}
