using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Websocket.Samples
{
    class CpuUsageServer:BaseServer
    {
        private PerformanceCounter[] pcs;

        class Processor
        {
            public string Name { get; set; }
            public float Usage { get; set; }
        }

        private System.Threading.Timer mTimer;

        public override void Opened(Beetle.Express.IServer server)
        {
            base.Opened(server);
            int coreCount = 0;
            foreach (var item in new System.Management.ManagementObjectSearcher("Select * from Win32_Processor").Get())
            {
                coreCount += int.Parse(item["NumberOfCores"].ToString());
            }

            PerformanceCounter[] pcs = new PerformanceCounter[coreCount];

            for (int i = 0; i < coreCount; i++)
            {
                pcs[i] = new PerformanceCounter("Processor", "% Processor Time", i.ToString());
            }
            mTimer = new System.Threading.Timer(o => {

                try
                {
                    List<Processor> items = new List<Processor>();
                    for (int i = 0; i < coreCount; i++)
                    {
                        PerformanceCounter pc = pcs[i];
                        Processor item = new Processor();
                        item.Name = pc.InstanceName;
                        item.Usage = pc.NextValue();
                        items.Add(item);
                        //Console.WriteLine("{0}:{1}", pc.InstanceName, item.Usage);
                    }
                   // Console.WriteLine("");
                    SendAll(null, "stat", items);
                   // Console.WriteLine("get cpu usage success!");
                }
                catch (Exception e_)
                {
                    Console.WriteLine("get cpu usage error {0} ", e_.Message);
                }
            }, null, 1000, 1000);
        }
        
    }
}
