using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.TransactionsGroup.Dtos;
using Application.System.HRS.TransactionsGroup.Validators;
using Application.System.MasterData.Abstractions;
using Domain.System.HRS;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.TransactionsGroup.Commands
{
    public static class CreateTransactionsGroup
    {
        public record Command(CreateTransactionsGroupDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;
            private readonly ICompanyRepository _companyRepo;

            public Validator(
                IContextService contextService,
                ILocalizationService localizer,
                ICompanyRepository companyRepo)
            {
                _contextService = contextService;
                _localizer = localizer;
                _companyRepo = companyRepo;

                RuleFor(x => x.Data)
                    .CustomAsync(async (data, context, ct) =>
                    {
                        var companyId = _contextService.GetCurrentCompanyId();
                        var company = await _companyRepo.GetByIdAsync(companyId);
                        var lang = _contextService.GetCurrentLanguage();

                        if (company?.HasSequence != true)
                        {
                            if (string.IsNullOrWhiteSpace(data.Code))
                            {
                                context.AddFailure("Code", _localizer.GetMessage("CodeRequired", lang));
                            }
                            else if (data.Code.Length > 50)
                            {
                                context.AddFailure("Code", string.Format(_localizer.GetMessage("MaxLength", lang), 50));
                            }
                        }

                        var validator = new CreateTransactionsGroupValidator(_localizer, _contextService);
                        var result = await validator.ValidateAsync(data, ct);
                        foreach (var error in result.Errors)
                        {
                            context.AddFailure(error);
                        }
                    });
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly ITransactionsGroupRepository _repo;
            private readonly ICompanyRepository _companyRepo;
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;

            public Handler(
                ITransactionsGroupRepository repo,
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
                    throw new NotFoundException("Create TransactionsGroup", string.Format(
                        _localizer.GetMessage("NotFound", lang),
                        _localizer.GetMessage("Company", lang),
                        companyId));

                string code;

                if (company?.HasSequence == true)
                {
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
                    code = formattedNumber.ToString();
                }
                else
                {
                    code = request.Data.Code;

                    if (string.IsNullOrWhiteSpace(code))
                        throw new NotFoundException("CodeRequired", _localizer.GetMessage("CodeRequired", lang));

                    if (await _repo.CodeExistsAsync(code))
                        throw new ConflictException(string.Format(
                            _localizer.GetMessage("CodeExists", lang),
                            _localizer.GetMessage("TransactionsGroup", lang),
                            code));
                }

                var entity = new Domain.System.HRS.TransactionsGroup(
                    code: code,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    companyId: companyId,
                    remarks: request.Data.Remarks,
                    regUserId: request.Data.RegUserId,
                    regComputerId: request.Data.RegComputerId
                );

                await _repo.AddAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}