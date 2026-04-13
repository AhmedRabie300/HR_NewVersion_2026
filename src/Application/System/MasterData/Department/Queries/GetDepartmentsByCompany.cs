// Application/System/MasterData/Department/Queries/GetDepartmentsByCompany.cs
using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Department.Dtos;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Department.Queries
{
    public static class GetDepartmentsByCompany
    {
        public record Query : IRequest<List<DepartmentDto>>;

        public sealed class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                // لا توجد قواعد
            }
        }

        public class Handler : IRequestHandler<Query, List<DepartmentDto>>
        {
            private readonly IDepartmentRepository _repo;
            private readonly ICompanyRepository _companyRepo;
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;

            public Handler(
                IDepartmentRepository repo,
                ICompanyRepository companyRepo,
                IContextService contextService,
                ILocalizationService localizer)
            {
                _repo = repo;
                _companyRepo = companyRepo;
                _contextService = contextService;
                _localizer = localizer;
            }

            public async Task<List<DepartmentDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var companyId = _contextService.GetCurrentCompanyId();
                var lang = _contextService.GetCurrentLanguage();

                var company = await _companyRepo.GetByIdAsync(companyId);
                if (company == null)
                    throw new NotFoundException(
                        _localizer.GetMessage("Company", lang),
                        companyId,
                        string.Format(_localizer.GetMessage("NotFound", lang), _localizer.GetMessage("Company", lang), companyId));

                var departments = await _repo.GetByCompanyIdAsync(companyId);

                return departments.Select(d => new DepartmentDto(
                    Id: d.Id,
                    Code: d.Code,
                    CompanyId: d.CompanyId,
                    CompanyName: company.EngName ?? company.ArbName,
                    EngName: d.EngName,
                    ArbName: d.ArbName,
                    ArbName4S: d.ArbName4S,
                    ParentId: d.ParentId,
                    ParentDepartmentName: d.ParentDepartment?.EngName ?? d.ParentDepartment?.ArbName,
                    Remarks: d.Remarks,
                    CostCenterCode: d.CostCenterCode,
                    RegDate: d.RegDate,
                    CancelDate: d.CancelDate,
                    IsActive: d.IsActive()
                )).ToList();
            }
        }
    }
}