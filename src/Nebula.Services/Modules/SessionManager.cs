using System;
using NHibernate;

namespace Nebula.Services.Modules
{
    public interface ISessionManager : IDisposable
    {
        /// <summary>
        /// Is there a current session ?
        /// </summary>
        bool HasActive { get; }

        /// <summary>
        /// Gets or creates a new session and tracks it
        /// </summary>
        ISession GetSession();

        /// <summary>
        /// Always create a new session and does not track it, so you are obliged to do your own management here.
        /// </summary>
        /// <returns></returns>
        ISession OpenSession();
    }

    public class SessionManager : ISessionManager
    {
        public SessionManager(ISessionFactory sessionFactory)
        {
            SessionFactory = sessionFactory;
        }

        public ISessionFactory SessionFactory { get; private set; }

        public virtual ISession ActiveSession { get; private set; }

        public virtual bool HasActive
        {
            get { return ActiveSession != null; }
        }

        public virtual ISession GetSession()
        {
            if (!HasActive)
                ActiveSession = OpenSession();

            return ActiveSession;
        }

        public virtual ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }

        public void Dispose()
        {
            if (SessionFactory == null) return;
            SessionFactory.Dispose();
            SessionFactory = null;
            ActiveSession = null;
        }
    }
}