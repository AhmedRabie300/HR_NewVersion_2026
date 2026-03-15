using Application.UARbac.FormControls;
using Application.UARbac.FormControls.Dtos;
using MediatR;

namespace API.UARbac;

public static class FormControlEndpoints
{
    public static IEndpointRouteBuilder MapFormControlEndpoints(this IEndpointRouteBuilder routes)
    {
        // Option A: Nest under forms (recommended for "list by formId")
        var forms = routes.MapGroup("/forms").WithTags("Forms");

        forms.MapGet("/{formId:int}/controls",
      async (IMediator mediator, int formId, int? section, CancellationToken ct) =>
      {
          var result = await mediator.Send(new ListByFormId.Query(formId, section), ct);
          return Results.Ok(result);
      })
      .WithName("ListFormControlsByFormId");


        forms.MapPut("/{id:int}",
            async (IMediator mediator, int id, UpdateFormControlDto dto, CancellationToken ct) =>
            {
                // safety: route id is the source of truth
                var fixedDto = dto with { Id = id };

                await mediator.Send(new Update.Command(fixedDto), ct);
                return Results.NoContent();
            })
            .WithName("UpdateFormControl");

        return routes;
    }
}
