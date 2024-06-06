using System.Collections.ObjectModel;
using System.Management;
using System.Management.Automation;
using LibreHardwareMonitor.Hardware;

namespace JiaoLongWMI.Models
{
    public class SystemUsage
    {
        // Token: 0x060000F8 RID: 248 RVA: 0x00008554 File Offset: 0x00006754
        public int getCurrentCpuUsage(out ulong cpuusage)
        {
            cpuusage = 0UL;
            foreach (ManagementBaseObject managementBaseObject in SystemUsage.searchercpu.Get())
            {
                ManagementObject managementObject = (ManagementObject)managementBaseObject;
                object usage = managementObject["PercentProcessorTime"];
                if (managementObject["Name"].Equals("_Total"))
                {
                    cpuusage = (ulong)usage;
                    return 0;
                }
            }

            return 0;
        }

        public int getCpuCoreCount()
        {
            int corecount = 0;
            foreach (ManagementBaseObject managementBaseObject in SystemUsage.searchercpucore.Get())
            {
                corecount = int.Parse(((ManagementObject)managementBaseObject)["NumberOfCores"].ToString());
            }

            Console.WriteLine("Number Of Cores: {0}", corecount);
            return corecount;
        }

        public int getCurrentRamUsage(out double ramusage)
        {
            ramusage = 0.0;
            foreach (ManagementBaseObject managementBaseObject in SystemUsage.searcher.Get())
            {
                ulong FreePhysicalMemory = Convert.ToUInt64(managementBaseObject["FreePhysicalMemory"].ToString());
                ulong TotalVisibleMemorySize =
                    Convert.ToUInt64(managementBaseObject["TotalVisibleMemorySize"].ToString());
                ramusage = (1.0 - FreePhysicalMemory * 1.0 / TotalVisibleMemorySize) * 100.0;
            }

            return 0;
        }

        public int getCurrentDiskusage(out double freesize, out double totalsize)
        {
            string script =
                "Get-CimInstance Win32_Diskdrive -PipelineVariable disk |Where-Object {$_.DeviceID -eq '\\\\.\\PHYSICALDRIVE0'}|% { Get-CimAssociatedInstance $_ -ResultClass Win32_DiskPartition -pv partition}|% { Get-CimAssociatedInstance $_ -ResultClassName Win32_LogicalDisk } |Select-Object @{n='Disk';e={$disk.deviceid}},VolumeName,Size,FreeSpace";
            Collection<PSObject> collection = PowerShell.Create().AddScript(script).Invoke();
            freesize = 0.0;
            totalsize = 0.0;
            foreach (PSObject psobject in collection)
            {
                foreach (PSPropertyInfo keyProperties in psobject.Properties)
                {
                    if (keyProperties.Name == "FreeSpace")
                    {
                        freesize += (ulong)keyProperties.Value;
                    }

                    if (keyProperties.Name == "Size")
                    {
                        totalsize += (ulong)keyProperties.Value;
                    }
                }
            }

            return 0;
        }

        private static bool isRuning = false;
        private static Computer computer = new Computer
        {
            IsCpuEnabled = true,
            IsGpuEnabled = true,
            IsMemoryEnabled = false,
            IsMotherboardEnabled = false,
            IsControllerEnabled = false,
            IsNetworkEnabled = false,
            IsStorageEnabled = false
        };
        public static int GetNvidiaGpuUsage(out double usage, out int gputemp, out double gpufreq, out double rate,
            out int speed, out int cputemp)
        {
            usage = 0.0;
            gputemp = 0;
            rate = 0.0;
            speed = 0;
            gpufreq = 0.0;
            cputemp = 0;
            if (!isRuning)
            {
                isRuning = true; 

                computer.Open();
                computer.Accept(new UpdateVisitor());
                foreach (IHardware hardware in computer.Hardware)
                {
                    foreach (ISensor sensor in hardware.Sensors)
                    {
                        var identifier = sensor.Identifier.ToString();
                        var sensorValue = sensor.Value.GetValueOrDefault();

                        if (sensor.Name == "GPU Core")
                        {
                            if (identifier == "/gpu-nvidia/0/load/0")
                            {
                                usage = sensorValue;
                            }
                            else if (identifier == "/gpu-nvidia/0/temperature/0")
                            {
                                gputemp = (int)sensorValue;
                            }
                            else if (identifier == "/gpu-nvidia/0/clock/0")
                            {
                                gpufreq = sensorValue / 1024.0;
                            }
                        }
                        else if (sensor.Name == "GPU Fan")
                        {
                            if (identifier == "/gpu-nvidia/0/fan/1")
                            {
                                speed = (int)sensorValue;
                            }
                            else if (identifier == "/gpu-nvidia/0/control/1")
                            {
                                rate = sensorValue;
                            }
                        }
                        else if (sensor.Name == "Core (Tctl/Tdie)" && identifier == "/amdcpu/0/temperature/2")
                        {
                            cputemp = (int)sensorValue;
                        }
                    }
                }
                computer.Close();
                isRuning = false;
            }
            return 0;
        }

        public int getGpuTotalUsage(out double usage)
        {
            usage = 0.0;
            string script =
                "(((Get-Counter '\\GPU Engine(*engtype_3D)\\Utilization Percentage').CounterSamples | where CookedValue).CookedValue | measure -sum).sum";
            Collection<PSObject> results = PowerShell.Create().AddScript(script).Invoke();
            usage = (double)results[0].BaseObject;
            return 0;
        }

        public static int getFanusage(out double rate, out int speed)
        {
            rate = 0.0;
            speed = 0;
            Computer computer = new Computer
            {
                IsCpuEnabled = false,
                IsGpuEnabled = true,
                IsMemoryEnabled = false,
                IsMotherboardEnabled = false,
                IsControllerEnabled = false,
                IsNetworkEnabled = false,
                IsStorageEnabled = false
            };
            computer.Open();
            computer.Accept(new UpdateVisitor());
            foreach (IHardware hardware in computer.Hardware)
            {
                foreach (ISensor sensor in hardware.Sensors)
                {
                    Identifier identifier = sensor.Identifier;
                    if (sensor.Name == "GPU Fan" && identifier.ToString() == "/gpu-nvidia/0/fan/1")
                    {
                        speed = (int)sensor.Value.Value;
                    }

                    if (sensor.Name == "GPU Fan" && identifier.ToString() == "/gpu-nvidia/0/control/1")
                    {
                        rate = (double)sensor.Value.Value;
                    }
                }
            }

            computer.Close();
            return 0;
        }

        private static ManagementObjectSearcher searchercpu =
            new ManagementObjectSearcher("select * from Win32_PerfFormattedData_PerfOS_Processor");

        private static ObjectQuery winQuery = new ObjectQuery("SELECT * FROM CIM_OperatingSystem");
        private static ManagementObjectSearcher searcher = new ManagementObjectSearcher(SystemUsage.winQuery);

        private static ManagementObjectSearcher searchercpucore =
            new ManagementObjectSearcher("Select * from Win32_Processor");
    }
}