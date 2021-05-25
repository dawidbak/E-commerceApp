using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EcommerceApp.Web.Models;
using Microsoft.AspNetCore.Authorization;
using EcommerceApp.Infrastructure.Repositories;

namespace EcommerceApp.Web.Controllers
{
    public class AdminPanelController : Controller
    {
        

        public AdminPanelController(EmployeeRepository employeeRepository)
        {
            
        }
        
    }
}