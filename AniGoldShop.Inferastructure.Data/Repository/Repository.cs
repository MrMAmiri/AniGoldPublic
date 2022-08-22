using Microsoft.EntityFrameworkCore;
using AniGoldShop.Domain.Common.BaseModels;
using AniGoldShop.Domain.Interfaces;
using AniGoldShop.Inferastructure.Data.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AniGoldShop.Domain.Entities;

namespace AniGoldShop.Inferastructure.Data.Repository
{
    public class Repository<TEntity, TId> : IRepository<TEntity, TId> where TEntity : BaseEntity
    {
        private AnigoldContext _dbContext;

        public Repository(AnigoldContext dataContext)
        {
            _dbContext = dataContext;
        }

        public async Task Delete(TId id, bool autosave = false)
        {
            var entity = await _dbContext.Set<TEntity>().FindAsync(id);
            _dbContext.Set<TEntity>().Remove(entity);

            if (autosave)
                await SaveChange();
        }

        public async Task Delete(Expression<Func<TEntity, bool>> where, bool autosave = false)
        {
            var entities = _dbContext.Set<TEntity>().AsNoTracking().Where(where).AsAsyncEnumerable();

            await foreach (var entity in entities)
                _dbContext.Set<TEntity>().Remove(entity);

            if (autosave)
                await SaveChange();
        }

        public async Task<TEntity> Find(TId id)
        {
            var entity = await _dbContext.Set<TEntity>().FindAsync(id);

            return entity;
        }

        public async Task<bool> Exist(Expression<Func<TEntity, bool>> where)
        {
            var exisit = await _dbContext.Set<TEntity>().Where(where).AnyAsync();

            return exisit;
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> where)
        {
            var entities = await _dbContext.Set<TEntity>().Where(where).AsNoTracking().ToListAsync();

            return entities;
        }

        public IQueryable<TEntity> GetModel()
        {
            var entities = _dbContext.Set<TEntity>().AsNoTracking();
            return entities;
        }

        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> where)
        {
            var query = _dbContext.Set<TEntity>().Where(where).AsNoTracking();

            return query;
        }

