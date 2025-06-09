using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Application.CQRS.Queries;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Messaging;
using Infrastructure.Repositories;
using MediatR;

namespace Application.CQRS.Handlers
{
    public class GetPermissionsHandler(IPermissionRepository repository, IKafkaProducer kafkaProducer) : IRequestHandler<GetPermissionsQuery, IEnumerable<Permission>>
    {
        private readonly IPermissionRepository _repository = repository;
        private readonly IKafkaProducer _kafkaProducer = kafkaProducer;

        public async Task<IEnumerable<Permission>> Handle(GetPermissionsQuery request, CancellationToken cancellationToken)
        {
            await _kafkaProducer.SendMessageAsync("permissions-topic", $"Get requested");
            return await _repository.GetAllAsync();
        }
    }
}
