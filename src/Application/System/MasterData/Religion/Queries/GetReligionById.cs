using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Religion.Dtos;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Religion.Queries
{
    public static class GetReligionById
    {
        public record Query(int Id) : IRequest<ReligionDto>;

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

        public class Handler : IRequestHandler<Query, ReligionDto>
        {
            private readonly IReligionRepository _repo;
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;

            public Handler(IReligionRepository repo, IContextService contextService, ILocalizationService localizer)
            {
                _repo = repo;
                _contextService = contextService;
                _localizer = localizer;
            }

            public async Task<ReligionDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = _contextService.GetCurrentLanguage();

                var religion = await _repo.GetByIdAsync(request.Id);
                if (religion == null)
                    throw new NotFoundException(
                        _localizer.GetMessage("Religion", lang),
                        request.Id,
                        string.Format(_localizer.GetMessage("NotFound", lang), _localizer.GetMessage("Religion", lang), request.Id));

                return new ReligionDto(
                    Id: religion.Id,
                    Code: religion.Code,
                    EngName: religion.EngName,
                    ArbName: religion.ArbName,
                    ArbName4S: religion.ArbName4S,
                    Remarks: religion.Remarks,
                    RegDate: religion.RegDate,
                    CancelDate: religion.CancelDate,
                    IsActive: religion.IsActive()
                );
            }
        }
    }
}