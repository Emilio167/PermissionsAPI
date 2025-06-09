using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.CQRS.Commands;

namespace Application.Services
{
    public interface IPermissionService
    {
        Task RequestPermissionAsync(RequestPermissionCommand request);
    }
}
