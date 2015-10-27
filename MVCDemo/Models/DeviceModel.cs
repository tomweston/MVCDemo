using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using HistoricalData = System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<System.DateTime, int>>;

namespace MVCDemo.Models
{
    /// <summary>
    /// this is a simple class to represent a device object
    /// details about the CPU are encapsulated into a dedicated class
    /// </summary>
    public class Device
    {
        public int      DeviceID      { get; protected set; }
        public string   DeviceName    { get; protected set; }
        public CPU      CpuModel      { get; protected set; }
        public int      MemoryGB      { get; protected set; }

        public Device(int deviceID, string deviceName, CPU cpuModel, int memoryGB) 
        {
            this.DeviceID = deviceID;
            this.DeviceName = deviceName;
            this.CpuModel = cpuModel;
            this.MemoryGB = memoryGB;
        }

        /// <summary>
        /// for testing purposes we define a method that returns 
        /// historical data for cpu % utilisation (data is randomized)
        /// </summary>
        /// <param name="from">interval start date</param>
        /// <param name="to">interval end date</param>
        /// <returns></returns>
        public HistoricalData GetCpuUsage(DateTime from, DateTime to) 
        {
            HistoricalData result = new List<KeyValuePair<DateTime, int>>();
            int seed = DateTime.Now.Second;
            Random prng = new Random(seed);

            for (int i = 0; i < (to - from).TotalSeconds; i++) 
            {
                result.Add(new KeyValuePair<DateTime, int>(from.AddSeconds(i), prng.Next(0, 100)));   
            }

            // we introduce a delay to simulate a slow data source
            Thread.Sleep(3000);

            return result;
        }
    }

    public class CPU
    {
        public string Model { get; set; }
        public int SpeedMhz { get; set; }
        public int NumCores { get; set; }
    }


    /// <summary>
    /// this is an helper class to generate a random list of devices
    /// </summary>
    public class DeviceGenerator
    {
        public static List<Device> GetDevices(int quantity)
        {
            string[] models = { "intel i7", "intel i5", "intel i3", "Xeon E5", "Xeon Phi" };
            string[] roles = { "web", "DB", "app", "dns", "mail" };
            string[] os = {"windows", "linux"};
            int[] clocks = { 2600, 2200, 1800, 1600, 2800, 3200 };
            int[] coreConf = { 1, 2, 4, 8, 16 };
            int[] memoryConf = { 4, 8, 16, 32, 64, 128 };

            List<Device> devices = new List<Device>();
            int seed = 0;
            
            for (int i = 0; i < quantity; i++) 
            {
                devices.Add(
                    new Device(i, roles[new Random(seed++).Next(0, roles.Length - 1)] + i,
                        new CPU()
                        {
                            Model = models[new Random(seed++).Next(0, models.Length - 1)],
                            NumCores = coreConf[new Random(seed++).Next(0, coreConf.Length - 1)],
                            SpeedMhz = clocks[new Random(seed++).Next(0, clocks.Length - 1)]
                        },
                    memoryConf[new Random(seed++).Next(0, memoryConf.Length - 1)]));
            }
                        
            return devices;
        }
    }
}