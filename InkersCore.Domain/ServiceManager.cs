using InkersCore.Domain.IRepositories;
using InkersCore.Domain.IServices;
using InkersCore.Models.EntityModels;
using InkersCore.Models.RequestModels;
using InkersCore.Models.ResponseModels;

namespace InkersCore.Domain
{
    public class ServiceManager
    {
        private readonly IGenericRepository<Service> _serviceGenericRepository;
        private readonly ILoggerService<ServiceManager> _loggerService;

        public ServiceManager(IGenericRepository<Service> serviceGenericRepository, ILoggerService<ServiceManager> loggerService)
        {
            _loggerService = loggerService;
            _serviceGenericRepository = serviceGenericRepository;
        }

        /// <summary>
        /// Function to get Service list
        /// </summary>
        /// <returns>CommonResponse</returns>
        public CommonResponse GetServiceList()
        {
            var response = new CommonResponse();
            try
            {
                response.Success = true;
                response.Result = _serviceGenericRepository.Find(new Models.EntityFilter<Service>()
                {
                    Predicate = x=>x.IsActive&& !x.IsDeleted
                });
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }
            return response;
        }
       
    }
}
