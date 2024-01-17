using InkersCore.Models.AuthEntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InkersCore.Models.RequestModels
{
    public class AddUpdateCompanyRequest:CommonRequest
    {
        public long? CompanyId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string ZipCode { get; set; }
        public int? IsApproved { get; set; } = 0;
        public long[] RequestedServiceIds { get; set; }
    }
}
