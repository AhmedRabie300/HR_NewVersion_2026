using Application.System.MasterData.Abstractions;
using Application.System.MasterData.ContractType.Dtos;
using Application.System.MasterData.ContractType.Validators;
using Application.Common.Abstractions;
using Domain.System.MasterData;
using FluentValidation;
using MediatR;
using Application.Common;

namespace Application.System.MasterData.ContractType.Commands
{
    public static class CreateContractType
    {
        public record Command(CreateContractTypeDto Data, int Lang = 1) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(ILocalizationService localizer)
            {
                RuleFor(x => x.Data)
                    .Custom((data, context) =>
                    {
                        var lang = context.InstanceToValidate.Lang;
                        var validator = new CreateContractTypeValidator(localizer, lang);
                        var result = validator.Validate(data);
                        if (!result.IsValid)
                        {
                            foreach (var error in result.Errors)
                            {
                                context.AddFailure(error);
                            }
                        }
                    });
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IContractTypeRepository _repo;
            private readonly ICompanyRepository _companyRepo;
            private readonly ILocalizationService _localizer;

            public Handler(IContractTypeRepository repo, ICompanyRepository companyRepo, ILocalizationService localizer)
            {
                _repo = repo;
                _companyRepo = companyRepo;
                _localizer = localizer;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var company = await _companyRepo.GetByIdAsync(request.Data.CompanyId);
                if (company == null)
                    throw new NotFoundException("Create Contract Type",string.Format(
                        _localizer.GetMessage("NotFound", request.Lang),
                        _localizer.GetMessage("Company", request.Lang),
                        request.Data.CompanyId));

                if (await _repo.CodeExistsAsync(request.Data.Code, request.Data.CompanyId))
                    throw new ConflictException(string.Format(
                        _localizer.GetMessage("CodeExists", request.Lang),
                        _localizer.GetMessage("ContractType", request.Lang),
                        request.Data.Code));

                var entity = new Domain.System.MasterData.ContractType(
                    request.Data.Code,
                    request.Data.CompanyId,
                    request.Data.EngName,
                    request.Data.ArbName,
                    request.Data.ArbName4S,
                    request.Data.IsSpecial,
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