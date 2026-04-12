// Application/System/MasterData/Profession/Commands/UpdateProfession.cs
using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Profession.Dtos;
using Application.System.MasterData.Profession.Validators;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.System.MasterData.Profession.Commands
{
    public static class UpdateProfession
    {
        public record Command(UpdateProfessionDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            private readonly IContextService _ContextService;
            private readonly ILocalizationService _localizer;

            public Validator(IContextService ContextService, ILocalizationService localizer)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new UpdateProfessionValidator(_localizer, _ContextService));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IProfessionRepository _repo;
            private readonly IContextService _ContextService;
            private readonly ILocalizationService _localizer;

            public Handler(
                IProfessionRepository repo,
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
                        _localizer.GetMessage("Profession", lang),
                        request.Data.Id));

                // التأكد أن المهنة تتبع الشركة الحالية
                if (entity.CompanyId != companyId)
                    throw new UnauthorizedAccessException("Access denied: Profession does not belong to your company");

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