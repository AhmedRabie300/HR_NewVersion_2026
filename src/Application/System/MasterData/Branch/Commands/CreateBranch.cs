using Application.Common;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Branch.Dtos;
using Application.System.MasterData.Branch.Validators;
using Domain.System.MasterData;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Branch.Commands
{
    public static class CreateBranch
    {
        public record Command(CreateBranchDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Data)
                    .SetValidator(new CreateBranchValidator());
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IBranchRepository _repo;
            private readonly ICompanyRepository _companyRepo;

            public Handler(IBranchRepository repo, ICompanyRepository companyRepo)
            {
                _repo = repo;
                _companyRepo = companyRepo;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                // Check if company exists
                var company = await _companyRepo.GetByIdAsync(request.Data.CompanyId);
                if (company == null)
                    throw new NotFoundException("Create Branch",$"Company with ID {request.Data.CompanyId} not found");

                // Check if code exists within the same company
                var exists = await _repo.CodeExistsAsync(request.Data.Code, request.Data.CompanyId);
                if (exists)
                    throw new ConflictException($"Branch with code '{request.Data.Code}' already exists in this company");

                // Check if parent branch exists if provided
                if (request.Data.ParentId.HasValue)
                {
                    var parent = await _repo.GetByIdAsync(request.Data.ParentId.Value);
                    if (parent == null)
                        throw new NotFoundException("Create Branch", $"Parent branch with ID {request.Data.ParentId} not found");
                }

                var branch = new Domain.System.MasterData.Branch(
                    code: request.Data.Code,
                    companyId: request.Data.CompanyId,
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

                await _repo.AddAsync(branch);
                await _repo.SaveChangesAsync(cancellationToken);

                return branch.Id;
            }
        }
    }
}