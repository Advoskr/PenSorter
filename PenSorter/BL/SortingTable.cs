using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PenSorter.Core;

namespace PenSorter.BL
{
    public class SortingTable
    {
        private static int _instanceCounter;

        public int InstanceName { get; }

        public SortingTable(int sortersCount, int maxCapacity, ILogger logger)
        {
            Sorters = Enumerable.Range(0,sortersCount).Select(t=>new Sorter()).ToList();
            Logger = logger;
            MaxCapacity = maxCapacity;
            InstanceName = ++_instanceCounter;
        }

        public List<Sorter> Sorters { get; set; }

        public ILogger Logger { get; }
        public int MaxCapacity { get; set; }

        public int CurrentCapacity { get; set; }

        private readonly object _capacityLocker = new object();

        public async Task<List<PenPack>> Sort(List<PenPallet> penPalletInfo, int penPackSize)
        {
            var tasks = new List<Task<List<PenPack>>>();
            //моя идея такая. Мы будем кормить сортировщикам максимально большие паллеты. Поиск будет простым проходом, т.е. не очень эффективный.
            var listCopy = penPalletInfo.Select(t => t).OrderByDescending(t=>t.PensColorCodes.Count).ToList();
            var listCopyLocker = new object();

            var totalPens = 0;
            while (listCopy.Count>0)
            {
                PenPallet penPallet;
                lock (listCopyLocker)
                {
                    penPallet = listCopy.FirstOrDefault(t => t.PensColorCodes.Count <= MaxCapacity - CurrentCapacity);
                    listCopy.Remove(penPallet);
                }
                
                if (penPallet == null)
                {
                    await Task.Delay(5);
                    continue;
                }
                while (Sorters.All(t => t.IsBusy))
                {
                    //wait for sorter
                    await Task.Delay(5);
                }

                var sorter = Sorters.First(t => !t.IsBusy);
                lock (_capacityLocker)
                {
                    totalPens += penPallet.PensColorCodes.Count;
                    CurrentCapacity += penPallet.PensColorCodes.Count;
                }

                if (CurrentCapacity > MaxCapacity)
                {
                    Logger.Warn(
                        $"Table capacity exceeded limit ({MaxCapacity}). Capacity: {CurrentCapacity}");
                }

                var sortTask = sorter.Sort(penPallet, penPackSize).ContinueWith(t =>
                {
                    lock (_capacityLocker)
                    {
                        var count = penPallet.PensColorCodes.Count;
                        CurrentCapacity -= count;
                    }
                    return t.Result;
                });
                tasks.Add(sortTask);
            }

            Logger.Info($"Total pens sorted on sorting table #{InstanceName}: {totalPens}");
            return await Task.WhenAll(tasks).ContinueWith(sortTask =>
            {
                //собираем корзинки сортировщиков после того, как отсортировали все карандаши и дособираем из них пачки.
                //Здесь, для "честности" алгоритма правлиьно было бы отдать карандаши сортировщику.
                var bucketLeftovers = new Dictionary<int,int>();
                foreach (var sorter in Sorters)
                {
                    foreach (var kvp in sorter.PenColorsBuckets)
                    {
                        if (!bucketLeftovers.ContainsKey(kvp.Key))
                        {
                            bucketLeftovers[kvp.Key] = 0;
                        }
                        bucketLeftovers[kvp.Key] += kvp.Value;
                    }
                };
                var bucketPacks = new List<PenPack>();
                foreach (var penPair in bucketLeftovers)
                {
                    var fullPacks = penPair.Value / penPackSize;
                    bucketPacks.AddRange(Enumerable.Range(0, fullPacks).Select(t => new PenPack()));
                }

                var penPacks = sortTask.Result.SelectMany(t => t)
                    .Union(bucketPacks)
                    .ToList();
                return penPacks;
            });
        }
    }
}