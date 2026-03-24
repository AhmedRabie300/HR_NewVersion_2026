using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Education.Dtos;
using Application.Common.Abstractions;
using Domain.System.MasterData;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Education.Commands
{
    public static class CreateEducation
    {
        public record Command(CreateEducationDto Data, int Lang = 1) : IRequest<int>;

 
        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IEducationRepository _repo;
            private readonly ICompanyRepository _companyRepo;
            private readonly ILocalizationService _localizer;

            public Handler(IEducationRepository repo, ICompanyRepository companyRepo, ILocalizationService localizer)
            {
                _repo = repo;
                _companyRepo = companyRepo;
                _localizer = localizer;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var company = await _companyRepo.GetByIdAsync(request.Data.CompanyId);
                if (company == null)
                    throw new Exception(string.Format(
                        _localizer.GetMessage("NotFound", request.Lang),
                        _localizer.GetMessage("Company", request.Lang),
                        request.Data.CompanyId));

                if (await _repo.CodeExistsAsync(request.Data.Code, request.Data.CompanyId))
                    throw new Exception(string.Format(
                        _localizer.GetMessage("CodeExists", request.Lang),
                        _localizer.GetMessage("Education", request.Lang),
                        request.Data.Code));

                var entity = new Domain.System.MasterData.Education(
                    request.Data.Code,
                    request.Data.CompanyId,
                    request.Data.EngName,
                    request.Data.ArbName,
                    request.Data.ArbName4S,
                    request.Data.Level,
                    request.Data.RequiredYears,
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