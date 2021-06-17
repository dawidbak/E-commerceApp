using System;
using System.Collections.Generic;

namespace EcommerceApp.Application.ViewModels
{
    public class PaginatedVM<T>
    {
        public int CurrentPage { get; set; }
        public List<T> Items { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }
}
