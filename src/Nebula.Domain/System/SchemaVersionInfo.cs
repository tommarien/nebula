using System;

namespace Nebula.Domain.System
{
    public class SchemaVersionInfo
    {
        protected SchemaVersionInfo()
        {
        }

        public virtual long Version { get; protected set; }
        public virtual DateTime AppliedOn { get; protected set; }
    }
}