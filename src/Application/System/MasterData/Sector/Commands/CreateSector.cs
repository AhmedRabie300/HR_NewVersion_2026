using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Sector.Dtos;
using Application.System.MasterData.Sector.Validators;
using Domain.System.MasterData;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Sector.Commands
{
    public static class CreateSector
    {
        public record Command(CreateSectorDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;

            public Validator(IContextService contextService, ILocalizationService localizer)
            {
                _contextService = contextService;
                _localizer = localizer;

                RuleFor(x => x.Data)
                    .SetValidator(new CreateSectorValidator(_localizer, _contextService));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly ISectorRepository _repo;
            private readonly ICompanyRepository _companyRepo;
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;

            public Handler(
                ISectorRepository repo,
                ICompanyRepository companyRepo,
                IContextService contextService,
                ILocalizationService localizer)
            {
                _repo = repo;
                _companyRepo = companyRepo;
                _contextService = contextService;
                _localizer = localizer;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var companyId = _contextService.GetCurrentCompanyId();
                var lang = _contextService.GetCurrentLanguage();

                var company = await _companyRepo.GetByIdAsync(companyId);
                if (company == null)
                    throw new NotFoundException("Create Sector", string.Format(
                        _localizer.GetMessage("NotFound", lang),
                        _localizer.GetMessage("Company", lang),
                        companyId));

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
                            _localizer.GetMessage("Sector", lang),
                            code));
                }

                if (request.Data.ParentId.HasValue)
                {
                    var parent = await _repo.GetByIdAsync(request.Data.ParentId.Value);
                    if (parent == null)
                        throw new NotFoundException("Create Sector", string.Format(
                            _localizer.GetMessage("NotFound", lang),
                            _localizer.GetMessage("ParentSector", lang),
                            request.Data.ParentId));
                }

                var entity = new Domain.System.MasterData.Sector(
                    code: code,
                    companyId: companyId,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    parentId: request.Data.ParentId,
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