namespace zic_dotnet.Domain.Repositories
{
    /// <summary>
    /// Represents the object that manages the repository context.
    /// </summary>
    public sealed class RepositoryContextManager : Disposable, IUnitOfWork
    {
        #region Private Fields
        private readonly IRepositoryContext context;
        #endregion

        #region Ctor
        /// <summary>
        /// Initializes a new instance of <c>RepositoryContextManager</c> class.
        /// </summary>
        public RepositoryContextManager()
        {
            context = ServiceLocator.Instance.GetService<IRepositoryContext>();
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Disposes the object.
        /// </summary>
        /// <param name="disposing">A <see cref="System.Boolean"/> value which indicates whether
        /// the object should be disposed explicitly.</param>
        protected override void DisposeCore()
        {
            context.Dispose();
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the instance of the managed repository context.
        /// </summary>
        public IRepositoryContext Context
        {
            get { return this.context; }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Returns the instance of the repository for the specific type of aggregate root.
        /// </summary>
        /// <typeparam name="T">The type of the aggregate root.</typeparam>
        /// <returns>The instance of the repository.</returns>
        public IRepository<T> GetRepository<T>()
            where T : class, IAggregateRoot
        {
            return ServiceLocator.Instance.GetService<IRepository<T>>(new { context = context });
        }
        #endregion

        #region IUnitOfWork Members
        /// <summary>
        /// Gets a <see cref="System.Boolean"/> value which indicates whether the UnitOfWork
        /// was committed.
        /// </summary>
        public bool Committed
        {
            get { return context.Committed; }
        }
        /// <summary>
        /// Commits the UnitOfWork.
        /// </summary>
        public void Commit()
        {
            context.Commit();
        }
        /// <summary>
        /// Rolls-back the UnitOfWork.
        /// </summary>
        public void Rollback()
        {
            context.Rollback();
        }

        #endregion
    }
}
