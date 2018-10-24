using Bhbk.Lib.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace Bhbk.Lib.Core.Providers
{
    public abstract class ActivityProvider
    {

    }

    public class ActivityProvider<TEntity> : ActivityProvider
        where TEntity : class, IActivity, new()
    {
        private readonly DbContext _context;

        public ActivityProvider(DbContext context)
        {
            _context = context;
        }

        public void Commit(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            _context.SaveChanges();
        }
    }
}
