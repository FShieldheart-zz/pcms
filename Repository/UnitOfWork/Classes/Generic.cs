using SQLite;
using Structure.Domain.Classes;
using Structure.Helper.Classes;
using Structure.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Repository.UnitOfWork.Classes
{
    public class Generic<T> : IGeneric<T> where T : Base, new()
    {
        private readonly IMainDbContext _mainContext;

        public Generic(IMainDbContext mainContext)
        {
            _mainContext = mainContext;
        }

        private async Task<IList<T>> GetManyReady(Expression<Func<T, bool>> filter = null,
                                           Func<AsyncTableQuery<T>, AsyncTableQuery<T>> order = null,
                                           int skip = 0,
                                           int take = 0,
                                           bool includeDeleted = false)
        {


            AsyncTableQuery<T> tableQuery = _mainContext.GetCurrentContext().Table<T>();

            if (!includeDeleted)
            {
                tableQuery = tableQuery.Where(q => q.IsDeleted.Equals(includeDeleted));
            }

            if (filter != null)
            {
                tableQuery = tableQuery.Where(filter);
            }

            if (order != null)
            {
                tableQuery = order(tableQuery);
            }

            if (order == null)
            {
                tableQuery = tableQuery.OrderByDescending(b => b.CreatedDate);
            }

            if (skip > 0)
            {
                tableQuery = tableQuery.Skip(skip);
            }

            if (take > 0)
            {
                tableQuery = tableQuery.Take(take);
            }

            return await tableQuery.ToListAsync();
        }

        private async Task<int> CountReady(Expression<Func<T, bool>> filter = null,
                                   bool includeDeleted = false)
        {


            AsyncTableQuery<T> tableQuery = _mainContext.GetCurrentContext().Table<T>();

            if (!includeDeleted)
            {
                tableQuery = tableQuery.Where(q => q.IsDeleted.Equals(includeDeleted));
            }

            if (filter != null)
            {
                tableQuery = tableQuery.Where(filter);
            }

            return await tableQuery.CountAsync();
        }

        public async Task<T> GetSingleAsync(int id, bool includeDeleted = false)
        {
            T domain = await _mainContext.GetCurrentContext().FindAsync<T>(q => q.Id.Equals(id));
            return domain?.IsDeleted.Equals(includeDeleted) == false ? null : domain;
        }

        public async Task<IList<T>> GetManyAsync(Expression<Func<T, bool>> filter = null,
            Func<AsyncTableQuery<T>, AsyncTableQuery<T>> order = null,
            int skip = 0,
            int take = 0,
            bool includeDeleted = false)
        {
            return await GetManyReady(filter, order, skip, take, includeDeleted);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> filter = null,
            bool includeDeleted = false)
        {
            return await CountReady(filter, includeDeleted);
        }

        public async Task<int> InsertAsync(T baseModel)
        {
            return await _mainContext.GetCurrentContext().InsertAsync(baseModel);
        }

        public async Task<int> InsertManyAsync(IEnumerable<T> baseModels)
        {
            return await _mainContext.GetCurrentContext().InsertAllAsync(baseModels, true);
        }

        public async Task<int> UpdateAsync(T baseModel)
        {
            baseModel.UpdatedDate = DateTime.Now;
            return await _mainContext.GetCurrentContext().UpdateAsync(baseModel);
        }

        public async Task<int> UpdateManyAsync(IEnumerable<T> baseModels)
        {
            foreach (T baseModel in baseModels)
            {
                baseModel.UpdatedDate = DateTime.Now;
            }
            return await _mainContext.GetCurrentContext().UpdateAllAsync(baseModels, true);
        }

        public async Task<int> ReplaceAsync(T baseModel)
        {
            if (!(await DeleteAsync(baseModel)).Equals(1))
            {
                return 0;
            }

            baseModel.Reset();

            return await InsertAsync(baseModel);
        }

        public async Task<int> ReplaceManyAsync(IList<T> baseModels)
        {
            await DeleteManyAsync(baseModels);

            baseModels.Reset();

            return await InsertManyAsync(baseModels);
        }

        public async Task<int> DeleteAsync(T baseModel, bool deletePermanently = false)
        {
            if (baseModel == null)
            {
                return 0;
            }

            if (deletePermanently)
            {
                return await _mainContext.GetCurrentContext().DeleteAsync(baseModel);
            }
            else
            {
                baseModel.IsDeleted = true;
                return await UpdateAsync(baseModel);
            }
        }

        public async Task<int> DeleteManyAsync(IEnumerable<T> baseModels, bool deletePermanently = false)
        {
            if (baseModels.Any(baseModel => baseModel == null))
            {
                return 0;
            }

            if (deletePermanently)
            {
                int deletedCount = 0;
                foreach (T baseModel in baseModels)
                {
                    deletedCount += await _mainContext.GetCurrentContext().DeleteAsync(baseModel);
                }
                return deletedCount;
            }
            else
            {
                foreach (T baseModel in baseModels)
                {
                    baseModel.IsDeleted = true;
                }

                return await UpdateManyAsync(baseModels);
            }
        }

        public async Task<int> DeleteByIdAsync(int id, bool persist = false, bool includeDeleted = false, bool deletePermanently = false)
        {
            T baseModel = await GetSingleAsync(id, includeDeleted);
            return await DeleteAsync(baseModel, deletePermanently);
        }

        public async Task<int> DeleteManyByIdsAsync(IEnumerable<int> ids, bool includeDeleted = false, bool deletePermanently = false)
        {
            IList<T> baseModels;
            if (!includeDeleted)
            {
                baseModels = await GetManyAsync(q => ids.Contains(q.Id) && q.IsDeleted.Equals(includeDeleted));
            }
            else
            {
                baseModels = await GetManyAsync(q => ids.Contains(q.Id));
            }

            return await DeleteManyAsync(baseModels, deletePermanently);
        }

    }
}
