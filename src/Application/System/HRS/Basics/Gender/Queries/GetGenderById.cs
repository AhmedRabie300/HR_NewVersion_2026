using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.Gender.Dtos;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.Basics.Gender.Queries
{
    public static class GetGenderById
    {
        public record Query(int Id) : IRequest<GenderDto>;

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

        public class Handler : IRequestHandler<Query, GenderDto>
        {
            private readonly IGenderRepository _repo;
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;
            private readonly IValidationMessages _msg;

            public Handler(IGenderRepository repo, IContextService contextService, ILocalizationService localizer, IValidationMessages msg)
            {
                _repo = repo;
                _contextService = contextService;
                _localizer = localizer;
                _msg = msg;
            }

            public async Task<GenderDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = _contextService.GetCurrentLanguage();

                var entity = await _repo.GetByIdAsync(request.Id);
                if (entity == null)
                    throw new NotFoundException(_msg.NotFound("Gender", request.Id));


                return new GenderDto(
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