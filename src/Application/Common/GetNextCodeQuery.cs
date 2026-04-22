using Application.Common.Abstractions;
using MediatR;

namespace Application.Common
{
    public static class GetNextCode
    {
        public record Query(string EntityName) : IRequest<string>;

        public class Handler : IRequestHandler<Query, string>
        {
            private readonly ICodeGeneratorService _codeGenerator;

            public Handler(ICodeGeneratorService codeGenerator)
            {
                _codeGenerator = codeGenerator;
            }

            public async Task<string> Handle(Query request, CancellationToken cancellationToken)
            {
                var entityType = FindEntityType(request.EntityName);

                if (entityType == null)
                    throw new NotFoundException($"Entity '{request.EntityName}' not found");

                var method = typeof(ICodeGeneratorService)
                    .GetMethod(nameof(ICodeGeneratorService.GetNextCodeAsync))
                    ?.MakeGenericMethod(entityType);

                if (method == null)
                    throw new Exception($"Cannot get next code for '{request.EntityName}'");

                var result = await (Task<string>)method.Invoke(_codeGenerator, new object[] { cancellationToken })!;
                return result;
            }

            private static Type? FindEntityType(string entityName)
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