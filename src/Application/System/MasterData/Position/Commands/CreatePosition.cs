using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Position.Dtos;
using Application.System.MasterData.Position.Validators;
using Domain.System.MasterData;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Position.Commands
{
    public static class CreatePosition
    {
        public record Command(
            int CompanyId,
            int? RegUserId,
            CreatePositionDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IContextService contextService, ILocalizationService localizer)
            {
                var lang = contextService.GetCurrentLanguage();
 
                RuleFor(x => x.Data)
                    .SetValidator(new CreatePositionValidator(localizer, contextService));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IPositionRepository _repo;
            private readonly ICompanyRepository _companyRepo;
            private readonly ICodeGenerationService _codeGenerationService;
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;

            public Handler(
                IPositionRepository repo,
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
                            _localizer.GetMessage("ParentPosition", lang),
                            request.Data.ParentId.Value,
                            string.Format(_localizer.GetMessage("NotFound", lang), _localizer.GetMessage("ParentPosition", lang), request.Data.ParentId.Value));
                }

                var code = await _codeGenerationService.GenerateCodeAsync(
                    request.CompanyId,
                    request.Data.Code,
                    (companyId, ct) => _repo.GetMaxCodeAsync(companyId, ct),
                    (code, ct) => _repo.CodeExistsAsync(code),
                    cancellationToken
                );

                var entity = new Domain.System.MasterData.Position(
                    code: code,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    parentId: request.Data.ParentId,
                    positionLevelId: request.Data.PositionLevelId,
                    remarks: request.Data.Remarks,
                    employeesNo: request.Data.EmployeesNo,
                    applyValidation: request.Data.ApplyValidation,
                    positionBudget: request.Data.PositionBudget,
                    appraisalTypeGroupId: request.Data.AppraisalTypeGroupId,
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