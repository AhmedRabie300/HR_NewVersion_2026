// Application/System/MasterData/Location/Commands/UpdateLocation.cs
using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Location.Dtos;
using Application.System.MasterData.Location.Validators;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Application.Abstractions;

namespace Application.System.MasterData.Location.Commands
{
    public static class UpdateLocation
    {
        public record Command(UpdateLocationDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, ILocationRepository repo)
            {
                RuleFor(x => x.Data).SetValidator(new UpdateLocationValidator(msg, repo));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly ILocationRepository _repo;
                        private readonly IValidationMessages _msg;
            private readonly IContextService _ContextService;
private readonly ICompanyRepository _companyRepo;
            private readonly IBranchRepository _branchRepo;
            private readonly IDepartmentRepository _departmentRepo;
            public Handler(
                ILocationRepository repo, IValidationMessages msg,
                ICompanyRepository companyRepo,
                IBranchRepository branchRepo,
                IDepartmentRepository departmentRepo, IContextService ContextService)
            {
                _repo = repo;
                _msg = msg;
                _companyRepo = companyRepo;
                _branchRepo = branchRepo;
                _departmentRepo = departmentRepo;
                _ContextService = ContextService;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var companyId = _ContextService.GetCurrentCompanyId();
                var entity = await _repo.GetByIdAsync(request.Data.Id);
                if (entity == null)
                    throw new NotFoundException(_msg.NotFound("Location", request.Data.Id));

                 if (entity.CompanyId != companyId)
                    throw new UnauthorizedAccessException("Access denied: Location does not belong to your company");

                    entity.UpdateBasicInfo(
                        request.Data.EngName,
                        request.Data.ArbName,
                        request.Data.ArbName4S,
                        request.Data.Remarks,
                        request.Data.CostCenterCode1,
                        request.Data.CostCenterCode2,
                        request.Data.CostCenterCode3,
                        request.Data.CostCenterCode4
                    );

                 if (request.Data.ParentId.HasValue && request.Data.ParentId != entity.Id)
                {
                    var parent = await _repo.GetByIdAsync(request.Data.ParentId.Value);
                    if (parent == null)
                        throw new NotFoundException(_msg.NotFound("ParentLocation", request.Data.ParentId));
                 }

                 if (request.Data.BranchId.HasValue)
                {
                    var branch = await _branchRepo.GetByIdAsync(request.Data.BranchId.Value);
                    if (branch == null)
                        throw new NotFoundException(_msg.NotFound("Branch", request.Data.BranchId));
                 }

                await _repo.UpdateAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}