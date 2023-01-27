using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using Microsoft.Win32;

namespace prak_G_13_Client.Properties
{
    public static class SystemInfo
    {
        public static string FullInfo => getOperatingSystemInfo() + " " + getProcessorInfo() + " " + getVram() + " " + machineName();
        public static string getOperatingSystemInfo()
        {
            string info = string.Empty;

            ManagementObjectSearcher mos = new ManagementObjectSearcher("select * from Win32_OperatingSystem");

            foreach (ManagementObject managementObject in mos.Get())
            {
                if (managementObject["Caption"] != null)
                {
                    info += "\nНаименование операционной системы  :  " + managementObject["Caption"].ToString();
                }
                if (managementObject["OSArchitecture"] != null)
                {
                    info += "\nАрхитектура операционной системы  :  " + managementObject["OSArchitecture"].ToString();
                }
                if (managementObject["CSDVersion"] != null)
                {
                    info = "\nOperating System Service Pack   :  " + managementObject["CSDVersion"].ToString();
                }
            }

            return info;
        }

        public static string getProcessorInfo()
        {
            string info = string.Empty;

            RegistryKey processor_name = Registry.LocalMachine.OpenSubKey(@"Hardware\Description\System\CentralProcessor\0", RegistryKeyPermissionCheck.ReadSubTree);

            if (processor_name != null)
            {
                if (processor_name.GetValue("ProcessorNameString") != null)
                {
                    info += "\n" + processor_name.GetValue("ProcessorNameString");
                }
            }

            return info;
        }

        private static string getVram()
        {
            string vram = string.Empty;

            ManagementObjectSearcher Search = new ManagementObjectSearcher("Select * From Win32_ComputerSystem");

            foreach (ManagementObject Mobject in Search.Get())
            {
                double Ram_Bytes = (Convert.ToDouble(Mobject["TotalPhysicalMemory"]));
                vram += ("\nОперативная память в ГБ: ", Convert.ToInt32(Ram_Bytes / 1073741824));
            }

            return vram;
        }

        private static string machineName()
        {
            string name = string.Empty;

            ManagementObjectSearcher myVideoObject = new ManagementObjectSearcher("select * from Win32_VideoController");

            foreach (ManagementObject obj in myVideoObject.Get())
            {
                name += "\nВидеокарта  -  " + obj["Name"];
            }

            return name;
        }

    }
}
