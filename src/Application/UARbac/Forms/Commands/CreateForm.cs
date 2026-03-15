//// Application/UARbac/Forms/Commands/CreateForm.cs
//using Application.UARbac.Forms.Dtos;
//using Application.UARbac.Abstractions;
//using Domain.UARbac;
//using FluentValidation;
//using MediatR;

//namespace Application.UARbac.Forms.Commands
//{
//    public static class CreateForm
//    {
//        public record Command(CreateFormDto Data) : IRequest<int>;

//        public sealed class Validator : AbstractValidator<Command>
//        {
//            public Validator()
//            {
//                RuleFor(x => x.Data.ModuleId)
//                    .GreaterThan(0)
//                    .WithMessage("Module ID is required");

//                RuleFor(x => x.Data.Code)
//                    .MaximumLength(50);

//                RuleFor(x => x.Data.EngName)
//                    .MaximumLength(200);

//                RuleFor(x => x.Data.ArbName)
//                    .MaximumLength(200);
//            }
//        }

//        public class Handler : IRequestHandler<Command, int>
//        {
//            private readonly IFormRepository _repo;
//            private readonly IModuleRepository _moduleRepo;

//            public Handler(IFormRepository repo, IModuleRepository moduleRepo)
//            {
//                _repo = repo;
//                _moduleRepo = moduleRepo;
//            }

//            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
//            {
//                // Check if module exists
//                var module = await _moduleRepo.GetByIdAsync(request.Data.ModuleId);
//                if (module == null)
//                    throw new Exception($"Module with ID {request.Data.ModuleId} not found");

//                // Check if code exists (if provided)
//                if (!string.IsNullOrWhiteSpace(request.Data.Code))
//                {
//                    var existing = await _repo.GetByCodeAsync(request.Data.Code);
//                    if (existing != null)
//                        throw new Exception($"Form with code '{request.Data.Code}' already exists");
//                }

//                var form = new Form(
//                    request.Data.Code,
//                    request.Data.EngName,
//                    request.Data.ArbName,
//                    request.Data.ArbName4S,
//                    request.Data.EngDescription,
//                    request.Data.ArbDescription,
//                    request.Data.Rank,
//                    request.Data.ModuleId,
//                    request.Data.SearchFormId,
//                    request.Data.Height,
//                    request.Data.Width,
//                    request.Data.Remarks,
//                    request.Data.RegUserId,
//                    request.Data.RegComputerId,
//                    request.Data.Layout,
//                    request.Data.LinkTarget,
//                    request.Data.LinkUrl,
//                    request.Data.ImageUrl,
//                    request.Data.MainId
//                );

//                await _repo.AddAsync(form);
//                await _repo.SaveChangesAsync(cancellationToken);

//                return form.Id;
//            }
//        }
//    }
//}