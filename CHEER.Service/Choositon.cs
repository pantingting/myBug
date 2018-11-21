using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Xml;

namespace CHEER.Service
{
    partial class Choositon : ServiceBase
    {

        public Choositon()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //启动服务
            this.runJobs();
        }

        protected override void OnStop()
        {
            //停止服务
            this.stopJobs();
        }

        //用哈希表存放任务项
        private Hashtable hashJobs;


        #region 自定义方法

        private void runJobs()
        {
            try
            {

                //加载工作项
                if (this.hashJobs == null)
                {

                    hashJobs = new Hashtable();

                    //获取configSections节点
                    XmlNode configSections = Base.ServiceTools.GetConfigSections();

                    foreach (XmlNode section in configSections)
                    {
                        //过滤注释节点（如section中还包含其它节点需过滤）
                        if (section.Name.ToLower() == "section")
                        {
                            //创建每个节点的配置对象
                            string sectionName = section.Attributes["name"].Value.Trim();
                            string sectionType = section.Attributes["type"].Value.Trim();

                            //程序集名称
                            string assemblyName = sectionType.Split(',')[1];
                            //完整类名
                            string classFullName = assemblyName + ".Jobs." + sectionName + ".Config";

                            //创建配置对象
                            Base.ServiceConfig config = (Base.ServiceConfig)Assembly.Load(assemblyName).CreateInstance(classFullName);
                            //创建工作对象
                            Base.ServiceJob job = (Base.ServiceJob)Assembly.Load(config.Assembly.Split(',')[1]).CreateInstance(config.Assembly.Split(',')[0]);
                            job.ConfigObject = config;

                            //将工作对象加载进HashTable
                            this.hashJobs.Add(sectionName, job);
                        }
                    }
                }

                //执行工作项
                if (this.hashJobs.Keys.Count > 0)
                {
                    foreach (Base.ServiceJob job in hashJobs.Values)
                    {
                        //插入一个新的请求到线程池
                        if (System.Threading.ThreadPool.QueueUserWorkItem(threadCallBack, job))
                        {
                            //方法成功排入队列
                        }
                        else
                        {
                            //失败
                        }
                    }
                }

            }
            catch (Exception error)
            {
                Base.ServiceTools.WriteLog(Base.ServiceTools.GetAppSetting("LOG_PATH") + "Error.txt", error.ToString(), true);
            }
        }

        private void stopJobs()
        {
            //停止
            if (this.hashJobs != null)
            {
                this.hashJobs.Clear();
            }
        }

        /// <summary>
        /// 线程池回调方法
        /// </summary>
        /// <param name="state"></param>
        private void threadCallBack(Object state)
        {
            while (true)
            {
                ((Base.ServiceJob)state).StartJob();
                //休眠1秒
                Thread.Sleep(1000);
            }
        }

        #endregion


    }
}
