using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.CQRS.Commands
{
    public class ModifyPermissionCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public int NewPermissionTypeId { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
