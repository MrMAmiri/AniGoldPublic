using AniGoldShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AniGoldShop.Domain.Interfaces
{
    public interface IRepository<TEntity, TId>
    {
        Task Insert(TEntity entity, bool autosave = false);
        Task InsertRange(IEnumerable<TEntity> entity, bool autosave = false);
        Task Update(TEntity entity, bool autosave = false);
        Task Delete(TId id, bool autosave = false);
        Task Delete(Expression<Func<TEntity, bool>> where, bool autosave = false);
        Task SaveChange();
        Task<TEntity> Find(TId id);
        Task<bool> Exist(Expression<Func<TEntity, bool>> where);
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> where);
        Task<IEnumerable<TEntity>> FindAsync<TProperties>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TProperties>> include, int pgNumb = 1, int pgSize = 50);
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, dynamic>> orderby, bool DescendingOrder = false, int top = -1, params Expression<Func<TEntity, dynamic>>[] includes);
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, dynamic>>[] orderby, bool DescendingOrder = false, int top = -1, params Expression<Func<TEntity, dynamic>>[] includes);
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, dynamic>>[] includes);
        Task<decimal> MaxAsync( Expression<Func<TEntity, decimal>> max);
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> where);
        IQueryable<TEntity> GetModel();
        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> where);
        Task<TEntity> FindLast();
        Task<TEntity> FindLast<TResult>(Expression<Func<TEntity, TResult>> selector);
        Task<TEntity> FindFirst();
        Task<IEnumerable<TEntity>> GetAll<TProperties>(Expression<Func<TEntity, TProperties>> orderby);
        Task<TEntity> GetFirstOrderBy<TProperties>(Expression<Func<TEntity, TProperties>> orderby, Expression<Func<TEntity, bool>> where=null, bool desc = false);
        Task<IEnumerable<TEntity>> GetAll();

        Task<IEnumerable<TEntity>> GetAll<TOrderProperties, TProperties>(Expression<Func<TEntity, TOrderProperties>> orderby, Expression<Func<TEntity, TProperties>> include);
        Task<IEnumerable<TEntity>> FindGODAsync(Expression<Func<TEntity, bool>> where=null, Expression<Func<TEntity, dynamic>> orderby=null, bool DescendingOrder = false, int pgNumber = 1,int pgSize=500, params Expression<Func<TEntity, dynamic>>[] includes);
        Task<(decimal, int, int)> CountGODAsync(Expression<Func<TEntity, bool>> where=null, int pgSize = 0, int pgNumber = 0);


    }
}
