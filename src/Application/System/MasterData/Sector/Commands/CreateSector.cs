 using Application.Common;
using Application.Common.Abstractions;
using Application.Common.BaseHandlers;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Sector.Dtos;
using Application.System.MasterData.Sector.Validators;
using Domain.Common.Exceptions;
using Domain.System.MasterData;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Sector.Commands
{
    public static class CreateSector
    {
        public record Command(CreateSectorDto Data, int Lang = 1) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(ILocalizationService localization, int lang = 1)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new CreateSectorValidator(localization, lang));
            }
        }

        public class Handler : BaseCommandHandler, IRequestHandler<Command, int>
        {
            private readonly ISectorRepository _repo;
            private readonly ICompanyRepository _companyRepo;

            public Handler(
                ISectorRepository repo,
                ICompanyRepository companyRepo,
                ILocalizationService localization) : base(localization)
            {
                _repo = repo;
                _companyRepo = companyRepo;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                // Check if company exists
                var company = await _companyRepo.GetByIdAsync(request.Data.CompanyId);
                if (company == null)
                {
                    throw new NotFoundException(
                        GetMessage("Company", request.Lang),
                        request.Data.CompanyId,
                        GetFormattedMessage("NotFound", request.Lang, GetMessage("Company", request.Lang), request.Data.CompanyId)
                    );
                }

                // Check if code exists within the same company
                var exists = await _repo.CodeExistsAsync(request.Data.Code, request.Data.CompanyId);
                if (exists)
                {
                    throw new ConflictException(
                        GetMessage("Sector", request.Lang),
                        GetMessage("Code", request.Lang),
                        request.Data.Code
                    );
                }

                // Check if parent sector exists if provided
                if (request.Data.ParentId.HasValue)
                {
                    var parent = await _repo.GetByIdAsync(request.Data.ParentId.Value);
                    if (parent == null)
                    {
                        throw new NotFoundException(
                            GetMessage("ParentSector", request.Lang),
                            request.Data.ParentId.Value,
                            GetFormattedMessage("NotFound", request.Lang, GetMessage("ParentSector", request.Lang), request.Data.ParentId.Value)
                        );
                    }

                    // Verify parent belongs to same company
                    if (parent.CompanyId != request.Data.CompanyId)
                    {
                        throw new DomainException(GetFormattedMessage("ParentMustBeSameCompany", request.Lang));
                    }
                }

                var sector = new Domain.System.MasterData.Sector(
                    code: request.Data.Code,
                    companyId: request.Data.CompanyId,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    parentId: request.Data.ParentId,
                    remarks: request.Data.Remarks,
                    regUserId: request.Data.RegUserId,
                    regComputerId: request.Data.regComputerId
                );

                await _repo.AddAsync(sector);
                await _repo.SaveChangesAsync(cancellationToken);

                return sector.Id;
            }
        }
    }
}