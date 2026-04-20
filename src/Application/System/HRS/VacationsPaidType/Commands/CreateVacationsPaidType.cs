using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.VacationsPaidType.Dtos;
using Application.System.HRS.VacationsPaidType.Validators;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.VacationsPaidType.Commands
{
    public static class CreateVacationsPaidType
    {
        public record Command(
            CreateVacationsPaidTypeDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, IVacationsPaidTypeRepository repo)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new CreateVacationsPaidTypeValidator(msg, repo));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IVacationsPaidTypeRepository _repo;
            private readonly IValidationMessages _msg;

            public Handler(
                IVacationsPaidTypeRepository repo,
                IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
      

                var entity = new Domain.System.HRS.VacationsPaidType(
                    code: request.Data.Code,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    regUserId: null
                );

                await _repo.AddAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}