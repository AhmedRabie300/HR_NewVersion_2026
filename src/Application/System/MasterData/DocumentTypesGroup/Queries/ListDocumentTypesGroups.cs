using Application.System.MasterData.Abstractions;
using Application.System.MasterData.DocumentTypesGroup.Dtos;
using MediatR;

namespace Application.System.MasterData.DocumentTypesGroup.Queries
{
    public static class ListDocumentTypesGroups
    {
        public record Query : IRequest<List<DocumentTypesGroupDto>>;

        public class Handler : IRequestHandler<Query, List<DocumentTypesGroupDto>>
        {
            private readonly IDocumentTypesGroupRepository _repo;

            public Handler(IDocumentTypesGroupRepository repo)
            {
                _repo = repo;
            }

            public async Task<List<DocumentTypesGroupDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var items = await _repo.GetAllAsync();

                return items.Select(x => new DocumentTypesGroupDto(
                    x.Id,
                    x.Code,
                    x.EngName,
                    x.ArbName,
                    x.Remarks,
                    x.RegDate,
                    x.CancelDate,
                    x.IsActive()
                )).ToList();
            }
        }
    }
}