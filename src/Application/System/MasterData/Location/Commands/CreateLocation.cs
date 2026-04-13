using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Location.Dtos;
using Application.System.MasterData.Location.Validators;
using Domain.System.MasterData;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Location.Commands
{
    public static class CreateLocation
    {
        public record Command(CreateLocationDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;

            public Validator(IContextService contextService, ILocalizationService localizer)
            {
                _contextService = contextService;
                _localizer = localizer;

                RuleFor(x => x.Data)
                    .SetValidator(new CreateLocationValidator(_localizer, _contextService));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly ILocationRepository _repo;
            private readonly ICompanyRepository _companyRepo;
            private readonly IBranchRepository _branchRepo;
            private readonly IDepartmentRepository _departmentRepo;
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;

            public Handler(
                ILocationRepository repo,
                ICompanyRepository companyRepo,
                IBranchRepository branchRepo,
                IDepartmentRepository departmentRepo,
                IContextService contextService,
                ILocalizationService localizer)
            {
                _repo = repo;
                _companyRepo = companyRepo;
                _branchRepo = branchRepo;
                _departmentRepo = departmentRepo;
                _contextService = contextService;
                _localizer = localizer;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var companyId = _contextService.GetCurrentCompanyId();
                var lang = _contextService.GetCurrentLanguage();

                var company = await _companyRepo.GetByIdAsync(companyId);
                if (company == null)
                    throw new NotFoundException("Create Location", string.Format(
                        _localizer.GetMessage("NotFound", lang),
                        _localizer.GetMessage("Company", lang),
                        companyId));

                if (request.Data.BranchId.HasValue)
                {
                    var branch = await _branchRepo.GetByIdAsync(request.Data.BranchId.Value);
                    if (branch == null)
                        throw new NotFoundException("Create Location", string.Format(
                            _localizer.GetMessage("NotFound", lang),
                            _localizer.GetMessage("Branch", lang),
                            request.Data.BranchId));
                }

                if (request.Data.DepartmentId.HasValue)
                {
                    var department = await _departmentRepo.GetByIdAsync(request.Data.DepartmentId.Value);
                    if (department == null)
                        throw new NotFoundException("Create Location", string.Format(
                            _localizer.GetMessage("NotFound", lang),
                            _localizer.GetMessage("Department", lang),
                            request.Data.DepartmentId));
                }

                string code;

                if (company.HasSequence == true)
                {
                    var prefix = company.Prefix;
                    var separator = company.Separator ?? "-";
                    var sequenceLength = company.SequenceLength ?? 5;

                    var maxCode = await _repo.GetMaxCodeAsync(companyId, cancellationToken);
                    int lastNumber = 0;

                    if (!string.IsNullOrEmpty(maxCode))
                    {
                        if (maxCode.Contains(separator))
                        {
                            var lastPart = maxCode.Split(separator).Last();
                            int.TryParse(lastPart, out lastNumber);
                        }
                        else
                        {
                            int.TryParse(maxCode, out lastNumber);
                        }
                    }

                    var newNumber = lastNumber + 1;
                    var formattedNumber = newNumber.ToString($"D{sequenceLength}");
                    code = $"{prefix}{separator}{formattedNumber}";
                }
                else
                {
                    code = request.Data.Code;

                    if (string.IsNullOrWhiteSpace(code))
                        throw new Exception(_localizer.GetMessage("CodeRequired", lang));

                    if (await _repo.CodeExistsAsync(code, companyId))
                        throw new ConflictException(string.Format(
                            _localizer.GetMessage("CodeExists", lang),
                            _localizer.GetMessage("Location", lang),
                            code));
                }

                var entity = new Domain.System.MasterData.Location(
                    code: code,
                    companyId: companyId,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    cityId: request.Data.CityId,
                    branchId: request.Data.BranchId,
                    storeId: request.Data.StoreId,
                    departmentId: request.Data.DepartmentId,
                    remarks: request.Data.Remarks,
                    costCenterCode1: request.Data.CostCenterCode1,
                    costCenterCode2: request.Data.CostCenterCode2,
                    costCenterCode3: request.Data.CostCenterCode3,
                    costCenterCode4: request.Data.CostCenterCode4,
                    regUserId: request.Data.RegUserId,
                    regComputerId: request.Data.regComputerId
                );

                await _repo.AddAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}