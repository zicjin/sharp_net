using System;
using System.Collections.Generic;
using zic_dotnet;

namespace zic_dotnet.Domain.Repositories
{
    /// <summary>
    /// Represents the base class for repository contexts.
    /// </summary>
    public abstract class RepositoryContext : Disposable, IRepositoryContext
    {
        #region Private Fields
        private readonly Guid id = Guid.NewGuid();
        [ThreadStatic]
        private readonly Dictionary<Guid, object> newCollection = new Dictionary<Guid, object>();
        [ThreadStatic]
        private readonly Dictionary<Guid, object> modifiedCollection = new Dictionary<Guid, object>();
        [ThreadStatic]
        private readonly Dictionary<Guid, object> deletedCollection = new Dictionary<Guid, object>();
        [ThreadStatic]
        private bool committed = true;
        #endregion

        #region Protected Methods
        /// <summary>
        /// Clears all the registration in the repository context.
        /// </summary>
        /// <remarks>Note that this can only be called after the repository context has successfully committed.</remarks>
        protected void ClearRegistrations()
        {
            this.newCollection.Clear();
            this.modifiedCollection.Clear();
            this.deletedCollection.Clear();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Returns the instance of the repository for the specific type of aggregate root.
        /// </summary>
        /// <typeparam name="T">The type of the aggregate root.</typeparam>
        /// <returns>The instance of the repository.</returns>
        public IRepository<T> GetRepository<T>()
            where T : class, IAggregateRoot {
            return IocLocator.Instance.GetService<IRepository<T>>(new { context = this });
        }
        #endregion

        #region Protected Properties
        /// <summary>
        /// Gets an enumerator which iterates over the collection that contains all the objects need to be added to the repository.
        /// </summary>
        protected IEnumerable<KeyValuePair<Guid, object>> NewCollection
        {
            get { return newCollection; }
        }
        /// <summary>
        /// Gets an enumerator which iterates over the collection that contains all the objects need to be modified in the repository.
        /// </summary>
        protected IEnumerable<KeyValuePair<Guid, object>> ModifiedCollection
        {
            get { return modifiedCollection; }
        }
        /// <summary>
        /// Gets an enumerator which iterates over the collection that contains all the objects need to be deleted from the repository.
        /// </summary>
        protected IEnumerable<KeyValuePair<Guid, object>> DeletedCollection
        {
            get { return deletedCollection; }
        }
        #endregion

        #region IRepositoryContext Members
        /// <summary>
        /// Gets the ID of the repository context.
        /// </summary>
        public Guid ID
        {
            get { return id; }
        }
        /// <summary>
        /// Registers a new object to the repository context.
        /// </summary>
        /// <typeparam name="TAggregateRoot">The type of the aggregate root.</typeparam>
        /// <param name="obj">The object to be registered.</param>
        public virtual void RegisterNew<TAggregateRoot>(TAggregateRoot obj) where TAggregateRoot : class, IAggregateRoot
        {
            if (obj.ID.Equals(Guid.Empty))
                throw new ArgumentException("The ID of the object is empty.", "obj");
            if (modifiedCollection.ContainsKey(obj.ID))
                throw new InvalidOperationException("The object cannot be registered as a new object since it was marked as modified.");
            if (newCollection.ContainsKey(obj.ID))
                throw new InvalidOperationException("The object has already been registered as a new object.");
            newCollection.Add(obj.ID, obj);
            committed = false;
        }
        /// <summary>
        /// Registers a modified object to the repository context.
        /// </summary>
        /// <typeparam name="TAggregateRoot">The type of the aggregate root.</typeparam>
        /// <param name="obj">The object to be registered.</param>
        public virtual void RegisterModified<TAggregateRoot>(TAggregateRoot obj) where TAggregateRoot : class, IAggregateRoot
        {
            if (obj.ID.Equals(Guid.Empty))
                throw new ArgumentException("The ID of the object is empty.", "obj");
            if (deletedCollection.ContainsKey(obj.ID))
                throw new InvalidOperationException("The object cannot be registered as a modified object since it was marked as deleted.");
            if (!modifiedCollection.ContainsKey(obj.ID) && !newCollection.ContainsKey(obj.ID))
                modifiedCollection.Add(obj.ID, obj);
            committed = false;
        }
        /// <summary>
        /// Registers a deleted object to the repository context.
        /// </summary>
        /// <typeparam name="TAggregateRoot">The type of the aggregate root.</typeparam>
        /// <param name="obj">The object to be registered.</param>
        public virtual void RegisterDeleted<TAggregateRoot>(TAggregateRoot obj) where TAggregateRoot : class, IAggregateRoot
        {
            if (obj.ID.Equals(Guid.Empty))
                throw new ArgumentException("The ID of the object is empty.", "obj");
            if (newCollection.ContainsKey(obj.ID))
            {
                if (newCollection.Remove(obj.ID))
                    return;
            }
            bool removedFromModified = modifiedCollection.Remove(obj.ID);
            bool addedToDeleted = false;
            if (!deletedCollection.ContainsKey(obj.ID))
            {
                deletedCollection.Add(obj.ID, obj);
                addedToDeleted = true;
            }
            committed = !(removedFromModified || addedToDeleted);
        }

        #endregion

        #region IUnitOfWork Members
        /// <summary>
        /// Gets a <see cref="System.Boolean"/> value which indicates whether the UnitOfWork
        /// was committed.
        /// </summary>
        public bool Committed
        {
            get { return committed; }
            protected set { committed = value; }
        }
        /// <summary>
        /// Commits the UnitOfWork.
        /// </summary>
        public abstract void Commit();
        /// <summary>
        /// Rolls-back the UnitOfWork.
        /// </summary>
        public abstract void Rollback();

        #endregion

    }
}
