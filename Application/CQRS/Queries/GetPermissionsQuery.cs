using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using MediatR;

namespace Application.CQRS.Queries
{
    public class GetPermissionsQuery : IRequest<IEnumerable<Permission>>
    {
      
    }
}
