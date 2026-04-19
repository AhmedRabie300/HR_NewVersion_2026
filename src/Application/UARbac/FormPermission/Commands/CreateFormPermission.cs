using Application.Common;
using Application.Common.Abstractions;
using Application.UARbac.Abstractions;
using Application.UARbac.FormPermission.Dtos;
using Domain.UARbac;
using FluentValidation;
using MediatR;

namespace Application.UARbac.FormPermissions.Commands
{
    public static class CreateFormPermission
    {
        public record Command(CreateFormPermissionDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg)
            {
                RuleFor(x => x.Data.FormId)
                    .GreaterThan(0)
                    .WithMessage(msg.Get("FormIdRequired"));

                RuleFor(x => x.Data)
                    .Must(x => (x.GroupId.HasValue && !x.UserId.HasValue) || (!x.GroupId.HasValue && x.UserId.HasValue))
                    .WithMessage(msg.Get("EitherGroupOrUser"));

                RuleFor(x => x.Data)
                    .Must(x => x.AllowView == true || x.AllowAdd == true || x.AllowEdit == true || x.AllowDelete == true || x.AllowPrint == true)
                    .WithMessage(msg.Get("AtLeastOnePermission"));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IFormPermissionRepository _repo;
            private readonly IFormRepository _formRepo;
            private readonly IGroupRepository _groupRepo;
            private readonly IUserRepository _userRepo;
            private readonly IValidationMessages _msg;

            public Handler(
                IFormPermissionRepository repo,
                IFormRepository formRepo,
                IGroupRepository groupRepo,
                IUserRepository userRepo,
                IValidationMessages msg)
            {
                _repo = repo;
                _formRepo = formRepo;
                _groupRepo = groupRepo;
                _userRepo = userRepo;
                _msg = msg;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var form = await _formRepo.GetByIdAsync(request.Data.FormId);
                if (form == null)
                    throw new NotFoundException(_msg.NotFound("Form", request.Data.FormId));

                if (request.Data.GroupId.HasValue)
                {
                    var group = await _groupRepo.GetByIdAsync(request.Data.GroupId.Value);
                    if (group == null)
                        throw new NotFoundException(_msg.NotFound("Group", request.Data.GroupId.Value));

                    var existing = await _repo.GetByFormAndGroupAsync(request.Data.FormId, request.Data.GroupId.Value);
                    if (existing != null)
                        throw new ConflictException(_msg.Get("PermissionAlreadyExists"));
                }

                if (request.Data.UserId.HasValue)
                {
                    var user = await _userRepo.GetByIdAsync(request.Data.UserId.Value);
                    if (user == null)
                        throw new NotFoundException(_msg.NotFound("User", request.Data.UserId.Value));

                    var existing = await _repo.GetByFormAndUserAsync(request.Data.FormId, request.Data.UserId.Value);
                    if (existing != null)
                        throw new ConflictException(_msg.Get("PermissionAlreadyExists"));
                }

                var permission = new Domain.UARbac.FormPermission(
                    request.Data.FormId,
                    request.Data.GroupId,
                    request.Data.UserId,
                    request.Data.AllowView,
                    request.Data.AllowAdd,
                    request.Data.AllowEdit,
                    request.Data.AllowDelete,
                    request.Data.AllowPrint,
                    request.Data.RegUserId,
                    request.Data.regComputerId
                );

                await _repo.AddAsync(permission);
                await _repo.SaveChangesAsync(cancellationToken);

                return permission.Id;
            }
        }
    }
}
