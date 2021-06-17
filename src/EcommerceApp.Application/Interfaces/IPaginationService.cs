using System.Linq;
using System.Threading.Tasks;
using EcommerceApp.Application.ViewModels;

namespace EcommerceApp.Application.Interfaces
{
    public interface IPaginationService<T>
    {
        Task<PaginatedVM<T>> CreateAsync(IQueryable<T> source, int currentPage, int pageSize);
    }
}
