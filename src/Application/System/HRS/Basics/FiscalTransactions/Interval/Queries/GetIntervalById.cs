using Application.Abstractions;
using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.FiscalTransactions.Interval.Dtos;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.Basics.FiscalTransactions.Interval.Queries
{
    public static class GetIntervalById
    {
        public record Query(int Id) : IRequest<IntervalDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            private readonly ICurrentUser _currentUser;

            public Validator(ICurrentUser currentUser)
            {
                _currentUser = currentUser;

              
            }
        }

        public class Handler : IRequestHandler<Query, IntervalDto>
        {
            private readonly IIntervalRepository _repo;
            private readonly ICurrentUser _currentUser;

            public Handler(IIntervalRepository repo, ICurrentUser currentUser)
            {
                _repo = repo;
                _currentUser = currentUser;
            }

            public async Task<IntervalDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = _currentUser.Language;

                var entity = await _repo.GetByIdAsync(request.Id);
               
                return new IntervalDto(
                    Id: entity.Id,
                    Code: entity.Code,
                    EngName: entity.EngName,
                    ArbName: entity.ArbName,
                    ArbName4S: entity.ArbName4S,
                    Number: entity.Number,
                    CompanyId: entity.CompanyId,
                    Remarks: entity.Remarks,
                    RegDate: entity.RegDate,
                    CancelDate: entity.CancelDate,
                    IsActive: entity.IsActive()
                );
            }
        }
    }
}