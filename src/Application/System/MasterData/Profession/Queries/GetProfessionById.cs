using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Profession.Dtos;
using Application.Common.Abstractions;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Profession.Queries
{
    public static class GetProfessionById
    {
        public record Query(int Id, int Lang = 1) : IRequest<ProfessionDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            public Validator(ILocalizationService localizer)
            {
                RuleFor(x => x.Id).GreaterThan(0).WithMessage(localizer.GetMessage("IdGreaterThanZero", 1));
            }
        }

        public class Handler : IRequestHandler<Query, ProfessionDto>
        {
            private readonly IProfessionRepository _repo;

            public Handler(IProfessionRepository repo)
            {
                _repo = repo;
            }

            public async Task<ProfessionDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var entity = await _repo.GetByIdAsync(request.Id);
                if (entity == null)
                    throw new Exception($"Profession with ID {request.Id} not found");

                return new ProfessionDto(
                    entity.Id,
                    entity.Code,
                    entity.CompanyId,
                    entity.Company?.EngName ?? entity.Company?.ArbName,
                    entity.EngName,
                    entity.ArbName,
                    entity.ArbName4S,
                    entity.Remarks,
                    entity.RegDate,
                    entity.CancelDate,
                    entity.IsActive()
                );
            }
        }
    }
}