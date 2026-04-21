using Application.Abstractions;
using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Project.Dtos;
using Application.System.MasterData.Project.Validators;
using Domain.System.MasterData;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Project.Commands
{
    public static class UpdateProject
    {
        public record Command(UpdateProjectDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, IProjectRepository repo)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new UpdateProjectValidator(msg, repo));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IProjectRepository _repo;
            private readonly ICurrentUser _currentUser;
            private readonly IValidationMessages _msg;
            private readonly IBranchRepository _branchRepo;
            private readonly IDepartmentRepository _departmentRepo;
            private readonly ILocationRepository _locationRepo;

            public Handler(
                IProjectRepository repo,
                ICurrentUser currentUser,
                IValidationMessages msg,
                IBranchRepository branchRepo,
                IDepartmentRepository departmentRepo,
                ILocationRepository locationRepo)
            {
                _repo = repo;
                _currentUser = currentUser;
                _msg = msg;
                _branchRepo = branchRepo;
                _departmentRepo = departmentRepo;
                _locationRepo = locationRepo;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = await _repo.GetByIdAsync(request.Data.Id);
         

                entity.Update(
                    code: request.Data.Code,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    phone: request.Data.Phone,
                    mobile: request.Data.Mobile,
                    fax: request.Data.Fax,
                    email: request.Data.Email,
                    adress: request.Data.Adress,
                    contactPerson: request.Data.ContactPerson,
                    projectPeriod: request.Data.ProjectPeriod,
                    claimDuration: request.Data.ClaimDuration,
                    startDate: request.Data.StartDate,
                    endDate: request.Data.EndDate,
                    creditLimit: request.Data.CreditLimit,
                    creditPeriod: request.Data.CreditPeriod,
                    isAdvance: request.Data.IsAdvance,
                    isHijri: request.Data.IsHijri,
                    notifyPeriod: request.Data.NotifyPeriod,
                    companyConditions: request.Data.CompanyConditions,
                    clientConditions: request.Data.ClientConditions,
                    isLocked: request.Data.IsLocked,
                    isStoped: request.Data.IsStoped,
                    branchId: request.Data.BranchId,
                    remarks: request.Data.Remarks,
                    workConditions: request.Data.WorkConditions,
                    locationId: request.Data.LocationId,
                    absentTransaction: request.Data.AbsentTransaction,
                    leaveTransaction: request.Data.LeaveTransaction,
                    lateTransaction: request.Data.LateTransaction,
                    sickTransaction: request.Data.SickTransaction,
                    oTTransaction: request.Data.OTTransaction,
                    hOTTransaction: request.Data.HOTTransaction,
                    costCenterCode1: request.Data.CostCenterCode1,
                    departmentId: request.Data.DepartmentId,
                    costCenterCode2: request.Data.CostCenterCode2,
                    costCenterCode3: request.Data.CostCenterCode3,
                    costCenterCode4: request.Data.CostCenterCode4
                );

                await _repo.UpdateAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}