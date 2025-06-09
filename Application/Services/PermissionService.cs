using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Application.CQRS.Commands;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Messaging;
using Infrastructure.Repositories;
using Infrastructure.Search;

namespace Application.Services
{
    public class PermissionService(IPermissionRepository repository, IKafkaProducer kafkaProducer, ElasticsearchService elasticsearchService): IPermissionService
    {
        private readonly IPermissionRepository _repository = repository;
        private readonly IKafkaProducer _kafkaProducer = kafkaProducer;
        private readonly ElasticsearchService _elasticsearchService = elasticsearchService;

        public async Task RequestPermissionAsync(RequestPermissionCommand command)
        {
            var permission = new Permission
            {
                EmployeeForename = command.EmployeeForename,
                EmployeeSurname = command.EmployeeSurname,
                PermissionTypeId = command.PermissionTypeId,
                PermissionDate = command.PermissionDate
            };

            await _repository.AddAsync(permission);
            await _kafkaProducer.SendMessageAsync("permissions-topic", $"Permission requested: {permission.Id}");
            await _elasticsearchService.IndexPermissionAsync(permission);
        }
    }
}