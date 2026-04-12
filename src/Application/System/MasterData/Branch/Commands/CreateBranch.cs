using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Branch.Dtos;
using Application.System.MasterData.Branch.Validators;
using Domain.System.MasterData;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.System.MasterData.Branch.Commands
{
    public static class CreateBranch
    {
        public record Command(CreateBranchDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            private readonly IContextService _ContextService;
            private readonly ILocalizationService _localizer;

            public Validator(IContextService ContextService, ILocalizationService localizer)
            {
                _ContextService = ContextService;
                _localizer = localizer;

                RuleFor(x => x.Data)
                    .SetValidator(new CreateBranchValidator(_localizer, _ContextService));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IBranchRepository _repo;
            private readonly ICompanyRepository _companyRepo;
            private readonly IContextService _ContextService;
            private readonly ILocalizationService _localizer;

            public Handler(
                IBranchRepository repo,
                ICompanyRepository companyRepo,
                IHttpContextAccessor httpContextAccessor,
                IContextService ContextService,
                ILocalizationService localizer)
            {
                _repo = repo;
                _companyRepo = companyRepo;
                _ContextService = ContextService;
                _localizer = localizer;
            }

           

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var companyId =_ContextService.GetCurrentCompanyId();
                var lang = _ContextService.GetCurrentLanguage();

                 var company = await _companyRepo.GetByIdAsync(companyId);
                if (company == null)
                    throw new NotFoundException("Create Branch", string.Format(
                        _localizer.GetMessage("NotFound", lang),
                        _localizer.GetMessage("Company", lang),
                        companyId));

                 if (await _repo.CodeExistsAsync(request.Data.Code, companyId))
                    throw new ConflictException(string.Format(
                        _localizer.GetMessage("CodeExists", lang),
                        _localizer.GetMessage("Branch", lang),
                        request.Data.Code));

                 if (request.Data.ParentId.HasValue)
                {
                    var parent = await _repo.GetByIdAsync(request.Data.ParentId.Value);
                    if (parent == null)
                        throw new NotFoundException("Create Branch", string.Format(
                            _localizer.GetMessage("NotFound", lang),
                            _localizer.GetMessage("ParentBranch", lang),
                            request.Data.ParentId));
                }

                var entity = new Domain.System.MasterData.Branch(
                    code: request.Data.Code,
                    companyId: companyId,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    parentId: request.Data.ParentId,
                    countryId: request.Data.CountryId,
                    cityId: request.Data.CityId,
                    defaultAbsent: request.Data.DefaultAbsent,
                    prepareDay: request.Data.PrepareDay,
                    affectPeriod: request.Data.AffectPeriod,
                    remarks: request.Data.Remarks,
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