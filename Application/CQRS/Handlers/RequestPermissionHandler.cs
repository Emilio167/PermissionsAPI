using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.CQRS.Commands;
using Application.Services;
using MediatR;

namespace Application.CQRS.Handlers
{
    public class RequestPermissionHandler(IPermissionService service) : IRequestHandler<RequestPermissionCommand>
    {
        private readonly IPermissionService _service = service;

        public async Task<Unit> Handle(RequestPermissionCommand request, CancellationToken cancellationToken)
        {
            await _service.RequestPermissionAsync(request);
            return Unit.Value;
        }
    }
}
