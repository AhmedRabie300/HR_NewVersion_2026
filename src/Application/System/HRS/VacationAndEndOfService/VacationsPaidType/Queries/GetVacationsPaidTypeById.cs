using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.VacationAndEndOfService.VacationsPaidType.Dtos;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.VacationAndEndOfService.VacationsPaidType.Queries
{
    public static class GetVacationsPaidTypeById
    {
        public record Query(int Id) : IRequest<VacationsPaidTypeDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            private readonly ILocalizationService _localizer;
            private readonly IContextService _contextService;

            public Validator(ILocalizationService localizer, IContextService contextService)
            {
                _localizer = localizer;
                _contextService = contextService;

            }
        }

        public class Handler : IRequestHandler<Query, VacationsPaidTypeDto>
        {
            private readonly IVacationsPaidTypeRepository _repo;
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;
            private readonly IValidationMessages _msg;

            public Handler(IVacationsPaidTypeRepository repo, IContextService contextService, ILocalizationService localizer, IValidationMessages msg)
            {
                _repo = repo;
                _contextService = contextService;
                _localizer = localizer;
                _msg = msg;
            }

            public async Task<VacationsPaidTypeDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = _contextService.GetCurrentLanguage();

                var entity = await _repo.GetByIdAsync(request.Id);
        

                return new VacationsPaidTypeDto(
                    Id: entity.Id,
                    Code: entity.Code,
                    EngName: entity.EngName,
                    ArbName: entity.ArbName,
                    RegDate: entity.RegDate,
                    CancelDate: entity.CancelDate,
                    IsActive: entity.IsActive()
                );
            }
        }
    }
}