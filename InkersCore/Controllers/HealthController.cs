using InkersCore.Domain.IRepositories;
using InkersCore.Infrastructure;
using InkersCore.Infrastructure.Configurations;
using InkersCore.Models.AuthEntityModels;
using InkersCore.Models.EntityModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InkersCore.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class HealthController : Controller
    {
        private readonly AppDBContext _dbContext;
        private IGenericRepository<Service> _serviceGenericRepository;

        public HealthController(AppDBContext context, IGenericRepository<Service> serviceGenericRepository)
        {
            _dbContext = context;
            _serviceGenericRepository = serviceGenericRepository;
        }

        [HttpGet]
        public IActionResult StatusCheck()
        {
            return Ok("Ok");
        }

        [HttpPost]
        public IActionResult InitialRun()
        {
            //Truncate all table
            var tableNames = _dbContext.Model.GetEntityTypes()
                            .Select(t => t.GetTableName())
                            .Distinct()
                            .ToList();
            foreach (var tableName in tableNames)
            {
                _dbContext.Database.ExecuteSqlRaw($"SET FOREIGN_KEY_CHECKS = 0; TRUNCATE TABLE `{tableName}`;");
            }

            //Create new Users
            var userSysytem = new Models.AuthEntityModels.UserAccount()
            {
                Name = "System",
                Password = "System",
                Email = "system@inkers.in",
                Phone = "xxxxxxxxxx",
            };
            _dbContext.UserAccounts.Add(userSysytem);
            var userAnoop = new Models.AuthEntityModels.UserAccount()
            {
                Name = "Anoop",
                Password = "Anoop",
                Email = "anoop@inkers.in",
                Phone = "9746364612",
            };
            _dbContext.UserAccounts.Add(userAnoop);
            var userMubarak = new Models.AuthEntityModels.UserAccount()
            {
                Name = "Mubarak",
                Password = "Mubarak",
                Email = "mubarak@inkers.in",
                Phone = "9567474709"
            };
            _dbContext.UserAccounts.Add(userMubarak);
            var userKarthik = new Models.AuthEntityModels.UserAccount()
            {
                Name = "Karthik",
                Password = "Karthik",
                Email = "karthik@inkers.in",
                Phone = "9645634119"
            };
            _dbContext.UserAccounts.Add(userKarthik);
            var userAkash = new Models.AuthEntityModels.UserAccount()
            {
                Name = "Akash K Manoj",
                Password = "Akash",
                Email = "akash@inkers.in",
                Phone = "8592955009"
            };
            _dbContext.UserAccounts.Add(userAkash);
            _dbContext.SaveChanges();

            //Create new User Groups
            var adminGroup = new UserGroup()
            {
                Name = "Admin",
                CreatedBy = userAnoop,
                LastUpdatedBy = userAnoop,
            };
            _dbContext.UserGroups.Add(adminGroup);
            var companyGroup = new UserGroup()
            {
                Name = "Company",
                CreatedBy = userAnoop,
                LastUpdatedBy = userAnoop,
            };
            _dbContext.UserGroups.Add(adminGroup);
            _dbContext.SaveChanges();

            //Create new User Group mappings
            List<UserGroupMapping> userGroupMappingList = new List<UserGroupMapping>()
            {
                new UserGroupMapping() {
                    UserAccount = userAnoop,
                    UserGroup = adminGroup,
                    CreatedBy = userAnoop,
                    LastUpdatedBy = userAnoop,
                },
                new UserGroupMapping() {
                    UserAccount = userMubarak,
                    UserGroup = adminGroup,
                    CreatedBy = userAnoop,
                    LastUpdatedBy = userAnoop,
                },
                new UserGroupMapping() {
                    UserAccount = userKarthik,
                    UserGroup = companyGroup,
                    CreatedBy = userAnoop,
                    LastUpdatedBy = userAnoop,
                },
                new UserGroupMapping() {
                    UserAccount = userAkash,
                    UserGroup = companyGroup,
                    CreatedBy = userAnoop,
                    LastUpdatedBy = userAnoop,
                }
            };
            _dbContext.UserGroupMappings.AddRange(userGroupMappingList);
            _dbContext.SaveChanges();

            //Create new User Permission
            var usrPermission = new UserPermission()
            {
                Name = "User Management",
                Code = "USR",
                CreatedBy = userAnoop,
                LastUpdatedBy = userAnoop,
            };
            _dbContext.UserPermissions.Add(usrPermission);
            var serPermission = new UserPermission()
            {
                Name = "Service Management",
                Code = "SER",
                CreatedBy = userAnoop,
                LastUpdatedBy = userAnoop,
            };
            _dbContext.UserPermissions.Add(serPermission);
            var comPermission = new UserPermission()
            {
                Name = "Company Management",
                Code = "COM",
                CreatedBy = userAnoop,
                LastUpdatedBy = userAnoop,
            };
            _dbContext.UserPermissions.Add(comPermission);
            var ordPermission = new UserPermission()
            {
                Name = "Order Management",
                Code = "ORD",
                CreatedBy = userAnoop,
                LastUpdatedBy = userAnoop,
            };
            _dbContext.UserPermissions.Add(ordPermission);
            var subPermission = new UserPermission()
            {
                Name = "Subscription Management",
                Code = "SUB",
                CreatedBy = userAnoop,
                LastUpdatedBy = userAnoop,
            };
            _dbContext.UserPermissions.Add(ordPermission);
            _dbContext.SaveChanges();

            List<UserGroupPermissionMapping> permissionMappingList = new List<UserGroupPermissionMapping>()
            {
                new UserGroupPermissionMapping()
                {
                    UserGroup = adminGroup,
                    Permission = usrPermission,
                    Add = true,
                    Remove = true,
                    Approve = true,
                    View = true,
                    Update = true,
                    CreatedBy= userAnoop,
                    LastUpdatedBy= userAnoop
                },
                new UserGroupPermissionMapping()
                {
                    UserGroup = adminGroup,
                    Permission = ordPermission,
                    Add = true,
                    Remove = true,
                    Approve = true,
                    View = true,
                    Update = true,
                    CreatedBy= userAnoop,
                    LastUpdatedBy= userAnoop
                },
                new UserGroupPermissionMapping()
                {
                    UserGroup = adminGroup,
                    Permission = serPermission,
                    Add = true,
                    Remove = true,
                    Approve = true,
                    View = true,
                    Update = true,
                    CreatedBy= userAnoop,
                    LastUpdatedBy= userAnoop
                },
                new UserGroupPermissionMapping()
                {
                    UserGroup = adminGroup,
                    Permission = comPermission,
                    Add = true,
                    Remove = true,
                    Approve = true,
                    View = true,
                    Update = true,
                    CreatedBy= userAnoop,
                    LastUpdatedBy= userAnoop
                },
                new UserGroupPermissionMapping()
                {
                    UserGroup = companyGroup,
                    Permission = comPermission,
                    Add = true,
                    Remove = true,
                    Approve = true,
                    View = true,
                    Update = true,
                    CreatedBy= userAnoop,
                    LastUpdatedBy= userAnoop
                }
            };
            _dbContext.UserGroupPermissionMappings.AddRange(permissionMappingList);
            _dbContext.SaveChanges();

            var serviceList = new List<Service>
            {
                new Service()
                {
                    Name = "2D/3D Plans",
                    CreatedBy = userAnoop,
                    LastUpdatedBy = userAnoop,
                },
                new Service()
                {
                    Name = "Doors",
                    CreatedBy = userAnoop,
                    LastUpdatedBy = userAnoop,
                },
                new Service()
                {
                    Name = "Windows",
                    CreatedBy = userAnoop,
                    LastUpdatedBy = userAnoop,
                },
                new Service()
                {
                    Name = "Kitchens",
                    CreatedBy = userAnoop,
                    LastUpdatedBy = userAnoop,
                },
                new Service()
                {
                    Name = "Pools",
                    CreatedBy = userAnoop,
                    LastUpdatedBy = userAnoop,
                }
            };

            _serviceGenericRepository.InsertRange(serviceList);

            return Ok("Intial Run completed");
        }
    }
}
