using Application.System.MasterData.Abstractions;
using Application.System.MasterData.DependantType.Dtos;
using Application.Common.Abstractions;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.DependantType.Queries
{
    public static class GetDependantTypeById
    {
        public record Query(int Id, int Lang = 1) : IRequest<DependantTypeDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            public Validator(ILocalizationService localizer)
            {
                RuleFor(x => x.Id).GreaterThan(0).WithMessage(localizer.GetMessage("IdGreaterThanZero", 1));
            }
        }

        public class Handler : IRequestHandler<Query, DependantTypeDto>
        {
            private readonly IDependantTypeRepository _repo;

            public Handler(IDependantTypeRepository repo)
            {
                _repo = repo;
            }

            public async Task<DependantTypeDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var entity = await _repo.GetByIdAsync(request.Id);
                if (entity == null)
                    throw new Exception($"DependantType with ID {request.Id} not found");

                return new DependantTypeDto(
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