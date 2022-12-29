using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreConsoleApplication.Benchmark
{
    [MemoryDiagnoser]
    //[MemoryDiagnoser(false)]
    public class BechmarkApiDemo
    {
        int NumberOfItems = 100000;
        
        //[GlobalSetup]
        //public void GlobalSetup()
        //{
        //}


        [Benchmark]
        public string ConcatStringsUsingStringBuilder()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < NumberOfItems; i++)
            {
                sb.Append("Hello World!" + i);
            }
            return sb.ToString();
        }

        [Benchmark]
        public string ConcatStringsUsingGenericList()
        {
            var list = new List<string>(NumberOfItems);
            for (int i = 0; i < NumberOfItems; i++)
            {
                list.Add("Hello World!" + i);
            }
            return list.ToString();
        }
    }
}
