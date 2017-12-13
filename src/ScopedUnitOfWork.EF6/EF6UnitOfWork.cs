using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using CommonServiceLocator;
using ScopedUnitOfWork.Framework;
using ScopedUnitOfWork.Interfaces;
using ScopedUnitOfWork.Interfaces.Exceptions;

namespace ScopedUnitOfWork.EF6
{
    public class EF6UnitOfWork<TContext> : UnitOfWork<TContext> where TContext : DbContext
    {
        public EF6UnitOfWork(TContext context, IScopeManager scopeManager, IServiceLocator serviceLocator, ScopeType scopeType) 
            : base(context, scopeManager, serviceLocator, scopeType)
        {
        }

        protected override void SaveContextChanges()
        {
            try
            {
                Context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                throw new ConcurrentModificationException(
                    "The record you attempted to edit was modified by another " +
                    "user after you loaded it. The edit operation was cancelled and the " +
                    "currect values in the database are displayed. Please try again.", exception);
            }
        }
    }
}