using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prak_G_13_Client
{
    public class AppInfo
    {
        private static readonly AppInfo instance = new AppInfo();
        public int userId;
        public string user = "DXR1DXR";
        public string baseAdress = "http://192.168.0.103:58799/api/";
        public string project = "prak_G_13_Client";
        public string file = "prak_G_13_Client.exe";
        public AppInfo()
        {
        }
        public static AppInfo GetInstance()
        {
            return instance;
        }
    }
}
