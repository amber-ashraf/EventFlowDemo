using System.Collections.Generic;

namespace EventflowApi.Read.Dto
{
    public class EmployeeResponseModel
    {
        public string TenantId { get; set; }
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Department { get; set; }
    }
}
