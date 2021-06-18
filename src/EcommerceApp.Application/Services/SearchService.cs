using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels.AdminPanel;
using EcommerceApp.Application.ViewModels.EmployeePanel;
using EcommerceApp.Domain.Interfaces;

namespace EcommerceApp.Application.Services
{
    public class SearchService : ISearchService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IPaginationService<EmployeeForListVM> _employeePaginationService;
        private readonly IPaginationService<CategoryForListVM> _categoryPaginationService;
        private readonly IPaginationService<ProductForListVM> _productPaginationService;
        private readonly IPaginationService<CustomerVM> _customerPaginationService;
        private readonly IPaginationService<OrderForListVM> _orderPaginationService;
        private readonly IMapper _mapper;
        public SearchService(ICategoryRepository categoryRepository, IProductRepository productRepository, IEmployeeRepository employeeRepository, ICustomerRepository customerRepository,
        IPaginationService<EmployeeForListVM> employeePaginationService, IPaginationService<CategoryForListVM> categoryPaginationService, IPaginationService<CustomerVM> customerPaginationService,
        IPaginationService<ProductForListVM> productPaginationService, IOrderRepository orderRepository, IPaginationService<OrderForListVM> orderPaginationService, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _employeeRepository = employeeRepository;
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
            _employeePaginationService = employeePaginationService;
            _categoryPaginationService = categoryPaginationService;
            _customerPaginationService = customerPaginationService;
            _productPaginationService = productPaginationService;
            _orderPaginationService = orderPaginationService;
            _mapper = mapper;
        }
        public async Task<ListCategoryForListVM> SearchSelectedCategoriesAsync(string selectedValue, string searchString, int pageSize, int pageNumber)
        {
            var idParse = int.TryParse(searchString, out int id);
            var emptyQuery = Enumerable.Empty<CategoryForListVM>().AsQueryable();
            var baseQuery = _categoryRepository.GetAllCategories().ProjectTo<CategoryForListVM>(_mapper.ConfigurationProvider);
            IQueryable<CategoryForListVM> query = selectedValue switch
            {
                "Id" => idParse ? baseQuery.Where(x => x.Id == id) : emptyQuery,
                "Name" => baseQuery.Where(x => x.Name.Contains(searchString)),
                _ => emptyQuery,
            };
            var paginatedVM = await _categoryPaginationService.CreateAsync(query, pageNumber, pageSize);
            return new ListCategoryForListVM
            {
                Categories = paginatedVM.Items,
                TotalPages = paginatedVM.TotalPages,
                CurrentPage = paginatedVM.CurrentPage
            };
        }

        public async Task<ListProductForListVM> SearchSelectedProductsAsync(string selectedValue, string searchString, int pageSize, int pageNumber)
        {
            var idParse = int.TryParse(searchString, out int id);
            var unitPriceParse = decimal.TryParse(searchString, out decimal price);
            var unitsInStock = int.TryParse(searchString, out int units);
            var emptyQuery = Enumerable.Empty<ProductForListVM>().AsQueryable();
            var baseQuery = _productRepository.GetAllProducts().ProjectTo<ProductForListVM>(_mapper.ConfigurationProvider);

            IQueryable<ProductForListVM> query = selectedValue switch
            {
                "Id" => idParse ? baseQuery.Where(x => x.Id == id) : emptyQuery,
                "Name" => baseQuery.Where(x => x.CategoryName.Contains(searchString)),
                "UnitPrice" => unitPriceParse ? baseQuery.Where(x => x.UnitPrice == price) : emptyQuery,
                "UnitsInStock" => unitsInStock ? baseQuery.Where(x => x.UnitsInStock == units) : emptyQuery,
                "CategoryName" => baseQuery.Where(x => x.Name.Contains(searchString)),
                _ => emptyQuery,
            };
            var paginatedVM = await _productPaginationService.CreateAsync(query, pageNumber, pageSize);
            return new ListProductForListVM
            {
                Products = paginatedVM.Items,
                TotalPages = paginatedVM.TotalPages,
                CurrentPage = paginatedVM.CurrentPage,
            };
        }

