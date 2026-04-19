using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Document.Dtos;
using FluentValidation;
using MediatR;
using Application.Abstractions;

namespace Application.System.MasterData.Document.Queries
{
    public static class GetDocumentById
    {
        public record Query(int Id) : IRequest<DocumentDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            public Validator(IValidationMessages msg)
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage(msg.Get("IdGreaterThanZero"));
            }
        }

        public class Handler : IRequestHandler<Query, DocumentDto>
        {
            private readonly IDocumentRepository _repo;
                        private readonly IValidationMessages _msg;
public Handler(IDocumentRepository repo, IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<DocumentDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var entity = await _repo.GetByIdAsync(request.Id);
                if (entity == null)
                    throw new NotFoundException(_msg.NotFound("Document", request.Id));

                return new DocumentDto(
                    Id: entity.Id,
                    Code: entity.Code,
                    EngName: entity.EngName,
                    ArbName: entity.ArbName,
                    ArbName4S: entity.ArbName4S,
                    IsForCompany: entity.IsForCompany,
                    Remarks: entity.Remarks,
                    DocumentTypesGroupId: entity.DocumentTypesGroupId,
                    DocumentTypesGroupName: entity.DocumentTypesGroup?.EngName ?? entity.DocumentTypesGroup?.ArbName,
                    RegDate: entity.RegDate,
                    CancelDate: entity.CancelDate,
                    IsActive: entity.IsActive()
                );
            }
        }
    }
}