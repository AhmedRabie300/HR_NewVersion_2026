using Application.Abstractions;
using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.VacationAndEndOfService.EndOfService.Dtos;
using Application.System.HRS.VacationAndEndOfService.EndOfService.Validators;
using Domain.System.HRS.VacationAndEndOfService;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.VacationAndEndOfService.EndOfService.Commands
{
    public static class CreateEndOfService
    {
        public record Command(CreateEndOfServiceDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, IEndOfServiceRepository repo)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new CreateEndOfServiceValidator(msg, repo));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IEndOfServiceRepository _repo;
            private readonly ICurrentUser _currentUser;

            public Handler(IEndOfServiceRepository repo, ICurrentUser currentUser)
            {
                _repo = repo;
                _currentUser = currentUser;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var companyId = _currentUser.CompanyId;

                var entity = new Domain.System.HRS.VacationAndEndOfService.EndOfService(
                    code: request.Data.Code,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    companyId: companyId,
                    remarks: request.Data.Remarks,
                    regComputerId: request.Data.RegComputerId,
                    extraTransactionId: request.Data.ExtraTransactionId,
                    excludedFromSSRequests: request.Data.ExcludedFromSSRequests
                );

                // Add Rules
                if (request.Data.Rules != null)
                {
                    foreach (var ruleDto in request.Data.Rules)
                    {
                        var rule = new EndOfServiceRule(
                            endOfServiceId: 0, // Will be set after entity is added
                            fromWorkingMonths: ruleDto.FromWorkingMonths,
                            toWorkingMonths: ruleDto.ToWorkingMonths,
                            amountPercent: ruleDto.AmountPercent,
                            formula: ruleDto.Formula,
                            extraDedFormula: ruleDto.ExtraDedFormula,
                            remarks: ruleDto.Remarks,
                            regComputerId: ruleDto.RegComputerId
                        );
                        entity.AddRule(rule);
                    }
                }

                await _repo.AddAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}