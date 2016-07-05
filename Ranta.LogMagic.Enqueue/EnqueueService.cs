using Ranta.LogMagic.Enqueue.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Ranta.LogMagic.Server
{
    public partial class EnqueueService : ServiceBase
    {
        ServiceHost host = null;

        public EnqueueService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                LocalLog.Info("Starting RantaLogService.");

                host = new ServiceHost(typeof(LogService));

                host.Open();

                LocalLog.Info("RantaLogService Started.");
            }
            catch (Exception ex)
            {
                LocalLog.Error(ex.ToString());
            }
        }

        protected override void OnStop()
        {
            try
            {
                LocalLog.Info("Stopping RantaLogService。");

                host.Close();

                LocalLog.Info("RantaLogService stopped.");
            }
            catch (Exception ex)
            {
                LocalLog.Error(ex.ToString());
            }
        }
    }
}
