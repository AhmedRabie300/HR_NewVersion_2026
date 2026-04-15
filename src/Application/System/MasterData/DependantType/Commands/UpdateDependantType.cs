using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.DependantType.Dtos;
using Application.System.MasterData.DependantType.Validators;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.DependantType.Commands
{
    public static class UpdateDependantType
    {
        public record Command(UpdateDependantTypeDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;

            public Validator(IContextService contextService, ILocalizationService localizer)
            {
                _contextService = contextService;
                _localizer = localizer;

                RuleFor(x => x.Data)
                    .SetValidator(new UpdateDependantTypeValidator(_localizer, _contextService));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IDependantTypeRepository _repo;
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;

            public Handler(
                IDependantTypeRepository repo,
                IContextService contextService,
                ILocalizationService localizer)
            {
                _repo = repo;
                _contextService = contextService;
                _localizer = localizer;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var lang = _contextService.GetCurrentLanguage();
                var companyId = _contextService.GetCurrentCompanyId();

                var entity = await _repo.GetByIdAsync(request.Data.Id);
                if (entity == null)
                    throw new NotFoundException(
                        _localizer.GetMessage("DependantType", lang),
                        request.Data.Id,
                        string.Format(_localizer.GetMessage("NotFound", lang), _localizer.GetMessage("DependantType", lang), request.Data.Id));

                if (entity.CompanyId != companyId)
                    throw new UnauthorizedAccessException("Access denied: Dependant type does not belong to your company");

                entity.Update(
                    request.Data.EngName,
                    request.Data.ArbName,
                    request.Data.ArbName4S,
                    request.Data.Remarks
                );

                await _repo.UpdateAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}