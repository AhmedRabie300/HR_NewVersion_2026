using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Document.Dtos;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Document.Queries
{
    public static class GetDocumentsByGroup
    {
        public record Query(int DocumentTypesGroupId) : IRequest<List<DocumentDto>>;

        public sealed class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.DocumentTypesGroupId).GreaterThan(0);
            }
        }

        public class Handler : IRequestHandler<Query, List<DocumentDto>>
        {
            private readonly IDocumentRepository _repo;

            public Handler(IDocumentRepository repo)
            {
                _repo = repo;
            }

            public async Task<List<DocumentDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var items = await _repo.GetByGroupIdAsync(request.DocumentTypesGroupId);

                return items.Select(x => new DocumentDto(
                    x.Id,
                    x.Code,
                    x.EngName,
                    x.ArbName,
                    x.ArbName4S,
                    x.IsForCompany,
                    x.Remarks,
                    x.DocumentTypesGroupId,
                    x.DocumentTypesGroup?.EngName ?? x.DocumentTypesGroup?.ArbName,
                    x.RegDate,
                    x.CancelDate,
                    x.IsActive()
                )).ToList();
            }
        }
    }
}