        public async Task<IEnumerable<TEntity>> FindAsync<TProperties>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TProperties>> include, int pgNumb = 1, int pgSize = 500)
        {
            var entities = await _dbContext.Set<TEntity>().Include(include).Where(where)
                .AsNoTracking().Where(where).Skip((pgNumb - 1) * pgSize).Take(pgSize).ToListAsync();

            return entities;
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, dynamic>> orderby, bool DescendingOrder = false, int top = -1, params Expression<Func<TEntity, dynamic>>[] includes)
        {
            return await FindAsync(
                where,
                new Expression<Func<TEntity, dynamic>>[]
                {
                    orderby
                },
                DescendingOrder,
                top,
                includes
            );
        }

        public async Task<decimal> MaxAsync(Expression<Func<TEntity, decimal>> max)
        {
            if ((await _dbContext.Set<TEntity>().AnyAsync()))
            {
                var entities = await _dbContext.Set<TEntity>()
                    .AsNoTracking().MaxAsync(max);


                return entities;
            }
            else
                return 0;
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, dynamic>>[] orderby, bool DescendingOrder = false, int top = -1, params Expression<Func<TEntity, dynamic>>[] includes)
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>().AsQueryable();

            if (DescendingOrder)
            {
                for (int i = 0; i < orderby.Count(); i++)
                {
                    if (i == 0)
                    {
                        query = query.OrderByDescending(orderby[i]);
                    }
                    else
                    {
                        query = ((IOrderedQueryable<TEntity>)query).ThenByDescending(orderby[i]);
                    }
                }
            }
            else
            {
                for (int i = 0; i < orderby.Count(); i++)
                {
                    if (i == 0)
                    {
                        query = query.OrderBy(orderby[i]);
                    }
                    else
                    {
                        query = ((IOrderedQueryable<TEntity>)query).ThenBy(orderby[i]);
                    }
                }
            }

            foreach (var item in includes ?? Enumerable.Empty<Expression<Func<TEntity, dynamic>>>())
            {
                query = query.Include(item);
            }

            query = query.Where(where);

            if (top > 0)
            {
                query = query.Take(top);
            }

            var entities = await query.AsNoTracking().ToListAsync();

            return entities;
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, dynamic>>[] includes)
        {
            var set = _dbContext.Set<TEntity>();
            IQueryable<TEntity> query = _dbContext.Set<TEntity>().AsQueryable();
            foreach (var item in includes)
            {
                query = query.Include(item);
            }

            var entities = await query.Where(where).AsNoTracking().ToListAsync();

            return entities;
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> where)
        {
            var entities = _dbContext.Set<TEntity>().Where(where).AsNoTracking().AsEnumerable();

            return entities;
        }

        public async Task<TEntity> FindFirst()
        {
            var entity = await _dbContext.Set<TEntity>().FirstOrDefaultAsync();

            return entity;
        }

        public async Task<TEntity> FindLast()
        {
            var entity = await _dbContext.Set<TEntity>().LastOrDefaultAsync();

            return entity;
        }

        public async Task<TEntity> FindLast<TResult>(Expression<Func<TEntity, TResult>> selector)
        {

            var entity = await _dbContext.Set<TEntity>().AsNoTracking().OrderByDescending(selector).FirstOrDefaultAsync();
            return entity;
        }

        public async Task Insert(TEntity entity, bool autosave = false)
        {
            setNowDateTime(entity);

            _dbContext.Entry(entity).State = EntityState.Added;

            if (autosave)
                await SaveChange();
        }

        public async Task InsertRange(IEnumerable<TEntity> entity, bool autosave = false)
        {
            await _dbContext.AddRangeAsync(entity);

            if (autosave)
                await SaveChange();
        }

        public async Task SaveChange()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(TEntity entity, bool autosave = false)
        {
            setNowDateTime(entity);

            _dbContext.Entry(entity).State = EntityState.Modified;

            if (autosave)
                await SaveChange();
        }

        public async Task<IEnumerable<TEntity>> GetAll<TOrderProperties>(Expression<Func<TEntity, TOrderProperties>> orderby)
        {
            return await _dbContext
               .Set<TEntity>()
               .OrderBy(orderby)
               .AsNoTracking()
               .ToListAsync();
        }
        public async Task<TEntity> GetFirstOrderBy<TOrderProperties>(Expression<Func<TEntity, TOrderProperties>> orderby, Expression<Func<TEntity, bool>> where = null, bool desc = false)
        {
            IQueryable<TEntity> data = null;
            if (desc)
                data = _dbContext
                   .Set<TEntity>()
                   .OrderByDescending(orderby)
                   .AsNoTracking();
            else
                data = _dbContext
                   .Set<TEntity>()
                   .OrderBy(orderby)
                   .AsNoTracking();

            if (where != null)
                return await data.Where(where).FirstOrDefaultAsync();
            else
                return await data.FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await _dbContext
                .Set<TEntity>()
                .ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAll<TOrderProperties, TProperties>(Expression<Func<TEntity, TOrderProperties>> orderby, Expression<Func<TEntity, TProperties>> include)
        {
            return await _dbContext
               .Set<TEntity>()
               .OrderBy(orderby)
               .Include(include)
               .AsNoTracking()
               .ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> FindGODAsync(Expression<Func<TEntity, bool>> where = null, Expression<Func<TEntity, dynamic>> orderby = null, bool DescendingOrder = false, int pgNumber = 1, int pgSize = 500, params Expression<Func<TEntity, dynamic>>[] includes)
        {
            IQueryable<TEntity> data = _dbContext
               .Set<TEntity>()
               .Where(where != null ? where : g => 1 == 1)
               .AsQueryable();
            if (includes != null)
                foreach (var item in includes)
                {
                    data = data.Include(item);
                }



            if (orderby != null)
            {
                if (!DescendingOrder)
                    data = data.OrderBy(orderby);
                else
                    data = data.OrderByDescending(orderby);
            }

            return await data.Skip((pgNumber - 1) * pgSize).Take(pgSize).ToListAsync();

        }

        public async Task<(decimal, int, int)> CountGODAsync(Expression<Func<TEntity, bool>> where = null, int pgSize = 0, int pgNumber = 0)
        {
            decimal total = 0;
            int pageCount = 0;
            int nextPage = 0;

            total = await _dbContext
               .Set<TEntity>()
               .Where(where != null ? where : g => 1 == 1)
               .CountAsync();

            if (pgSize == 0)
                return (total, pageCount, nextPage);
            if (total > pgSize)
                pageCount = (int)Math.Floor(total / pgSize);
            else
                pageCount = 1;
            if (nextPage < pageCount)
                nextPage++;

            return (total, pageCount, nextPage);

        }





        private void setNowDateTime(TEntity entity)
        {
            // انتساب زمان جاری جهت ثبت خودکار آخرین زمان ثبت و ویرایش اطلاعات
            entity.CreateDate = DateTime.Now;
            entity.ModifiedDate = DateTime.Now;
        }

    }
}
