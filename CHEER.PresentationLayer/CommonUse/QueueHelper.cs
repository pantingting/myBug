using CHEER.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;

namespace CHEER.PresentationLayer.CommonUse
{
    //队列临时类  
    public class QueueInfo
    {
        public object DATA { get; set; }
        public string FUNMODULE { get; set; }
    }

    public class JGXMDATA
    {
        public string CODE { get; set; }
        public int TARGETNUM { get; set; }
    }

    public class ZRQDATA
    {
        public string CODE { get; set; }
    }

    public class QueueHelper
    {
        #region 解决修改监管项目最少巡查次数时，前台页面卡住的现象  
        //原理：利用生产者消费者模式进行入列出列操作  

        public readonly static QueueHelper Instance = new QueueHelper();
        private QueueHelper()
        { }

        private Queue<QueueInfo> ListQueue = new Queue<QueueInfo>();

        public void AddQueue(object DATA,string FUNMODULE = "JGXM") //入列  
        {
            QueueInfo queueinfo = new QueueInfo();

            queueinfo.DATA = DATA;
            queueinfo.FUNMODULE = FUNMODULE;

            ListQueue.Enqueue(queueinfo);
        }

        public void Start()//启动  
        {
            Thread thread = new Thread(threadStart);
            thread.IsBackground = true;
            thread.Start();
        }

        private void threadStart()
        {
            while (true)
            {
                if (ListQueue.Count > 0)
                {
                    try
                    {
                        ScanQueue();
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
                else
                {
                    //没有任务，休息3秒钟  
                    Thread.Sleep(3000);
                }
            }
        }

        //要执行的方法  
        private void ScanQueue()
        {
            while (ListQueue.Count > 0)
            {
                try
                {
                    //从队列中取出  
                    QueueInfo queueinfo = ListQueue.Dequeue();

                    //取出的queueinfo就可以用了，里面有你要的东西  
                    //以下就是处理程序了  
                    //。。。。。。  
                    CommonMethod.ExecuteFunc(_broker => {
                        if(queueinfo.FUNMODULE == "JGXM")
                        {
                            JGXMDATA data = queueinfo.DATA as JGXMDATA;
                            Log2.Instance.Error(typeof(QueueHelper), $"监管项目最小巡查次数处理开始，CODE:{data.CODE},TARGETNUM:{data.TARGETNUM}");

                            var dt = _broker.ExecuteSQLForDst($@"SELECT A.NBXH,A.JGXM,
                                (SELECT MAX(TARGETNUM) TARGETNUM FROM hk_bm_jgxmxl WHERE LOCATE(`code`,A.JGXM) > 0) TARGETNUM 
                                FROM hk_jd A where A.JDZT = '1' AND  A.JGXM like '%{data.CODE}%'")
                                .Tables[0].AsEnumerable().ToList();

                            StringBuilder sb = new StringBuilder();
                            StringBuilder sbNbxh = new StringBuilder();

                            _broker.ExecuteNonQuery($@"TRUNCATE TABLE tmp;");

                            var TARGETNUM = 0;
                            sb.Append($@"insert into tmp values ");
                            dt.ForEach(row =>
                            {
                                if (data.TARGETNUM >= Convert.ToInt32(row["TARGETNUM"]))
                                {
                                    TARGETNUM = data.TARGETNUM;
                                }
                                else
                                {
                                    TARGETNUM = Convert.ToInt32(row["TARGETNUM"]);
                                }
                                sb.Append($@"('{row["NBXH"]}','{TARGETNUM}'),");
                            });
                            var updateSql = sb.ToString().TrimEnd(',') + ";";
                            _broker.ExecuteNonQuery(updateSql);

                            var sql = $@"update hk_jd, tmp set hk_jd.MINNUM=tmp.MINNUM where hk_jd.NBXH=tmp.NBXH;";
                            _broker.ExecuteNonQuery(sql);

                            Log2.Instance.Error(typeof(QueueHelper), $"监管项目最小巡查次数处理结束，CODE:{data.CODE},TARGETNUM:{data.TARGETNUM}");
                        }
                        else if(queueinfo.FUNMODULE == "JDXX")
                        {
                            //处理责任区修改责任人后 建档信息
                            ZRQDATA data = queueinfo.DATA as ZRQDATA;
                            Log2.Instance.Error(typeof(QueueHelper), $"批量修改建档信息责任人开始，责任区code:{data.CODE}");

                            var zzzr = _broker.ExecuteSQLForDst($"SELECT ZZZR FROM bm_pianhao WHERE `CODE` = '{data.CODE}'").Tables[0].Rows[0]["ZZZR"].ToString();

                            var sql = $@"SELECT `CODE` FROM bm_glwg WHERE ZRQCODE = '{data.CODE}'";
                            var dt = _broker.ExecuteSQLForDst(sql).Tables[0];
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                _broker.ExecuteNonQuery($"UPDATE hk_jd SET GLPH = '{data.CODE}',ZRR = '{zzzr}' WHERE GLWG = '{dt.Rows[i]["CODE"].ToString()}'");
                            }

                            Log2.Instance.Error(typeof(QueueHelper), $"批量修改建档信息责任人结束，责任区code:{data.CODE}");
                        }
                    }, ex => {
                        Log2.Instance.Error(typeof(QueueHelper), $"队列处理出错...{ex.Message}");
                    });
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }


        #endregion
    }
}