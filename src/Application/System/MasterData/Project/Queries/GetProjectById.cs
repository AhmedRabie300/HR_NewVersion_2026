using Application.Abstractions;
using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Project.Dtos;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Project.Queries
{
    public static class GetProjectById
    {
        public record Query(int Id) : IRequest<ProjectDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            private readonly ICurrentUser _currentUser;

            public Validator(ICurrentUser currentUser)
            {
                _currentUser = currentUser;

                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage(x => _currentUser.Language == 2 ? "المعرف يجب أن يكون أكبر من 0" : "ID must be greater than 0");
            }
        }

        public class Handler : IRequestHandler<Query, ProjectDto>
        {
            private readonly IProjectRepository _repo;
            private readonly ICurrentUser _currentUser;

            public Handler(IProjectRepository repo, ICurrentUser currentUser)
            {
                _repo = repo;
                _currentUser = currentUser;
            }

            public async Task<ProjectDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = _currentUser.Language;

                var entity = await _repo.GetByIdAsync(request.Id);
 

                return new ProjectDto(
                    Id: entity.Id,
                    Code: entity.Code,
                    EngName: entity.EngName,
                    ArbName: entity.ArbName,
                    ArbName4S: entity.ArbName4S,
                    Phone: entity.Phone,
                    Mobile: entity.Mobile,
                    Fax: entity.Fax,
                    Email: entity.Email,
                    Adress: entity.Adress,
                    ContactPerson: entity.ContactPerson,
                    ProjectPeriod: entity.ProjectPeriod,
                    ClaimDuration: entity.ClaimDuration,
                    StartDate: entity.StartDate,
                    EndDate: entity.EndDate,
                    CreditLimit: entity.CreditLimit,
                    CreditPeriod: entity.CreditPeriod,
                    IsAdvance: entity.IsAdvance,
                    IsHijri: entity.IsHijri,
                    NotifyPeriod: entity.NotifyPeriod,
                    CompanyConditions: entity.CompanyConditions,
                    ClientConditions: entity.ClientConditions,
                    IsLocked: entity.IsLocked,
                    IsStoped: entity.IsStoped,
                    BranchId: entity.BranchId,
                    BranchName: lang == 2 ? entity.Branch?.ArbName : entity.Branch?.EngName,
                    CompanyId: entity.CompanyId,
                    CompanyName: lang == 2 ? entity.Company?.ArbName : entity.Company?.EngName,
                    Remarks: entity.Remarks,
                    WorkConditions: entity.WorkConditions,
                    LocationId: entity.LocationId,
                    LocationName: lang == 2 ? entity.Location?.ArbName : entity.Location?.EngName,
                    AbsentTransaction: entity.AbsentTransaction,
                    LeaveTransaction: entity.LeaveTransaction,
                    LateTransaction: entity.LateTransaction,
                    SickTransaction: entity.SickTransaction,
                    OTTransaction: entity.OTTransaction,
                    HOTTransaction: entity.HOTTransaction,
                    CostCenterCode1: entity.CostCenterCode1,
                    DepartmentId: entity.DepartmentId,
                    DepartmentName: lang == 2 ? entity.Department?.ArbName : entity.Department?.EngName,
                    CostCenterCode2: entity.CostCenterCode2,
                    CostCenterCode3: entity.CostCenterCode3,
                    CostCenterCode4: entity.CostCenterCode4,
                    RegDate: entity.RegDate,
                    CancelDate: entity.CancelDate,
                    IsActive: entity.IsActive()
                );
            }
        }
    }
}