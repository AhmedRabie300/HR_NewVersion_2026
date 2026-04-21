using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.Gender.Dtos;
using Application.System.HRS.Basics.Gender.Validators;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.Basics.Gender.Commands
{
    public static class CreateGender
    {
        public record Command(
            CreateGenderDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, IGenderRepository repo)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new CreateGenderValidator(msg, repo));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IGenderRepository _repo;
            private readonly IValidationMessages _msg;

            public Handler(
                IGenderRepository repo,
                IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
             
                var entity = new Domain.System.HRS.Gender(
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