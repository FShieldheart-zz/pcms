using Repository.UnitOfWork.Classes;
using Service.AccessPoint.Interfaces;
using Structure.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace Service.AccessPoint.Classes
{
    public abstract class BaseService<T> : IBaseService<T> where T : IBase, new()
    {
        protected IGeneric<T> BaseRepository;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public virtual async Task OnCreatingAsync(T model)
        {
            model.CreatedDate = DateTime.Now;
        }
        public virtual async Task OnCreatedAsync(T model)
        {
            //TODO: Logging
        }
        public virtual async Task OnUpdatingAsync(T model)
        {
            model.UpdatedDate = DateTime.Now;
        }
        public virtual async Task OnUpdatedAsync(T model)
        {
            //TODO: Logging
        }
        public virtual async Task OnDeletingAsync(T model)
        {
            //Back-Up
        }
        public virtual async Task OnDeletedAsync(T model)
        {
            //TODO: Logging
        }
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    }
}
