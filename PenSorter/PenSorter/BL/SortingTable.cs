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
                        CurrentCapacity -= penPallet.PensColorCodes.Count;
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
                 var bucketPallet = new PenPallet { PensColorCodes = new List<int>() };
                  foreach (var sorter in Sorters)
                  {
                      foreach (var kvp in sorter.PenColorsBuckets)
                      {
                         //собираем псевдо-паллету с количеством карандашей, равным остатку в корзинке сортировщика
                         bucketPallet.PensColorCodes.AddRange(Enumerable.Range(0, kvp.Value).Select(t => kvp.Key));
                      }
                      sorter.PenColorsBuckets.Clear();
                  };

                  var bucketPacks = Sorters.First().Sort(bucketPallet, penPackSize).Result;

                  var penPacks = sortTask.Result.SelectMany(t => t)
                      .Union(bucketPacks)
                      .ToList();
                  return penPacks;
              });
        }
    }
}