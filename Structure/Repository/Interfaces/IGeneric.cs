using SQLite;
using Structure.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Repository.UnitOfWork.Classes
{
    public interface IGeneric<T> where T : IBase, new()
    {
        Task<int> DeleteAsync(T baseModel, bool deletePermanently = false);
        Task<int> DeleteByIdAsync(int id, bool persist = false, bool includeDeleted = false, bool deletePermanently = false);
        Task<int> DeleteManyAsync(IEnumerable<T> baseModels, bool deletePermanently = false);
        Task<int> DeleteManyByIdsAsync(IEnumerable<int> ids, bool includeDeleted = false, bool deletePermanently = false);
        Task<IList<T>> GetManyAsync(Expression<Func<T, bool>> filter = null, Func<AsyncTableQuery<T>, AsyncTableQuery<T>> order = null, int skip = 0, int take = 0, bool includeDeleted = false);
        Task<int> CountAsync(Expression<Func<T, bool>> filter = null, bool includeDeleted = false);
        Task<T> GetSingleAsync(int id, bool includeDeleted = false);
        Task<int> InsertAsync(T baseModel);
        Task<int> InsertManyAsync(IEnumerable<T> baseModels);
        Task<int> ReplaceAsync(T baseModel);
        Task<int> ReplaceManyAsync(IList<T> baseModels);
        Task<int> UpdateAsync(T baseModel);
        Task<int> UpdateManyAsync(IEnumerable<T> baseModels);
    }
}