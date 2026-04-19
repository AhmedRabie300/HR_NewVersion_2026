using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Nationality.Dtos;
using FluentValidation;
using MediatR;
using Application.Abstractions;

namespace Application.System.MasterData.Nationality.Queries
{
    public static class GetNationalityById
    {
        public record Query(int Id) : IRequest<NationalityDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            public Validator(IValidationMessages msg)
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage(msg.Get("IdGreaterThanZero"));
            }
        }

        public class Handler : IRequestHandler<Query, NationalityDto>
        {
            private readonly INationalityRepository _repo;
                        private readonly IValidationMessages _msg;
public Handler(INationalityRepository repo, IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<NationalityDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var nationality = await _repo.GetByIdAsync(request.Id);
                if (nationality == null)
                    throw new NotFoundException(_msg.NotFound("Nationality", request.Id));

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