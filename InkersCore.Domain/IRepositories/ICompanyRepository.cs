using InkersCore.Models.EntityModels;
namespace InkersCore.Domain.IRepositories
{
    public interface ICompanyRepository
    {
        List<CompanyServiceMapping> GetCompanyServiceMappings(Company company);
        Company? GetCompanyById(long? id);
        List<Company> GetCompanyListByService(long serviceId);
    }
}
