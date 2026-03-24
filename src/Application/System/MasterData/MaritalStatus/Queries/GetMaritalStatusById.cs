using Application.System.MasterData.Abstractions;
using Application.System.MasterData.MaritalStatus.Dtos;
using Application.Common.Abstractions;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.MaritalStatus.Queries
{
    public static class GetMaritalStatusById
    {
        public record Query(int Id, int Lang = 1) : IRequest<MaritalStatusDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            public Validator(ILocalizationService localizer)
            {
                RuleFor(x => x.Id).GreaterThan(0).WithMessage(localizer.GetMessage("IdGreaterThanZero", 1));
            }
        }

        public class Handler : IRequestHandler<Query, MaritalStatusDto>
        {
            private readonly IMaritalStatusRepository _repo;

            public Handler(IMaritalStatusRepository repo)
            {
                _repo = repo;
            }

            public async Task<MaritalStatusDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var entity = await _repo.GetByIdAsync(request.Id);
                if (entity == null)
                    throw new Exception($"MaritalStatus with ID {request.Id} not found");

                return new MaritalStatusDto(
                    entity.Id,
                    entity.Code,
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