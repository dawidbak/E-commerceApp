using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Application.Services
{
    public class PaginationService<T> : IPaginationService<T>
    {
        public async Task<PaginatedVM<T>> CreateAsync(IQueryable<T> source, int currentPage, int pageSize)
        {
            var count = await source.ToAsyncEnumerable().CountAsync();
            source = source.Skip((currentPage - 1) * pageSize).Take(pageSize);
            var items = await source.ToAsyncEnumerable().ToListAsync();
            return new PaginatedVM<T>
            {
                Items = items,
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalCount = count,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize)
            };
        }
    }
}
