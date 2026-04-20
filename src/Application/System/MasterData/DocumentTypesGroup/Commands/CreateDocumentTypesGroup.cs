using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.DocumentTypesGroup.Dtos;
using Application.System.MasterData.DocumentTypesGroup.Validators;
using Domain.System.MasterData;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.DocumentTypesGroup.Commands
{
    public static class CreateDocumentTypesGroup
    {
        public record Command(CreateDocumentTypesGroupDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg,IDocumentTypesGroupRepository repo)
            {
                RuleFor(x => x.Data).SetValidator(new CreateDocumentTypesGroupValidator(msg,repo));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IDocumentTypesGroupRepository _repo;
                        private readonly IValidationMessages _msg;
public Handler(
                IDocumentTypesGroupRepository repo, IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                

var entity = new Domain.System.MasterData.DocumentTypesGroup(
                    code: request.Data.Code,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    remarks: request.Data.Remarks
                );

                await _repo.AddAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}