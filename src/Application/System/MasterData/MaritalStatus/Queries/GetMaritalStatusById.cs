using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.MaritalStatus.Dtos;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.MaritalStatus.Queries
{
    public static class GetMaritalStatusById
    {
        public record Query(int Id) : IRequest<MaritalStatusDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            private readonly ILocalizationService _localizer;
            private readonly IContextService _contextService;

            public Validator(ILocalizationService localizer, IContextService contextService)
            {
                _localizer = localizer;
                _contextService = contextService;

                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage(x => _localizer.GetMessage("IdGreaterThanZero", _contextService.GetCurrentLanguage()));
            }
        }

        public class Handler : IRequestHandler<Query, MaritalStatusDto>
        {
            private readonly IMaritalStatusRepository _repo;
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;

            public Handler(IMaritalStatusRepository repo, IContextService contextService, ILocalizationService localizer)
            {
                _repo = repo;
                _contextService = contextService;
                _localizer = localizer;
            }

            public async Task<MaritalStatusDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = _contextService.GetCurrentLanguage();

                var entity = await _repo.GetByIdAsync(request.Id);
                if (entity == null)
                    throw new NotFoundException(
                        _localizer.GetMessage("MaritalStatus", lang),
                        request.Id,
                        string.Format(_localizer.GetMessage("NotFound", lang), _localizer.GetMessage("MaritalStatus", lang), request.Id));

                return new MaritalStatusDto(
                    Id: entity.Id,
                    Code: entity.Code,
                    EngName: entity.EngName,
                    ArbName: entity.ArbName,
                    ArbName4S: entity.ArbName4S,
                    Remarks: entity.Remarks,
                    RegDate: entity.RegDate,
                    CancelDate: entity.CancelDate,
                    IsActive: entity.IsActive()
                );
            }
        }
    }
}