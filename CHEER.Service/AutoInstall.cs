using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace CHEER.Service
{
    [RunInstaller(true)]
    public partial class AutoInstall : System.Configuration.Install.Installer
    {
       private ServiceInstaller serviceInstaller;
        private ServiceProcessInstaller processInstaller;

        public AutoInstall()
        {
            processInstaller = new ServiceProcessInstaller();
            serviceInstaller = new ServiceInstaller();
            processInstaller.Account = ServiceAccount.LocalSystem;
            serviceInstaller.StartType = ServiceStartMode.Manual;
            serviceInstaller.ServiceName = "Choositon";
            Installers.Add(serviceInstaller);
            Installers.Add(processInstaller);
        }
    }
}
