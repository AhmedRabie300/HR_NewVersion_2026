using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Sponsor.Dtos;
using Application.Common.Abstractions;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Sponsor.Queries
{
    public static class GetSponsorById
    {
        public record Query(int Id, int Lang = 1) : IRequest<SponsorDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            public Validator(ILocalizationService localizer)
            {
                RuleFor(x => x.Id).GreaterThan(0).WithMessage(localizer.GetMessage("IdGreaterThanZero", 1));
            }
        }

        public class Handler : IRequestHandler<Query, SponsorDto>
        {
            private readonly ISponsorRepository _repo;

            public Handler(ISponsorRepository repo)
            {
                _repo = repo;
            }

            public async Task<SponsorDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var entity = await _repo.GetByIdAsync(request.Id);
                if (entity == null)
                    throw new Exception($"Sponsor with ID {request.Id} not found");

                return new SponsorDto(
                    entity.Id,
                    entity.Code,
                    entity.EngName,
                    entity.ArbName,
                    entity.ArbName4S,
                    entity.SponsorNumber,
                    entity.CompanyId,
                    entity.Company?.EngName ?? entity.Company?.ArbName,
                    entity.RegDate,
                    entity.CancelDate,
                    entity.IsActive()
                );
            }
        }
    }
}