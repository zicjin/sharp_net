﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace zic_dotnet.Domain {
    /// <summary>
    /// Represents the Unit Of Work.
    /// </summary>
    public interface IUnitOfWork {
        /// <summary>
        /// Gets a <see cref="System.Boolean"/> value which indicates whether the UnitOfWork
        /// was committed.
        /// </summary>
        bool Committed {
            get;
        }
        /// <summary>
        /// Commits the UnitOfWork.
        /// </summary>
        void Commit();
        /// <summary>
        /// Rolls-back the UnitOfWork.
        /// </summary>
        void Rollback();
    }
}