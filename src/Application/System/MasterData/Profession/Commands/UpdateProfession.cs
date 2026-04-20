// Application/System/MasterData/Profession/Commands/UpdateProfession.cs
using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Profession.Dtos;
using Application.System.MasterData.Profession.Validators;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Application.Abstractions;

namespace Application.System.MasterData.Profession.Commands
{
    public static class UpdateProfession
    {
        public record Command(UpdateProfessionDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg,IProfessionRepository repo)
            {
                RuleFor(x => x.Data).SetValidator(new UpdateProfessionValidator(msg,repo));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IProfessionRepository _repo;
                        private readonly IValidationMessages _msg;
            private readonly IContextService _ContextService;
public Handler(
                IProfessionRepository repo, IValidationMessages msg, IContextService ContextService)
            {
                _repo = repo;
                _msg = msg;
                _ContextService = ContextService;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                 var entity = await _repo.GetByIdAsync(request.Data.Id);
           
                 
                entity.Update(
                    request.Data.EngName,
                    request.Data.ArbName,
                    request.Data.ArbName4S,
                    request.Data.Remarks
                );

                await _repo.UpdateAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}