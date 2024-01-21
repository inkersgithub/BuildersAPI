using InkersCore.Domain.IRepositories;
using InkersCore.Domain.IServices;
using InkersCore.Infrastructure.Configurations;

namespace InkersCore.Infrastructure
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDBContext _context;
        private readonly ILoggerService<CustomerRepository> _loggerService;

        public CustomerRepository(AppDBContext context, ILoggerService<CustomerRepository> loggerService)
        {
            _context = context;
            _loggerService = loggerService;
        }
    }
}
