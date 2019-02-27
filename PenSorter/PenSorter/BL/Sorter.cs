using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PenSorter.BL
{
    //This can be done with BackgroundWorkers
    public class Sorter
    {
        //Key of this dict is penColor, value is number of pens of this color
        public Dictionary<int,int> PenColorsBuckets { get; } = new Dictionary<int, int>();
        private object _locker = new object();

        public bool IsBusy { get; private set; }

        //TODO this method cannot be called twice?
        public async Task<List<PenPack>> Sort(PenPallet penPalletInfo, int penPackSize)
        {
            lock (_locker)
            {
                IsBusy = true;
            }

            await Task.Delay(1);
            var result = new List<PenPack>();
            foreach (var pen in penPalletInfo.PensColorCodes)
            {
                if (!PenColorsBuckets.ContainsKey(pen))
                    PenColorsBuckets[pen] = 0;
                PenColorsBuckets[pen]++;
                //pack is ready, add it to result
                if (PenColorsBuckets[pen] == penPackSize)
                {
                    result.Add(new PenPack()
                    {
                        PackSize = penPackSize,
                        PenColor = pen
                    });
                    PenColorsBuckets[pen] = 0;
                }
            }
            lock (_locker)
            {
                IsBusy = false;
            }
            return await Task.FromResult(result);
        }
    }
}
