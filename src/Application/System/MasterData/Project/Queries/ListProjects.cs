using Application.Abstractions;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Project.Dtos;
using MediatR;

namespace Application.System.MasterData.Project.Queries
{
    public static class ListProjects
    {
        public record Query : IRequest<List<ProjectDto>>;

        public class Handler : IRequestHandler<Query, List<ProjectDto>>
        {
            private readonly IProjectRepository _repo;
            private readonly ICurrentUser _currentUser;

            public Handler(IProjectRepository repo, ICurrentUser currentUser)
            {
                _repo = repo;
                _currentUser = currentUser;
            }

            public async Task<List<ProjectDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = _currentUser.Language;
                var companyId = _currentUser.CompanyId;

                var items = await _repo.GetByCompanyIdAsync();

                return items.Select(x => new ProjectDto(
                    Id: x.Id,
                    Code: x.Code,
                    EngName: x.EngName,
                    ArbName: x.ArbName,
                    ArbName4S: x.ArbName4S,
                    Phone: x.Phone,
                    Mobile: x.Mobile,
                    Fax: x.Fax,
                    Email: x.Email,
                    Adress: x.Adress,
                    ContactPerson: x.ContactPerson,
                    ProjectPeriod: x.ProjectPeriod,
                    ClaimDuration: x.ClaimDuration,
                    StartDate: x.StartDate,
                    EndDate: x.EndDate,
                    CreditLimit: x.CreditLimit,
                    CreditPeriod: x.CreditPeriod,
                    IsAdvance: x.IsAdvance,
                    IsHijri: x.IsHijri,
                    NotifyPeriod: x.NotifyPeriod,
                    CompanyConditions: x.CompanyConditions,
                    ClientConditions: x.ClientConditions,
                    IsLocked: x.IsLocked,
                    IsStoped: x.IsStoped,
                    BranchId: x.BranchId,
                    BranchName: lang == 2 ? x.Branch?.ArbName : x.Branch?.EngName,
                    CompanyId: x.CompanyId,
                    CompanyName: lang == 2 ? x.Company?.ArbName : x.Company?.EngName,
                    Remarks: x.Remarks,
                    WorkConditions: x.WorkConditions,
                    LocationId: x.LocationId,
                    LocationName: lang == 2 ? x.Location?.ArbName : x.Location?.EngName,
                    AbsentTransaction: x.AbsentTransaction,
                    LeaveTransaction: x.LeaveTransaction,
                    LateTransaction: x.LateTransaction,
                    SickTransaction: x.SickTransaction,
                    OTTransaction: x.OTTransaction,
                    HOTTransaction: x.HOTTransaction,
                    CostCenterCode1: x.CostCenterCode1,
                    DepartmentId: x.DepartmentId,
                    DepartmentName: lang == 2 ? x.Department?.ArbName : x.Department?.EngName,
                    CostCenterCode2: x.CostCenterCode2,
                    CostCenterCode3: x.CostCenterCode3,
                    CostCenterCode4: x.CostCenterCode4,
                    RegDate: x.RegDate,
                    CancelDate: x.CancelDate,
                    IsActive: x.IsActive()
                )).ToList();
            }
        }
    }
}