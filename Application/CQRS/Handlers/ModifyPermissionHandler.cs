using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.CQRS.Commands;
using Application.Services;
using Domain.Interfaces;
using Infrastructure.Messaging;
using Infrastructure.Repositories;
using MediatR;

namespace Application.CQRS.Handlers
{
    public class ModifyPermissionHandler(IPermissionRepository repository, IKafkaProducer kafkaProducer) : IRequestHandler<ModifyPermissionCommand>
    {
        private readonly IPermissionRepository _repository = repository;
        private readonly IKafkaProducer _kafkaProducer = kafkaProducer;

        public async Task<Unit> Handle(ModifyPermissionCommand request, CancellationToken cancellationToken)
        {
            var permission = await _repository.GetByIdAsync(request.Id);
            if (permission == null) return Unit.Value;

            permission.PermissionTypeId = request.NewPermissionTypeId;
            permission.PermissionDate = request.UpdatedDate;

            await _repository.UpdateAsync(permission);
            await _kafkaProducer.SendMessageAsync("permissions-topic", $"Modify requested: {permission.Id}");
            return Unit.Value;
        }
    }
}
