using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Religion.Dtos;
using FluentValidation;
using MediatR;
using Application.Abstractions;

namespace Application.System.MasterData.Religion.Queries
{
    public static class GetReligionById
    {
        public record Query(int Id) : IRequest<ReligionDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            public Validator(IValidationMessages msg)
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage(msg.Get("IdGreaterThanZero"));
            }
        }

        public class Handler : IRequestHandler<Query, ReligionDto>
        {
            private readonly IReligionRepository _repo;
                        private readonly IValidationMessages _msg;
public Handler(IReligionRepository repo, IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<ReligionDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var religion = await _repo.GetByIdAsync(request.Id);
                if (religion == null)
                    throw new NotFoundException(_msg.NotFound("Religion", request.Id));

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