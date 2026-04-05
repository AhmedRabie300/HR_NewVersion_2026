using Application.Common;
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
            public Validator()
            {
                RuleFor(x => x.Data.FormId)
                    .GreaterThan(0)
                    .WithMessage("Form ID is required");

                RuleFor(x => x.Data)
                    .Must(x => (x.GroupId.HasValue && !x.UserId.HasValue) ||
                              (!x.GroupId.HasValue && x.UserId.HasValue))
                    .WithMessage("Either GroupId or UserId must be provided, but not both");

                RuleFor(x => x.Data)
                    .Must(x => x.AllowView == true || x.AllowAdd == true ||
                              x.AllowEdit == true || x.AllowDelete == true ||
                              x.AllowPrint == true)
                    .WithMessage("At least one permission must be granted");
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IFormPermissionRepository _repo;
            private readonly IFormRepository _formRepo;
            private readonly IGroupRepository _groupRepo;
            private readonly IUserRepository _userRepo;

            public Handler(
                IFormPermissionRepository repo,
                IFormRepository formRepo,
                IGroupRepository groupRepo,
                IUserRepository userRepo)
            {
                _repo = repo;
                _formRepo = formRepo;
                _groupRepo = groupRepo;
                _userRepo = userRepo;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                // Check if form exists
                var form = await _formRepo.GetByIdAsync(request.Data.FormId);
                if (form == null)
                    throw new Exception($"Form with ID {request.Data.FormId} not found");

                // Check if group exists (if provided)
                if (request.Data.GroupId.HasValue)
                {
                    var group = await _groupRepo.GetByIdAsync(request.Data.GroupId.Value);
                    if (group == null)
                        throw new Exception($"Group with ID {request.Data.GroupId} not found");

                    // Check if permission already exists for this form and group
                    var existing = await _repo.GetByFormAndGroupAsync(
                        request.Data.FormId,
                        request.Data.GroupId.Value);

                    if (existing != null)
                        throw new Exception($"Permission already exists for this form and group");
                }

                // Check if user exists (if provided)
                if (request.Data.UserId.HasValue)
                {
                    var user = await _userRepo.GetByIdAsync(request.Data.UserId.Value);
                    if (user == null)
                        throw new Exception($"User with ID {request.Data.UserId} not found");

                    // Check if permission already exists for this form and user
                    var existing = await _repo.GetByFormAndUserAsync(
                        request.Data.FormId,
                        request.Data.UserId.Value);

                    if (existing != null)
                        throw new NotFoundException("Create Form Permission",$"Permission already exists for this form and user");
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