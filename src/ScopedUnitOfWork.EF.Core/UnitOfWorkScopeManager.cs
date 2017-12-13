using CommonServiceLocator;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ScopedUnitOfWork.Framework;
using ScopedUnitOfWork.Interfaces;

namespace ScopedUnitOfWork.EF.Core
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
            return new EFCoreUnitOfWork<TContext>(ScopeStack.Context, this, _serviceLocator, scopeType);
        }

        protected override ITransactionWrapper CreateAndStartTransaction()
        {
            return new TransactionWrapper(ScopeStack.Context.Database.BeginTransaction());
        }

        class TransactionWrapper : ITransactionWrapper
        {
            private readonly IDbContextTransaction _transaction;

            public TransactionWrapper(IDbContextTransaction transaction)
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