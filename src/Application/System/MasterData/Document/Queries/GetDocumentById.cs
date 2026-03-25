using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Document.Dtos;
using Application.Common.Abstractions;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Document.Queries
{
    public static class GetDocumentById
    {
        public record Query(int Id, int Lang = 1) : IRequest<DocumentDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            public Validator(ILocalizationService localizer)
            {
                RuleFor(x => x.Id).GreaterThan(0).WithMessage(localizer.GetMessage("IdGreaterThanZero", 1));
            }
        }

        public class Handler : IRequestHandler<Query, DocumentDto>
        {
            private readonly IDocumentRepository _repo;

            public Handler(IDocumentRepository repo)
            {
                _repo = repo;
            }

            public async Task<DocumentDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var entity = await _repo.GetByIdAsync(request.Id);
                if (entity == null)
                    throw new Exception($"Document with ID {request.Id} not found");

                return new DocumentDto(
                    entity.Id,
                    entity.Code,
                    entity.EngName,
                    entity.ArbName,
                    entity.ArbName4S,
                    entity.IsForCompany,
                    entity.Remarks,
                    entity.DocumentTypesGroupId,
                    entity.DocumentTypesGroup?.EngName ?? entity.DocumentTypesGroup?.ArbName,
                    entity.RegDate,
                    entity.CancelDate,
                    entity.IsActive()
                );
            }
        }
    }
}