        public async Task<ListEmployeeForListVM> SearchSelectedEmployeesAsync(string selectedValue, string searchString, int pageSize, int pageNumber)
        {
            var idParse = int.TryParse(searchString, out int id);
            var emptyQuery = Enumerable.Empty<EmployeeForListVM>().AsQueryable();
            var baseQuery = _productRepository.GetAllProducts().ProjectTo<EmployeeForListVM>(_mapper.ConfigurationProvider);

            IQueryable<EmployeeForListVM> query = selectedValue switch
            {
                "Id" => idParse ? baseQuery.Where(x => x.Id == id) : emptyQuery,
                "FirstName" => baseQuery.Where(x => x.FirstName.Contains(searchString)),
                "Email" => baseQuery.Where(x => x.Email.Contains(searchString)),
                "LastName" => baseQuery.Where(x => x.LastName.Contains(searchString)),
                "Position" => baseQuery.Where(x => x.Position.Contains(searchString)),
                _ => emptyQuery,
            };
            var paginatedVM = await _employeePaginationService.CreateAsync(query, pageNumber, pageSize);
            return new ListEmployeeForListVM
            {
                Employees = paginatedVM.Items,
                TotalPages = paginatedVM.TotalPages,
                CurrentPage = paginatedVM.CurrentPage,
            };
        }

        public async Task<ListCustomerVM> SearchSelectedCustomersAsync(string selectedValue, string searchString, int pageSize, int pageNumber)
        {
            var idParse = int.TryParse(searchString, out int id);
            var emptyQuery = Enumerable.Empty<CustomerVM>().AsQueryable();
            var baseQuery = _productRepository.GetAllProducts().ProjectTo<CustomerVM>(_mapper.ConfigurationProvider);
            IQueryable<CustomerVM> query = selectedValue switch
            {
                "Id" => idParse ? baseQuery.Where(x => x.Id == id) : emptyQuery,
                "FirstName" => baseQuery.Where(x => x.FirstName.Contains(searchString)),
                "Email" => baseQuery.Where(x => x.Email.Contains(searchString)),
                "LastName" => baseQuery.Where(x => x.LastName.Contains(searchString)),
                "City" => baseQuery.Where(x => x.City.Contains(searchString)),
                "PostalCode" => baseQuery.Where(x => x.PostalCode.Contains(searchString)),
                "Address" => baseQuery.Where(x => x.Address.Contains(searchString)),
                "PhoneNumber" => baseQuery.Where(x => x.PhoneNumber.Contains(searchString)),
                _ => emptyQuery,
            };
            var paginatedVM = await _customerPaginationService.CreateAsync(query, pageNumber, pageSize);
            return new ListCustomerVM
            {
                Customers = paginatedVM.Items,
                TotalPages = paginatedVM.TotalPages,
                CurrentPage = paginatedVM.CurrentPage,
            };
        }

        public async Task<ListOrderForListVM> SearchSelectedOrdersAsync(string selectedValue, string searchString, int pageSize, int pageNumber)
        {
            var idParse = int.TryParse(searchString, out int id);
            var priceParse = decimal.TryParse(searchString, out decimal price);
            var emptyQuery = Enumerable.Empty<OrderForListVM>().AsQueryable();
            var baseQuery = _orderRepository.GetAllOrders().ProjectTo<OrderForListVM>(_mapper.ConfigurationProvider);

            IQueryable<OrderForListVM> query = selectedValue switch
            {
                "Id" => idParse ? baseQuery.Where(x => x.Id == id) : emptyQuery,
                "CustomerId" => idParse ? baseQuery.Where(x => x.CustomerId == id) : emptyQuery,
                "Price" => priceParse ? baseQuery.Where(x => x.Price == price) : emptyQuery,
                "FirstName" => baseQuery.Where(x => x.ShipFirstName.Contains(searchString)),
                "Email" => baseQuery.Where(x => x.ShipContactEmail.Contains(searchString)),
                "LastName" => baseQuery.Where(x => x.ShipLastName.Contains(searchString)),
                "City" => baseQuery.Where(x => x.ShipCity.Contains(searchString)),
                "PostalCode" => baseQuery.Where(x => x.ShipPostalCode.Contains(searchString)),
                "Address" => baseQuery.Where(x => x.ShipAddress.Contains(searchString)),
                "PhoneNumber" => baseQuery.Where(x => x.ShipContactPhone.Contains(searchString)),
                _ => emptyQuery,
            };
            var paginatedVM = await _orderPaginationService.CreateAsync(query, pageNumber, pageSize);
            return new ListOrderForListVM
            {
                Orders = paginatedVM.Items,
                TotalPages = paginatedVM.TotalPages,
                CurrentPage = paginatedVM.CurrentPage,
            };
        }
    }
}
