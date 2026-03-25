using Application.System.MasterData.Abstractions;
using Application.System.MasterData.ContractType.Dtos;
using Application.Common.Abstractions;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.ContractType.Queries
{
    public static class GetContractTypeById
    {
        public record Query(int Id, int Lang = 1) : IRequest<ContractTypeDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            public Validator(ILocalizationService localizer)
            {
                RuleFor(x => x.Id).GreaterThan(0).WithMessage(localizer.GetMessage("IdGreaterThanZero", 1));
            }
        }

        public class Handler : IRequestHandler<Query, ContractTypeDto>
        {
            private readonly IContractTypeRepository _repo;

            public Handler(IContractTypeRepository repo)
            {
                _repo = repo;
            }

            public async Task<ContractTypeDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var entity = await _repo.GetByIdAsync(request.Id);
                if (entity == null)
                    throw new Exception($"ContractType with ID {request.Id} not found");

                return new ContractTypeDto(
                    entity.Id,
                    entity.Code,
                    entity.CompanyId,
                    entity.Company?.EngName ?? entity.Company?.ArbName,
                    entity.EngName,
                    entity.ArbName,
                    entity.ArbName4S,
                    entity.IsSpecial,
                    entity.Remarks,
                    entity.RegDate,
                    entity.CancelDate,
                    entity.IsActive()
                );
            }
        }
    }
}