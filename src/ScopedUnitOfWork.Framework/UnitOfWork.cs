using System;
using Microsoft.Practices.ServiceLocation;
using ScopedUnitOfWork.Interfaces;

namespace ScopedUnitOfWork.Framework
{
    public abstract class UnitOfWork<TContext> : IContextAwareUnitOfWork where TContext : class, IDisposable
    {
        protected readonly TContext Context;
        private readonly IServiceLocator _serviceLocator;
        private readonly IScopeManager _scopeManager;

        protected UnitOfWork(TContext context, IScopeManager scopeManager, IServiceLocator serviceLocator, ScopeType scopeType)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (scopeManager == null) throw new ArgumentNullException(nameof(scopeManager));
            if (serviceLocator == null) throw new ArgumentNullException(nameof(serviceLocator));

            Context = context;
            _scopeManager = scopeManager;
            _serviceLocator = serviceLocator;

            ScopeType = scopeType;
        }

        public object GetContext()
        {
            return Context;
        }

        public TRepository GetRepository<TRepository>() where TRepository : class, IRepository
        {
            var repository = _serviceLocator.GetInstance<TRepository>();
            repository.SetUnitOfWork(this);
            return repository;
        }
        
        public ScopeType ScopeType { get; private set; }
        
        public bool IsFinished { get; private set; }

        public void Commit()
        {
            if (IsFinished)
                throw new InvalidOperationException("Unit of work could not be commited either because it was " +
                                                    "already commited or it was disposed.");

            _scopeManager.Complete(this);

            try
            {
                SaveContextChanges();
            }
            finally
            {
                IsFinished = true;
            }
        }

        protected abstract void SaveContextChanges();

        public void Dispose()
        {
            _scopeManager.Remove(this);

            IsFinished = true;
        }
    }
}