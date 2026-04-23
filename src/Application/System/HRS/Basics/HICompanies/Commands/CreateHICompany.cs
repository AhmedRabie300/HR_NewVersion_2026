using Application.Abstractions;
using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.HICompanies.Dtos;
using Application.System.HRS.Basics.HICompanies.Validators;
using Domain.System.HRS.Basics.HICompanies;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.Basics.HICompanies.Commands
{
    public static class CreateHICompany
    {
        public record Command(CreateHICompanyDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, IHICompanyRepository repo)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new CreateHICompanyValidator(msg, repo));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IHICompanyRepository _repo;
            private readonly ICurrentUser _currentUser;

            public Handler(IHICompanyRepository repo, ICurrentUser currentUser)
            {
                _repo = repo;
                _currentUser = currentUser;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var companyId = _currentUser.CompanyId;

                var entity = new HICompany(
                    code: request.Data.Code,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    companyId: companyId,
                    remarks: request.Data.Remarks,
                    regComputerId: request.Data.RegComputerId
                );

                 if (request.Data.Classes != null)
                {
                    foreach (var classDto in request.Data.Classes)
                    {
                        var hiCompanyClass = new HICompanyClass(
                            hiCompanyId: 0,  
                            companyId: companyId,
                            engName: classDto.EngName,
                            arbName: classDto.ArbName,
                            arbName4S: classDto.ArbName4S,
                            remarks: classDto.Remarks,
                            companyAmount: classDto.CompanyAmount,
                            employeeAmount: classDto.EmployeeAmount,
                            regComputerId: classDto.RegComputerId
                        );
                        entity.AddClass(hiCompanyClass);
                    }
                }

                await _repo.AddAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}