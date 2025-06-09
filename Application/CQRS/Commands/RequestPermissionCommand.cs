using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.CQRS.Commands
{
    public class RequestPermissionCommand : IRequest<Unit>
    {
        public string? EmployeeForename { get; set; }
        public string? EmployeeSurname { get; set; }
        public int PermissionTypeId { get; set; }
        public DateTime PermissionDate { get; set; }
    }
}
