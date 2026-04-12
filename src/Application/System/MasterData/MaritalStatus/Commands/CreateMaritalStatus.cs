using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.MaritalStatus.Dtos;
using Application.System.MasterData.MaritalStatus.Validators;
using Domain.System.MasterData;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.MaritalStatus.Commands
{
    public static class CreateMaritalStatus
    {
        public record Command(CreateMaritalStatusDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IContextService ContextService, ILocalizationService localizer)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new CreateMaritalStatusValidator(localizer, ContextService));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IMaritalStatusRepository _repo;
            private readonly IContextService _ContextService;
            private readonly ILocalizationService _localizer;

            public Handler(IMaritalStatusRepository repo, IContextService ContextService, ILocalizationService localizer)
            {
                _repo = repo;
                _ContextService = ContextService;
                _localizer = localizer;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var lang = _ContextService.GetCurrentLanguage();

                if (await _repo.CodeExistsAsync(request.Data.Code))
                    throw new ConflictException(string.Format(
                        _localizer.GetMessage("CodeExists", lang),
                        _localizer.GetMessage("MaritalStatus", lang),
                        request.Data.Code));

                var entity = new Domain.System.MasterData.MaritalStatus(
                    request.Data.Code,
                    request.Data.EngName,
                    request.Data.ArbName,
                    request.Data.ArbName4S,
                    request.Data.Remarks,
                    request.Data.RegUserId,
                    request.Data.regComputerId
                );

                await _repo.AddAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}