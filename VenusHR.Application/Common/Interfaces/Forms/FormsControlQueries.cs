using MediatR;
using VenusHR.Application.Common.DTOs.Forms;

namespace VenusHR.Application.Common.Interfaces.Forms
{
    public static class FormsControlQueries
    {
        public record GetAllControlsQuery() : IRequest<List<FormsControlDto>>;
        public record GetControlByIdQuery(int Id) : IRequest<FormsControlDto>;
        public record GetControlsByFormIdQuery(int FormId) : IRequest<object>;  
        public record GetVisibleControlsByFormIdQuery(int FormId) : IRequest<List<FormsControlDto>>;

        public class Handler :
            IRequestHandler<GetAllControlsQuery, List<FormsControlDto>>,
            IRequestHandler<GetControlByIdQuery, FormsControlDto>,
            IRequestHandler<GetControlsByFormIdQuery, object>,
            IRequestHandler<GetVisibleControlsByFormIdQuery, List<FormsControlDto>>
        {
            private readonly IFormsControlService _service;

            public Handler(IFormsControlService service) => _service = service;

            public async Task<List<FormsControlDto>> Handle(GetAllControlsQuery query, CancellationToken ct)
                => await _service.GetAllControlsAsync();

            public async Task<FormsControlDto> Handle(GetControlByIdQuery query, CancellationToken ct)
                => await _service.GetControlByIdAsync(query.Id);

            public async Task<object> Handle(GetControlsByFormIdQuery query, CancellationToken ct)
                => await _service.GetFormControlsStructureAsync(query.FormId);

            public async Task<List<FormsControlDto>> Handle(GetVisibleControlsByFormIdQuery query, CancellationToken ct)
                => await _service.GetVisibleControlsByFormIdAsync(query.FormId);
        }
    }
}