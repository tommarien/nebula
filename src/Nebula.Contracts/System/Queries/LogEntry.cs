using System;

namespace Nebula.Contracts.System.Queries
{
    public class LogEntry
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Level { get; set; }
        public string Logger { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public string HostName { get; set; }
        public string SessionId { get; set; }
        public string UserName { get; set; }
    }
}