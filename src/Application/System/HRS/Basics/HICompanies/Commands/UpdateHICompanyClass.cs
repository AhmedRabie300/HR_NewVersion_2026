using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.HICompanies.Dtos;
using Application.System.HRS.Basics.HICompanies.Validators;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.Basics.HICompanies.Commands
{
    public static class UpdateHICompanyClass
    {
        public record Command(UpdateHICompanyClassDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, IHICompanyRepository repo)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new UpdateHICompanyClassValidator(msg));

                RuleFor(x => x.Data.HICompanyId)
                    .MustAsync(async (id, cancellation) => await repo.ExistsAsync(id))
                    .WithMessage(x => msg.Format("NotFound", msg.Get("HICompany"), x.Data.HICompanyId));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IHICompanyRepository _repo;
            private readonly IValidationMessages _msg;

            public Handler(IHICompanyRepository repo, IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = await _repo.GetClassByIdAsync(request.Data.Id);
                if (entity == null)
                    throw new NotFoundException(_msg.NotFound("HICompanyClass", request.Data.Id));

                entity.Update(
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    remarks: request.Data.Remarks,
                    companyAmount: request.Data.CompanyAmount,
                    employeeAmount: request.Data.EmployeeAmount
                );

                await _repo.UpdateClassAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}