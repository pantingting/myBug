using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace CHEER.Service
{
    static class Program
    {
        ///// <summary>
        ///// 应用程序的主入口点。
        ///// </summary>
        //static void Main()
        //{
        //    ServiceBase[] ServicesToRun;
        //    ServicesToRun = new ServiceBase[] 
        //    { 

        //    };
        //    ServiceBase.Run(ServicesToRun);
        //}
        static void Main(string[] args)
        {            
            if (args.Length > 0 && args[0] == "s")
            {              
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                    new Choositon(),
                };
                ServiceBase.Run(ServicesToRun);
            }
            else
            {
                Console.WriteLine("这是Windows应用程序");
                Console.WriteLine("请选择，[1]安装服务 [2]卸载服务 [3]退出");
                var rs = int.Parse(Console.ReadLine());
                switch (rs)
                {
                    case 1:
                        //取当前可执行文件路径，加上"s"参数，证明是从windows服务启动该程序
                        var path = Process.GetCurrentProcess().MainModule.FileName + " s";
                        Process.Start("sc", "create Choositon binpath= \"" + path + "\" displayName= Choositon start= auto");
                        Console.WriteLine("安装成功");
                        Console.Read();
                        break;
                    case 2:
                        Process.Start("sc", "delete Choositon");
                        Console.WriteLine("卸载成功");
                        Console.Read();
                        break;
                    case 3: break;
                }
            }
        }
    }
}
