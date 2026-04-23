using Application.Abstractions;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.HICompanies.Dtos;
using MediatR;

namespace Application.System.HRS.Basics.HICompanies.Queries
{
    public static class GetHICompanyClasses
    {
        public record Query(int HICompanyId) : IRequest<List<HICompanyClassDto>>;

        public class Handler : IRequestHandler<Query, List<HICompanyClassDto>>
        {
            private readonly IHICompanyRepository _repo;
            private readonly ICurrentUser _currentUser;

            public Handler(IHICompanyRepository repo, ICurrentUser currentUser)
            {
                _repo = repo;
                _currentUser = currentUser;
            }

            public async Task<List<HICompanyClassDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = _currentUser.Language;
                var classes = await _repo.GetClassesByHICompanyIdAsync(request.HICompanyId);

                return classes.Select(c => new HICompanyClassDto(
                    Id: c.Id,
                    HICompanyId: c.HICompanyId,
                    CompanyId: c.CompanyId,
                    CompanyName: lang == 2 ? c.Company?.ArbName : c.Company?.EngName,
                    EngName: c.EngName,
                    ArbName: c.ArbName,
                    ArbName4S: c.ArbName4S,
                    Remarks: c.Remarks,
                    CompanyAmount: c.CompanyAmount,
                    EmployeeAmount: c.EmployeeAmount,
                    RegDate: c.RegDate,
                    CancelDate: c.CancelDate,
                    IsActive: c.IsActive()
                )).ToList();
            }
        }
    }
}