// Application/System/MasterData/Department/Commands/UpdateDepartment.cs
using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Department.Dtos;
using Application.System.MasterData.Department.Validators;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Application.Abstractions;

namespace Application.System.MasterData.Department.Commands
{
    public static class UpdateDepartment
    {
        public record Command(UpdateDepartmentDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg)
            {
                RuleFor(x => x.Data).SetValidator(new UpdateDepartmentValidator(msg));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IDepartmentRepository _repo;
                        private readonly IValidationMessages _msg;
            private readonly IContextService _ContextService;
public Handler(
                IDepartmentRepository repo, IValidationMessages msg,
                IHttpContextAccessor httpContextAccessor, IContextService ContextService)
            {
                _repo = repo;
                _msg = msg;
                _ContextService = ContextService;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
               // var companyId = _ContextService.GetCurrentCompanyId();

                var entity = await _repo.GetByIdAsync(request.Data.Id);
                //if (entity == null)
                //    throw new NotFoundException(_msg.NotFound("Department", request.Data.Id));

                 //if (entity.CompanyId != companyId)
                 //   throw new UnauthorizedAccessException("Access denied: Department does not belong to your company");

                    entity.UpdateBasicInfo(
                        request.Data.EngName,
                        request.Data.ArbName,
                        request.Data.ArbName4S,
                        request.Data.Remarks,
                        request.Data.CostCenterCode
                    );

                // Update parent
                if (request.Data.ParentId.HasValue && request.Data.ParentId != entity.Id)
                {
                    var parent = await _repo.GetByIdAsync(request.Data.ParentId.Value);
                    if (parent == null)
                        throw new NotFoundException(_msg.NotFound("ParentDepartment", request.Data.ParentId));

                    entity.UpdateParent(request.Data.ParentId);
                }

                await _repo.UpdateAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}