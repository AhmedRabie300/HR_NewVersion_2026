using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Currency.Dtos;
using Application.System.MasterData.Currency.Validators;
using Application.Common.Abstractions;
using Domain.System.MasterData;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Currency.Commands
{
    public static class CreateCurrency
    {
        public record Command(CreateCurrencyDto Data, int Lang = 1) : IRequest<int>;

   

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly ICurrencyRepository _repo;
            private readonly ICompanyRepository _companyRepo;
            private readonly ILocalizationService _localizer;

            public Handler(ICurrencyRepository repo, ICompanyRepository companyRepo, ILocalizationService localizer)
            {
                _repo = repo;
                _companyRepo = companyRepo;
                _localizer = localizer;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                if (request.Data.CompanyId.HasValue)
                {
                    var company = await _companyRepo.GetByIdAsync(request.Data.CompanyId.Value);
                    if (company == null)
                        throw new Exception(string.Format(
                            _localizer.GetMessage("NotFound", request.Lang),
                            _localizer.GetMessage("Company", request.Lang),
                            request.Data.CompanyId));
                }

                if (await _repo.CodeExistsAsync(request.Data.Code))
                    throw new Exception(string.Format(
                        _localizer.GetMessage("CodeExists", request.Lang),
                        _localizer.GetMessage("Currency", request.Lang),
                        request.Data.Code));

                var entity = new Domain.System.MasterData.Currency(
                    request.Data.Code,
                    request.Data.EngName,
                    request.Data.ArbName,
                    request.Data.ArbName4S,
                    request.Data.EngSymbol,
                    request.Data.ArbSymbol,
                    request.Data.DecimalFraction,
                    request.Data.DecimalEngName,
                    request.Data.DecimalArbName,
                    request.Data.Amount,
                    request.Data.NoDecimalPlaces,
                    request.Data.CompanyId,
                    request.Data.Remarks,
                    request.Data.RegUserId,
                    request.Data.RegComputerId
                );

                await _repo.AddAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}