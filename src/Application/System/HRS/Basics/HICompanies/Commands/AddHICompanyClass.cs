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
    public static class AddHICompanyClass
    {
        public record Command(int HICompanyId, CreateHICompanyClassDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, IHICompanyRepository repo)
            {
                RuleFor(x => x.HICompanyId)
                    .GreaterThan(0).WithMessage(x => msg.Get("HICompanyIdRequired"))
                    .MustAsync(async (id, cancellation) => await repo.ExistsAsync(id))
                    .WithMessage(x => msg.Format("NotFound", msg.Get("HICompany"), x.HICompanyId));

                RuleFor(x => x.Data)
                    .SetValidator(new CreateHICompanyClassValidator(msg));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IHICompanyRepository _repo;
            private readonly ICurrentUser _currentUser;
            private readonly IValidationMessages _msg;

            public Handler(IHICompanyRepository repo, ICurrentUser currentUser, IValidationMessages msg)
            {
                _repo = repo;
                _currentUser = currentUser;
                _msg = msg;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var hiCompany = await _repo.GetByIdAsync(request.HICompanyId);
                if (hiCompany == null)
                    throw new NotFoundException(_msg.NotFound("HICompany", request.HICompanyId));

                var companyId = _currentUser.CompanyId;

                var entity = new HICompanyClass(
                    hiCompanyId: request.HICompanyId,
                    companyId: companyId,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    remarks: request.Data.Remarks,
                    companyAmount: request.Data.CompanyAmount,
                    employeeAmount: request.Data.EmployeeAmount,
                    regComputerId: request.Data.RegComputerId
                );

                await _repo.AddClassAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}