using MediatR;
using VenusHR.Application.Common.DTOs.Forms;

namespace VenusHR.Application.Common.Interfaces.Forms
{
    public static class FormsControlCommands
    {
         public record CreateControlCommand(CreateFormsControlDto Dto) : IRequest<FormsControlDto>;
        public record UpdateControlCommand(int Id, UpdateFormsControlDto Dto) : IRequest<FormsControlDto>;
        public record DeleteControlCommand(int Id) : IRequest<bool>;
        public record CreateBulkControlsCommand(List<CreateFormsControlDto> Dtos) : IRequest<List<FormsControlDto>>;
        public record DeleteControlsByFormIdCommand(int FormId) : IRequest<bool>;
        public record ReorderControlsCommand(int FormId, Dictionary<int, int> ControlRanks) : IRequest<bool>;

        public class Handler :
            IRequestHandler<CreateControlCommand, FormsControlDto>,
            IRequestHandler<UpdateControlCommand, FormsControlDto>,
            IRequestHandler<DeleteControlCommand, bool>,
            IRequestHandler<CreateBulkControlsCommand, List<FormsControlDto>>,
            IRequestHandler<DeleteControlsByFormIdCommand, bool>,
            IRequestHandler<ReorderControlsCommand, bool>
        {
            private readonly IFormsControlService _service;

            public Handler(IFormsControlService service) => _service = service;

             public async Task<FormsControlDto> Handle(CreateControlCommand cmd, CancellationToken ct)
                => await _service.CreateControlAsync(cmd.Dto);

             public async Task<FormsControlDto> Handle(UpdateControlCommand cmd, CancellationToken ct)
                => await _service.UpdateControlAsync(cmd.Id, cmd.Dto);

             public async Task<bool> Handle(DeleteControlCommand cmd, CancellationToken ct)
                => await _service.SoftDeleteControlAsync(cmd.Id);

             public async Task<List<FormsControlDto>> Handle(CreateBulkControlsCommand cmd, CancellationToken ct)
                => await _service.CreateBulkControlsAsync(cmd.Dtos);

             public async Task<bool> Handle(DeleteControlsByFormIdCommand cmd, CancellationToken ct)
                => await _service.DeleteControlsByFormIdAsync(cmd.FormId);

             public async Task<bool> Handle(ReorderControlsCommand cmd, CancellationToken ct)
                => await _service.ReorderControlsAsync(cmd.FormId, cmd.ControlRanks);
        }
    }
}