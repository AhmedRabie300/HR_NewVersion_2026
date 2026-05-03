using Application.System.HRS.Abstractions;
using MediatR;
using Newtonsoft.Json.Linq;

namespace Application.System.HRS.Basics.EmployeesClasses.Queries
{
    public static class GetEmployeeClassList
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
            private readonly IEmployeeClassRepository _repo;

            public Handler(IEmployeeClassRepository repo)
            {
                _repo = repo;
            }

            public async Task<string?> Handle(Query request, CancellationToken cancellationToken)
            {
                 var criteriaObj = new JObject();

                if (request.Filters != null)
                {
                    foreach (var filter in request.Filters)
                    {
                        if (!string.IsNullOrEmpty(filter.Value))
                        {
                            criteriaObj[filter.Key] = filter.Value;
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