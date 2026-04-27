using Application.Abstractions;
using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.FiscalYears.Dtos;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.Basics.FiscalYears.Queries
{
    public static class GetFiscalYearById
    {
        public record Query(int Id) : IRequest<FiscalYearDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            private readonly ICurrentUser _currentUser;

            public Validator(ICurrentUser currentUser)
            {
                _currentUser = currentUser;
             
            }
        }

        public class Handler : IRequestHandler<Query, FiscalYearDto>
        {
            private readonly IFiscalYearRepository _repo;
            private readonly ICurrentUser _currentUser;

            public Handler(IFiscalYearRepository repo, ICurrentUser currentUser)
            {
                _repo = repo;
                _currentUser = currentUser;
            }

            public async Task<FiscalYearDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = _currentUser.Language;

                var entity = await _repo.GetByIdAsync(request.Id);
                 

                return new FiscalYearDto(
                    Id: entity.Id,
                    Code: entity.Code,
                    EngName: entity.EngName,
                    ArbName: entity.ArbName,
                    ArbName4S: entity.ArbName4S,
                    CompanyId: entity.CompanyId,
                    CompanyName: lang == 2 ? entity.Company?.ArbName : entity.Company?.EngName,
                    Remarks: entity.Remarks,
                    RegDate: entity.RegDate,
                    CancelDate: entity.CancelDate,
                    IsActive: entity.IsActive()
                );
            }
        }
    }
}