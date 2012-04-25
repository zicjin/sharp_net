﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace zic_dotnet.Domain.Repositories.EntityFramework
{
    /// <summary>
    /// Represents that the 
    /// </summary>
    public interface IEntityFrameworkRepositoryContext : IRepositoryContext
    {
        DbContext Context { get; }
    }
}
