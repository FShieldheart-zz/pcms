using System.Threading.Tasks;
using Structure.Domain.Interfaces;

namespace Service.AccessPoint.Interfaces
{
    public interface IBaseService<T> where T : IBase, new()
    {
        Task OnCreatedAsync(T model);
        Task OnCreatingAsync(T model);
        Task OnDeletedAsync(T model);
        Task OnDeletingAsync(T model);
        Task OnUpdatedAsync(T model);
        Task OnUpdatingAsync(T model);
    }
}