using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Department.Dtos;
using Application.System.MasterData.Department.Validators;
using Domain.System.MasterData;
using FluentValidation;
using MediatR;
using System.ComponentModel.Design;

namespace Application.System.MasterData.Department.Commands
{
    public static class CreateDepartment
    {
        public record Command(
            int CompanyId,
            int? RegUserId,
            CreateDepartmentDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IContextService contextService, ILocalizationService localizer)
            {
                var lang = contextService.GetCurrentLanguage();

              
                RuleFor(x => x.Data)
                    .SetValidator(new CreateDepartmentValidator(localizer, contextService));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IDepartmentRepository _repo;
            private readonly ICompanyRepository _companyRepo;
            private readonly ICodeGenerationService _codeGenerationService;
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;

            public Handler(
                IDepartmentRepository repo,
                ICompanyRepository companyRepo,
                ICodeGenerationService codeGenerationService,
                IContextService contextService,
                ILocalizationService localizer)
            {
                _repo = repo;
                _companyRepo = companyRepo;
                _codeGenerationService = codeGenerationService;
                _contextService = contextService;
                _localizer = localizer;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var lang = _contextService.GetCurrentLanguage();

                var company = await _companyRepo.GetByIdAsync(request.CompanyId);
            
                if (request.Data.ParentId.HasValue)
                {
                    var parent = await _repo.GetByIdAsync(request.Data.ParentId.Value);
                    if (parent == null)
                        throw new NotFoundException(
                            _localizer.GetMessage("ParentDepartment", lang),
                            request.Data.ParentId.Value,
                            string.Format(_localizer.GetMessage("NotFound", lang), _localizer.GetMessage("ParentDepartment", lang), request.Data.ParentId.Value));
                }

                var code = await _codeGenerationService.GenerateCodeAsync(
                    request.CompanyId,
                    request.Data.Code,
                    (companyId, ct) => _repo.GetMaxCodeAsync(companyId, ct),
                    (code, ct) => _repo.CodeExistsAsync(code, request.CompanyId),
                    cancellationToken
                );

                var entity = new Domain.System.MasterData.Department(
                    code: code,
                    companyId: request.CompanyId,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    parentId: request.Data.ParentId,
                    remarks: request.Data.Remarks,
                    costCenterCode: request.Data.CostCenterCode,
                    regUserId: request.RegUserId,
                    regComputerId: request.Data.RegComputerId
                );

                await _repo.AddAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}