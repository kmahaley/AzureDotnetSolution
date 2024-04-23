using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreConsoleApplication.Dotnetutilities
{
    public class TaskBasedUtilities
    {

        public static async Task HandleTaskAsync()
        {
            string hbResult = string.Empty;
            bool isDataDisk = false;
            int i = 0;
            while (true)
            {
                await Console.Out.WriteLineAsync("--------");
                var list = new List<Task>();
                Task<string> hbTask = Task.Run(() => GetHeartBeat(i));
                Task dataDiskTask = Task.CompletedTask;
                list.Add(hbTask);
                if (!isDataDisk)
                {
                    dataDiskTask = GetDataDisk(i);
                    list.Add(dataDiskTask);
                }

                try
                {
                    var totalTask = Task.WhenAll(list);
                    await totalTask;
                    if (dataDiskTask.Status == TaskStatus.RanToCompletion)
                    {
                        isDataDisk = true;
                    }
                    //if (t.Status == TaskStatus.RanToCompletion)
                    if (totalTask.Status == TaskStatus.RanToCompletion)
                    {
                        hbResult = hbTask.Result;
                        await Console.Out.WriteLineAsync("completed all tasks");
                        await Console.Out.WriteLineAsync($"Values ====> {hbResult}");
                        break;
                    }
                }
                catch (Exception ex)
                {
                    if (dataDiskTask.Status == TaskStatus.RanToCompletion)
                    {
                        isDataDisk = true;
                    }

                    await Console.Out.WriteLineAsync($"{ex.Message} ===> {nameof(hbTask)}={hbTask.Status}, {nameof(dataDiskTask)}={dataDiskTask.Status}");
                    await Console.Out.WriteLineAsync();
                }

                if (hbResult.Equals("apple"))
                {
                    await Console.Out.WriteLineAsync("got it apple");
                }

                i++;
            }
        }

        public static async Task<string> GetHeartBeat(int i)
        {
            Console.Out.WriteLine("GetHeartBeat= called synchronously");
            await Task.Delay(2000);
            if (i != 3)
            {
                throw new Exception("GetHeartBeat => not working ex thrown!!!!!!!");
            }
            await Console.Out.WriteLineAsync("GetHeartBeat= finally return");
            return "apple";
        }

        public static async Task<int> GetDataDisk(int i)
        {
            await Task.Delay(1000);
            if (i == 0)
            {
                throw new Exception("GetDataDisk => not working ex thrown!!!!!!!");
            }
            await Console.Out.WriteLineAsync("GetDataDisk= should be called just once. completed task");
            return 1;
        }
    }
}
