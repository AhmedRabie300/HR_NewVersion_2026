using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Nationality.Dtos;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Nationality.Queries
{
    public static class GetNationalityById
    {
        public record Query(int Id) : IRequest<NationalityDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage("Nationality ID must be greater than 0");
            }
        }

        public class Handler : IRequestHandler<Query, NationalityDto>
        {
            private readonly INationalityRepository _repo;

            public Handler(INationalityRepository repo)
            {
                _repo = repo;
            }

            public async Task<NationalityDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var nationality = await _repo.GetByIdAsync(request.Id);
                if (nationality == null)
                    throw new Exception($"Nationality with ID {request.Id} not found");

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