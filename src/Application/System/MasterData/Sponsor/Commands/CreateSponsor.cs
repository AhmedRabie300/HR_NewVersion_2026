using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Sponsor.Dtos;
using Application.System.MasterData.Sponsor.Validators;
using Application.Common.Abstractions;
using Domain.System.MasterData;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Sponsor.Commands
{
    public static class CreateSponsor
    {
        public record Command(CreateSponsorDto Data, int Lang = 1) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(ILocalizationService localizer)
            {
                RuleFor(x => x.Data)
                    .Custom((data, context) =>
                    {
                        var lang = context.InstanceToValidate.Lang;
                        var validator = new CreateSponsorValidator(localizer, lang);
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
            private readonly ISponsorRepository _repo;
            private readonly ICompanyRepository _companyRepo;
            private readonly ILocalizationService _localizer;

            public Handler(ISponsorRepository repo, ICompanyRepository companyRepo, ILocalizationService localizer)
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
                        _localizer.GetMessage("Sponsor", request.Lang),
                        request.Data.Code));

                var entity = new Domain.System.MasterData.Sponsor(
                    request.Data.Code,
                    request.Data.EngName,
                    request.Data.ArbName,
                    request.Data.ArbName4S,
                    request.Data.SponsorNumber,
                    request.Data.RegUserId,
                    request.Data.RegComputerId,
                    request.Data.CompanyId
                );

                await _repo.AddAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}