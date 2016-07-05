using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ranta.LogMagic.Contract;
using System.ServiceModel;
using System.Configuration;
using System.Diagnostics;
using Ranta.LogMagic.Common.Generic;
using Ranta.LogMagic.Common;

namespace Ranta.LogMagic.Enqueue.Core
{
    public class LogService : ILogService
    {
        private Msmq<MessageBase<LogEvent>> msmq = null;

        public LogService()
        {
            try
            {
                msmq = new Msmq<MessageBase<LogEvent>>(ConfigurationManager.AppSettings["RantaLogQueue"]);
            }
            catch (Exception ex)
            {
                LocalLog.Error(ex.ToString());
            }
        }

        public LogResponse WriteLog(LogRequest request)
        {
            LogResponse response = new LogResponse();

            try
            {
                var message = new MessageBase<LogEvent>();

                message.Guid = request.Guid;
                message.Content = request.LogEvent;

                msmq.Enqueue(message);

                response.Message = string.Format("Message {0} is in queue now.", request.Guid);
            }
            catch (Exception ex)
            {
                LocalLog.Error(ex.ToString());

                var fault = new FaultData { Message = ex.ToString() };
                var reason = new FaultReason("WriteLog发生异常");

                throw new FaultException<FaultData>(fault, reason);
            }

            return response;
        }

        public Task<LogResponse> CreateTaskOfWriteLog(LogRequest request)
        {
            var task = new Task<LogResponse>(() =>
            {
                return this.WriteLog(request);
            });

            return task;
        }
    }
}
