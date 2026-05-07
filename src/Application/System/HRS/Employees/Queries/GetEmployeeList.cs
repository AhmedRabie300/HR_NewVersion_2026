using Application.System.HRS.Abstractions;
using MediatR;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace Application.System.HRS.Employees.Queries
{
    public static class GetEmployeeList
    {
        public record Query(
         int PageNumber = 1,
         int PageSize = 20,
         string? OrderBy = null,
         string? OrderDirection = null,
         string? Filters = null
     ) : IRequest<string?>;
        public class Handler : IRequestHandler<Query, string?>
        {
            private readonly IEmployeeRepository _repo;

            public Handler(IEmployeeRepository repo)
            {
                _repo = repo;
            }

            public async Task<string?> Handle(Query request, CancellationToken cancellationToken)
            {
                var criteriaObj = new JObject();

                if (!string.IsNullOrEmpty(request.Filters))
                {
                    var filtersDict = JsonSerializer.Deserialize<Dictionary<string, string>>(request.Filters);

                    if (filtersDict != null)
                    {
                        foreach (var filter in filtersDict)
                        {
                            if (!string.IsNullOrEmpty(filter.Value))
                            {
                                criteriaObj[filter.Key] = filter.Value;
                            }
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