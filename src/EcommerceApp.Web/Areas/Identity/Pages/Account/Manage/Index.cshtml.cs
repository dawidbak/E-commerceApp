using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommerceApp.Web.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ICustomerRepository _customerRepository;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ICustomerRepository customerRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _customerRepository = customerRepository;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [StringLength(50, MinimumLength = 2,ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.")]
            [Display(Name ="First Name")]
            public string FirstName { get; set; }

            [StringLength(50, MinimumLength = 2,ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.")]
            [Display(Name ="Last Name")]
            public string LastName { get; set; }

            [StringLength(50, MinimumLength = 2,ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.")]
            [Display(Name ="City")]
            public string City { get; set; }

            [StringLength(10, MinimumLength = 5,ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.")]
            [Display(Name ="Postal Code")]
            public string PostalCode { get; set; }

            [StringLength(50, MinimumLength = 2,ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.")]
            [Display(Name ="Address")]
            public string Address { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var customerId = await _customerRepository.GetCustomerIdAsync(user.Id);
            var customer = await _customerRepository.GetCustomerAsync(customerId);

            Username = userName;

            Input = new InputModel
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                City = customer.City,
                PostalCode = customer.PostalCode,
                Address = customer.Address,
                PhoneNumber = phoneNumber
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var customer = new Customer
            {
                Id = await _customerRepository.GetCustomerIdAsync(user.Id),
                FirstName = Input.FirstName,
                LastName = Input.LastName,
                City = Input.City,
                PostalCode = Input.PostalCode,
                Address = Input.Address,
                AppUserId = user.Id,
            };

            await _customerRepository.UpdateCustomerAsync(customer);

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
