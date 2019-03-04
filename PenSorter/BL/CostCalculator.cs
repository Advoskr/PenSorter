using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace PenSorter.BL
{
    //make singletone maybe?
    public class CostCalculator
    {
        private OpenratesData CachedData { get; set; }

        public CostCalculator()
        {
            
        }

        //
        public double CalculateCost(int cost, string inputCashCode, string outputCashCode)
        {
            if (CachedData==null || CachedData.Date != DateTime.Now.Date)
            {
                //refresh cache
                HttpClient http = new HttpClient();
                var jsonData = http.GetAsync("https://api.openrates.io/latest").Result.Content.ReadAsStringAsync()
                    .Result;
                var data = JsonConvert.DeserializeObject<OpenratesData>(jsonData);
                CachedData = data;
            }

            if (!CachedData.Rates.ContainsKey(inputCashCode.ToUpper()))
            {
                //TODO Can be rewritten with operationResult pattern
                throw new Exception($"Cash code {inputCashCode} not found");
            }
            if (!CachedData.Rates.ContainsKey(outputCashCode.ToUpper()))
            {
                //TODO Can be rewritten with operationResult pattern
                throw new Exception($"Cash code {outputCashCode} not found");
            }
            //inputCashCode->openratesBase -> outputCashCode
            var inputCashRate = CachedData.Rates[inputCashCode.ToUpper()];
            var costInBase = 1 / inputCashRate * cost;
            var result = CachedData.Rates[outputCashCode.ToUpper()] * costInBase;
            return result;
        }
    }

    public class OpenratesData
    {
        //code of base cash
        public string Base { get; set; }

        public DateTime Date { get; set; }

        public Dictionary<string, double> Rates { get; set; }
    }
}
