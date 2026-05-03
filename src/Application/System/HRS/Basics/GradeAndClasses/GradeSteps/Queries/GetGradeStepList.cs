using Application.System.HRS.Abstractions;
using MediatR;
using Newtonsoft.Json.Linq;

namespace Application.System.HRS.Basics.GradeAndClasses.GradeSteps.Queries
{
    public static class GetGradeStepList
    {
        public record Query(
            int PageNumber = 1,
            int PageSize = 20,
            string? OrderBy = null,
            string? OrderDirection = null,
            Dictionary<string, string>? Filters = null
        ) : IRequest<string?>;

        public class Handler : IRequestHandler<Query, string?>
        {
            private readonly IGradeStepRepository _repo;

            public Handler(IGradeStepRepository repo)
            {
                _repo = repo;
            }

            public async Task<string?> Handle(Query request, CancellationToken cancellationToken)
            {
                // Build JSON Criteria from Filters
                var criteriaObj = new JObject();

                if (request.Filters != null)
                {
                    foreach (var filter in request.Filters)
                    {
                        if (!string.IsNullOrEmpty(filter.Value))
                        {
                            criteriaObj[filter.Key.ToLower()] = filter.Value;
                        }
                    }
                }

                var criteria = criteriaObj.ToString();

                return await _repo.GetListJsonAsync(
                    request.PageNumber,
                    request.PageSize,
                    request.OrderBy,
                    request.OrderDirection,
                    criteria);
            }
        }
    }
}