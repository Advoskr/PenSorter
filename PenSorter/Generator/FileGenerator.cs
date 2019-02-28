using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using PenSorter.BL;

namespace PenSorter.Generator
{
    //Need better name
    public class FileGenerator
    {
        private const string _testDataFolder = "test_data";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        /// <returns>file path</returns>
        public string GenerateDataFile(int size=2000)
        {
            var rnd = new Random();
            
            var builder = new StringBuilder();
            for (int i = 0; i < size; i++)
            {
                var coloreCode = rnd.Next(1, 4);
                builder.AppendLine(coloreCode.ToString());
            }

            var data = builder.ToString();
            return WriteFile(data);
        }

        public string WriteFile(string data)
        {
            //TODO This code has some non-clear side effects
            if (!Directory.Exists(_testDataFolder))
                Directory.CreateDirectory(_testDataFolder);
            var filePath = Path.Combine(_testDataFolder, $"test{Guid.NewGuid().ToString()}.txt");
            File.WriteAllText(filePath, data);
            return filePath;
        }

        public List<PenPallet> GetPallets()
        {
            if (!Directory.Exists(_testDataFolder)||!Directory.EnumerateFiles(_testDataFolder).Any())
            {
                throw new Exception("Test directory is empty or not created");
            }
            var result = new List<PenPallet>();
            foreach (var fileInfo in new DirectoryInfo(_testDataFolder).GetFiles())
            {
                var data = File.ReadAllLines(fileInfo.FullName).Select(int.Parse).ToList();
                var pallet = new PenPallet()
                {
                    PensColorCodes = data
                };
                result.Add(pallet);
            }
            
            return result;
        }


    }
}
