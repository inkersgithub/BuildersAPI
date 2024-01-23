using InkersCore.Domain.IRepositories;
using InkersCore.Domain.IServices;
using InkersCore.Infrastructure.Configurations;
using InkersCore.Models.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace InkersCore.Infrastructure
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly AppDBContext _context;
        private readonly ILoggerService<CompanyRepository> _loggerService;

        public CompanyRepository(AppDBContext context, ILoggerService<CompanyRepository> loggerService)
        {
            _loggerService = loggerService;
            _context = context;
        }

        public List<CompanyServiceMapping> GetCompanyServiceMappings(Company company)
        {
            return _context.CompanyServiceMappings.Where(x => !x.IsDeleted && x.IsActive && x.Company == company && !x.Service.IsDeleted)
                .Include(x => x.Service)
                .ToList();
        }

        public Company GetCompanyById(long? id)
        {
            return _context.Companys
                .Where(x => x.Id == id && x.IsActive && !x.IsDeleted)
                .Include(x => x.UserAccount)
                .FirstOrDefault();
        }

        public List<Company> GetCompanyListByService(long serviceId)
        {
            var mappingCompanyIds = _context.CompanyServiceMappings
                .Where(x => x.Service.Id == serviceId).Select(x => x.Company.Id).ToArray();
            var companies = _context.Companys.Where(x => mappingCompanyIds.Contains(x.Id))
                .Include(x => x.CompanyServices).ThenInclude(x=>x.Service)
                .ToList();
            return companies; 
        }
    }
}
