using Application.Common.Abstractions;
using Application.Common.Lookups;
using FluentValidation;
using MediatR;

namespace Application.Common
{
    public static class GetLookup
    {
        public record Query(string EntityName) : IRequest<List<LookupDto>>;

        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.EntityName)
                    .NotEmpty().WithMessage("Entity name is required");
            }
        }

        public class Handler : IRequestHandler<Query, List<LookupDto>>
        {
            private readonly ILookupService _lookupService;

            public Handler(ILookupService lookupService)
            {
                _lookupService = lookupService;
            }

            public async Task<List<LookupDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                // Find the entity type by name
                var entityType = FindEntityType(request.EntityName);

                if (entityType == null)
                    throw new Exception($"Entity '{request.EntityName}' not found");

                // Call the generic method
                var method = typeof(ILookupService)
                    .GetMethod(nameof(ILookupService.GetLookupAsync))
                    ?.MakeGenericMethod(entityType);

                if (method == null)
                    throw new Exception($"Cannot get lookup for '{request.EntityName}'");

                var result = await (Task<List<LookupDto>>)method.Invoke(_lookupService, new object[] { cancellationToken })!;
                return result;
            }

            private Type? FindEntityType(string entityName)
            {
                var domainAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                    .Where(a => a.FullName?.Contains("Domain") == true)
                    .ToList();

                foreach (var assembly in domainAssemblies)
                {
                    var entityType = assembly.GetTypes()
                        .Where(t => t.Namespace != null &&
                                    t.Namespace.StartsWith("Domain.System") &&
                                    !t.IsAbstract)
                        .FirstOrDefault(t => t.Name.Equals(entityName, StringComparison.OrdinalIgnoreCase));

                    if (entityType != null)
                        return entityType;
                }

                return null;
            }
        }
    }
}