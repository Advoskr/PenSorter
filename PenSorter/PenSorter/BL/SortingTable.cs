using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PenSorter.BL
{
    public class SortingTable
    {
        public SortingTable(int sortersCount)
        {
            Sorters = Enumerable.Range(0,sortersCount).Select(t=>new Sorter()).ToList();
        }

        public List<Sorter> Sorters { get; set; }

        public int MaxCapacity { get; set; }

        public async Task<List<PenPack>> Sort(List<PenPallet> penPalletInfo, int penPackSize)
        {
            var tasks = new List<Task<List<PenPack>>>();
            foreach (var penPallet in penPalletInfo)
            {
                while(Sorters.All(t => t.IsBusy))
                {
                    //wait for sorter
                    await Task.Delay(10);
                }
                var sorter=Sorters.First(t => !t.IsBusy);
                var sortTask = sorter.Sort(penPallet, penPackSize);
                tasks.Add(sortTask);
            }

            return await Task.WhenAll(tasks).ContinueWith(sortTask =>
            {
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