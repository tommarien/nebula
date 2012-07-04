using System;

namespace Nebula.Contracts.System.Queries
{
    public class LogSummary
    {
        public int Id { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual string Level { get; set; }
        public virtual string Logger { get; set; }
        public virtual string Message { get; set; }
    }
}