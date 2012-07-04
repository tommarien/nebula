using System;

namespace Nebula.Domain.System
{
    public class Log
    {
        public virtual int Id { get; protected set; }
        public virtual DateTime Date { get; set; }
        public virtual string Level { get; set; }
        public virtual string Logger { get; set; }
        public virtual string Message { get; set; }
        public virtual string Exception { get; set; }
        public virtual string HostName { get; set; }
        public virtual string SessionId { get; set; }
        public virtual string UserName { get; set; }
    }
}