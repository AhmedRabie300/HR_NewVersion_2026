using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Nationality.Dtos;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Nationality.Queries
{
    public static class GetNationalityById
    {
        public record Query(int Id) : IRequest<NationalityDto>;

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

        public class Handler : IRequestHandler<Query, NationalityDto>
        {
            private readonly INationalityRepository _repo;
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;

            public Handler(INationalityRepository repo, IContextService contextService, ILocalizationService localizer)
            {
                _repo = repo;
                _contextService = contextService;
                _localizer = localizer;
            }

            public async Task<NationalityDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = _contextService.GetCurrentLanguage();

                var nationality = await _repo.GetByIdAsync(request.Id);
                if (nationality == null)
                    throw new NotFoundException(
                        _localizer.GetMessage("Nationality", lang),
                        request.Id,
                        string.Format(_localizer.GetMessage("NotFound", lang), _localizer.GetMessage("Nationality", lang), request.Id));

                return new NationalityDto(
                    Id: nationality.Id,
                    Code: nationality.Code,
                    EngName: nationality.EngName,
                    ArbName: nationality.ArbName,
                    ArbName4S: nationality.ArbName4S,
                    IsMainNationality: nationality.IsMainNationality,
                    TravelRoute: nationality.TravelRoute,
                    TravelClass: nationality.TravelClass,
                    Remarks: nationality.Remarks,
                    TicketAmount: nationality.TicketAmount,
                    RegDate: nationality.RegDate,
                    CancelDate: nationality.CancelDate,
                    IsActive: nationality.IsActive()
                );
            }
        }
    }
}