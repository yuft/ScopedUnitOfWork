namespace ScopedUnitOfWork.Interfaces
{
    /// <summary>
    /// Type of the created unit of work.
    /// </summary>
    public enum ScopeType
    {
        /// <summary>
        /// Default behavior - only the context is 
        /// shared with other units of work down the stack.
        /// The unit of work DOES NOT behave transactionaly.
        /// </summary>
        Default,

        /// <summary>
        /// In addition to context sharing, the unit of work is 
        /// also transactional (by manging database transacation),
        /// and this behaviour applies to all other units of work
        /// down the stack as well (also those created with
        /// <see cref="Default"/>).
        /// </summary>
        Transactional
    }
}