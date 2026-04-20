using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Sponsor.Dtos;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Sponsor.Queries
{
    public static class GetSponsorById
    {
        public record Query(int Id) : IRequest<SponsorDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            public Validator(IValidationMessages msg)
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage(msg.Get("IdGreaterThanZero"));
            }
        }

        public class Handler : IRequestHandler<Query, SponsorDto>
        {
            private readonly ISponsorRepository _repo;
            private readonly IValidationMessages _msg;

            public Handler(
                ISponsorRepository repo,
                IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<SponsorDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var entity = await _repo.GetByIdAsync(request.Id);
                if (entity == null)
                    throw new NotFoundException(_msg.NotFound("Sponsor", request.Id));

                // التأكد أن الكفيل تابع للشركة الحالية

                return new SponsorDto(
                    Id: entity.Id,
                    Code: entity.Code,
                    EngName: entity.EngName,
                    ArbName: entity.ArbName,
                    ArbName4S: entity.ArbName4S,
                    SponsorNumber: entity.SponsorNumber,
                    RegDate: entity.RegDate,
                    CancelDate: entity.CancelDate,
                    IsActive: entity.IsActive()
                );
            }
        }
    }
}