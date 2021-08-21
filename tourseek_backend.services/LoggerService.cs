using System;
using System.Text;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RT.Comb;

namespace tourseek_backend.services
{
    public class LoggerService<T> where T : class
    {
        private readonly string _controller;
        private readonly string _action;
        public Guid ActionId { get; }
        private ILogger<T> Logger { get; }

        public LoggerService(ILogger<T> logger, string controller, string action)
        {
            Logger = logger;
            _controller = controller;
            _action = action;
            ActionId = Provider.PostgreSql.Create();
        }

        private string BaseMessage(string type) => $"{_controller}/{_action}|{type}-{ActionId}";
        private void Log(string type, params Tuple<string, object>[] rParams)
        {
            var sb = new StringBuilder(BaseMessage(type));
            foreach (var (name, value) in rParams)
            {
                sb.Append("| ").Append(name).Append(": ").Append(JsonConvert.SerializeObject(value, Formatting.None));
            }
            Logger.Log(LogLevel.Information, sb.ToString());
        }

        public void LogRequest(params Tuple<string, object>[] rParams) => Log("Request", rParams);

        public void LogResponse(params Tuple<string, object>[] rParams) => Log("Response", rParams);

        public void LogError(Exception e) => Logger.Log(LogLevel.Error, e, BaseMessage("Error"));
    }
}