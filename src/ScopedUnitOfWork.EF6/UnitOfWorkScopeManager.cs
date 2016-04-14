using System.Data.Entity;
using Microsoft.Practices.ServiceLocation;
using ScopedUnitOfWork.Framework;
using ScopedUnitOfWork.Interfaces;

namespace ScopedUnitOfWork.EF6
{
    internal class UnitOfWorkScopeManager<TContext> : UnitOfWorkScopeManagerBase<TContext> where TContext : DbContext
    {
        private readonly IServiceLocator _serviceLocator;

        public UnitOfWorkScopeManager(IServiceLocator serviceLocator) : base(serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }

        protected override IUnitOfWork CreateUnitOfWork(ScopeType scopeType)
        {
            return new EF6UnitOfWork<TContext>(ScopeStack.Context, this, _serviceLocator, scopeType);
        }

        protected override ITransactionWrapper CreateAndStartTransaction()
        {
            return new TransactionWrapper(ScopeStack.Context.Database.BeginTransaction());
        }

        class TransactionWrapper : ITransactionWrapper
        {
            private readonly DbContextTransaction _transaction;

            public TransactionWrapper(DbContextTransaction transaction)
            {
                _transaction = transaction;
            }

            public void Dispose()
            {
                _transaction.Dispose();
            }

            public void Commit()
            {
                _transaction.Commit();
            }

            public void Rollback()
            {
                _transaction.Rollback();
            }
        }
    }
}