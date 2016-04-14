using System;
using System.Collections.Generic;
using System.Linq;
using ScopedUnitOfWork.Interfaces;

namespace ScopedUnitOfWork.Framework
{
    public class UoWScopeStack<TContext> : IDisposable where TContext : IDisposable
    {
        private bool _isRolledBack;

        public UoWScopeStack(TContext context)
        {
            Context = context;
            Stack = new Stack<IUnitOfWork>();
        }

        public TContext Context { get; protected set; }
        public Stack<IUnitOfWork> Stack { get; protected set; } 

        public ITransactionWrapper Transaction { get; private set; } 

        public void Dispose()
        {
            Context.Dispose();
            Transaction?.Dispose();
        }

        public bool IsRolledBack()
        {
            return _isRolledBack;
        }

        public void RollBack()
        {
            Transaction.Rollback();
            _isRolledBack = true;
        }

        public void CleanTransaction()
        {
            _isRolledBack = false;

            if (Transaction != null)
            {
                Transaction.Dispose();
                Transaction = null;
            }
        }

        public bool HasTransaction()
        {
            return Transaction != null;
        }

        public void SetTransaction(ITransactionWrapper transaction)
        {
            Transaction = transaction;
        }

        public bool AnyTransactionalUnitsOfWork()
        {
            return Stack.ToArray().Any(x => x.ScopeType == ScopeType.Transactional);
        }

        public bool AnyTransactionalUnitsOfWorkBesides(IUnitOfWork unitOfWork)
        {
            return Stack.ToArray()
                .Where(x => x.ScopeType == ScopeType.Transactional)
                .Any(x => !ReferenceEquals(x, unitOfWork));
        }
    }
}