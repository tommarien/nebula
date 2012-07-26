using System;

namespace Nebula.Infrastructure
{
    public interface ISystemClock
    {
        DateTime Now();
        DateTime Today();
    }
}