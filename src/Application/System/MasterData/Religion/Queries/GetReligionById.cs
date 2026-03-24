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
            public Validator()
            {
                RuleFor(x => x.Id).GreaterThan(0).WithMessage("Religion ID must be greater than 0");
            }
        }

        public class Handler : IRequestHandler<Query, ReligionDto>
        {
            private readonly IReligionRepository _repo;

            public Handler(IReligionRepository repo)
            {
                _repo = repo;
            }

            public async Task<ReligionDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var religion = await _repo.GetByIdAsync(request.Id);
                if (religion == null)
                    throw new Exception($"Religion with ID {request.Id} not found");

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