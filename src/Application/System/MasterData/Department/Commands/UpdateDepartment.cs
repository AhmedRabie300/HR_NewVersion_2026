// Application/System/MasterData/Department/Commands/UpdateDepartment.cs
using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Department.Dtos;
using Application.System.MasterData.Department.Validators;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.System.MasterData.Department.Commands
{
    public static class UpdateDepartment
    {
        public record Command(UpdateDepartmentDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            private readonly IContextService _ContextService;
            private readonly ILocalizationService _localizer;

            public Validator(IContextService ContextService, ILocalizationService localizer)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new UpdateDepartmentValidator(_localizer, _ContextService));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IDepartmentRepository _repo;
            private readonly IContextService _ContextService;
            private readonly ILocalizationService _localizer;

            public Handler(
                IDepartmentRepository repo,
                IHttpContextAccessor httpContextAccessor,
                IContextService ContextService,
                ILocalizationService localizer)
            {
                _repo = repo;
                _ContextService = ContextService;
                _localizer = localizer;
            }

 

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var companyId = _ContextService.GetCurrentCompanyId();

                var lang = _ContextService.GetCurrentLanguage();

                var entity = await _repo.GetByIdAsync(request.Data.Id);
                if (entity == null)
                    throw new Exception(string.Format(
                        _localizer.GetMessage("NotFound", lang),
                        _localizer.GetMessage("Department", lang),
                        request.Data.Id));

                 if (entity.CompanyId != companyId)
                    throw new UnauthorizedAccessException("Access denied: Department does not belong to your company");

             
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
                        throw new NotFoundException("Update Department", string.Format(
                            _localizer.GetMessage("NotFound", lang),
                            _localizer.GetMessage("ParentDepartment", lang),
                            request.Data.ParentId));

                    entity.UpdateParent(request.Data.ParentId);
                }

                await _repo.UpdateAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}