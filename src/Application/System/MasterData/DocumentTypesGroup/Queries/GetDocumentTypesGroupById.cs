using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.DocumentTypesGroup.Dtos;
using FluentValidation;
using MediatR;
using Application.Abstractions;

namespace Application.System.MasterData.DocumentTypesGroup.Queries
{
    public static class GetDocumentTypesGroupById
    {
        public record Query(int Id) : IRequest<DocumentTypesGroupDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            public Validator(IValidationMessages msg)
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage(msg.Get("IdGreaterThanZero"));
            }
        }

        public class Handler : IRequestHandler<Query, DocumentTypesGroupDto>
        {
            private readonly IDocumentTypesGroupRepository _repo;
                        private readonly IValidationMessages _msg;
public Handler(IDocumentTypesGroupRepository repo, IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<DocumentTypesGroupDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var entity = await _repo.GetByIdAsync(request.Id);
                if (entity == null)
                    throw new NotFoundException(_msg.NotFound("DocumentTypesGroup", request.Id));

                return new DocumentTypesGroupDto(
                    Id: entity.Id,
                    Code: entity.Code,
                    EngName: entity.EngName,
                    ArbName: entity.ArbName,
                    Remarks: entity.Remarks,
                    RegDate: entity.RegDate,
                    CancelDate: entity.CancelDate,
                    IsActive: entity.IsActive()
                );
            }
        }
    }
}