using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Department.Dtos;
using Application.System.MasterData.Department.Validators;
using Domain.System.MasterData;
using FluentValidation;
using MediatR;
using System.ComponentModel.Design;
using Application.Abstractions;

namespace Application.System.MasterData.Department.Commands
{
    public static class CreateDepartment
    {
        public record Command(CreateDepartmentDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg,IDepartmentRepository repo)
            {
                RuleFor(x => x.Data).SetValidator(new CreateDepartmentValidator(msg,repo));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IDepartmentRepository _repo;
            private readonly IValidationMessages _msg;
            public Handler(IDepartmentRepository repo, IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;

            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {

              

              

                var entity = new Domain.System.MasterData.Department(
                    code: request.Data.Code,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    parentId: request.Data.ParentId,
                    remarks: request.Data.Remarks,
                    costCenterCode: request.Data.CostCenterCode
                );

                await _repo.AddAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}