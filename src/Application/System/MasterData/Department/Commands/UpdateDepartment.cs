using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Department.Dtos;
using Application.System.MasterData.Department.Validators;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Department.Commands
{
    public static class UpdateDepartment
    {
        public record Command(UpdateDepartmentDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(ILanguageService languageService, ILocalizationService localizer)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new UpdateDepartmentValidator(localizer, languageService));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IDepartmentRepository _repo;
            private readonly ILanguageService _languageService;
            private readonly ILocalizationService _localizer;

            public Handler(IDepartmentRepository repo, ILanguageService languageService, ILocalizationService localizer)
            {
                _repo = repo;
                _languageService = languageService;
                _localizer = localizer;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var lang = _languageService.GetCurrentLanguage();

                var department = await _repo.GetByIdAsync(request.Data.Id);
                if (department == null)
                    throw new NotFoundException("Update Department", string.Format(
                        _localizer.GetMessage("NotFound", lang),
                        _localizer.GetMessage("Department", lang),
                        request.Data.Id));

                if (request.Data.EngName != null ||
                    request.Data.ArbName != null ||
                    request.Data.ArbName4S != null ||
                    request.Data.Remarks != null ||
                    request.Data.CostCenterCode != null)
                {
                    department.UpdateBasicInfo(
                        request.Data.EngName,
                        request.Data.ArbName,
                        request.Data.ArbName4S,
                        request.Data.Remarks,
                        request.Data.CostCenterCode
                    );
                }

                if (request.Data.ParentId.HasValue)
                {
                    if (request.Data.ParentId != department.Id)
                    {
                        var parent = await _repo.GetByIdAsync(request.Data.ParentId.Value);
                        if (parent == null)
                            throw new NotFoundException("Update Department", string.Format(
                                _localizer.GetMessage("NotFound", lang),
                                _localizer.GetMessage("ParentDepartment", lang),
                                request.Data.ParentId));

                        department.UpdateParent(request.Data.ParentId);
                    }
                }

                await _repo.UpdateAsync(department);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}