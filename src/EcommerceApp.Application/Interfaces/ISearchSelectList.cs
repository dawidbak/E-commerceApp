using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EcommerceApp.Application.Interfaces
{
    public interface ISearchSelectList
    {
        List<SelectListItem> CategorySelectList{get;}
        List<SelectListItem> ProductSelectList{get;}
        List<SelectListItem> EmployeeSelectList{get;}
        List<SelectListItem> CustomerSelectList{get;}
    }
}
