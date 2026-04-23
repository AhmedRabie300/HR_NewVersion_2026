using Application.Abstractions;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.HICompanies.Dtos;
using MediatR;

namespace Application.System.HRS.Basics.HICompanies.Queries
{
    public static class ListHICompanies
    {
        public record Query : IRequest<List<HICompanyDto>>;

        public class Handler : IRequestHandler<Query, List<HICompanyDto>>
        {
            private readonly IHICompanyRepository _repo;
            private readonly ICurrentUser _currentUser;

            public Handler(IHICompanyRepository repo, ICurrentUser currentUser)
            {
                _repo = repo;
                _currentUser = currentUser;
            }

            public async Task<List<HICompanyDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = _currentUser.Language;
                var companyId = _currentUser.CompanyId;

                var items = await _repo.GetByCompanyIdAsync(companyId);

                return items.Select(x => new HICompanyDto(
                    Id: x.Id,
                    Code: x.Code,
                    EngName: x.EngName,
                    ArbName: x.ArbName,
                    ArbName4S: x.ArbName4S,
                    CompanyId: x.CompanyId,
                    CompanyName: lang == 2 ? x.Company?.ArbName : x.Company?.EngName,
                    Remarks: x.Remarks,
                    RegDate: x.RegDate,
                    CancelDate: x.CancelDate,
                    IsActive: x.IsActive(),
                    Classes: x.Classes.Select(c => new HICompanyClassDto(
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
                    )).ToList()
                )).ToList();
            }
        }
    }
}