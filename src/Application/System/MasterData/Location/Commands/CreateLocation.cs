using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Location.Dtos;
using Application.System.MasterData.Location.Validators;
using Domain.System.MasterData;
using FluentValidation;
using MediatR;
using Application.Abstractions;

namespace Application.System.MasterData.Location.Commands
{
    public static class CreateLocation
    {
        public record Command(CreateLocationDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg,ILocationRepository repo)
            {
                RuleFor(x => x.Data).SetValidator(new CreateLocationValidator(msg,repo));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly ILocationRepository _repo;
                        private readonly IValidationMessages _msg;
            private readonly IContextService _contextService;
private readonly IBranchRepository _branchRepo;
            private readonly IDepartmentRepository _departmentRepo;
            private readonly ICityRepository _cityRepo;
            public Handler(
                ILocationRepository repo, IValidationMessages msg,
                IBranchRepository branchRepo,
                IDepartmentRepository departmentRepo,
                ICityRepository cityRepo,
                IContextService contextService)
            {
                _repo = repo;
                _msg = msg;
                _branchRepo = branchRepo;
                _departmentRepo = departmentRepo;
                _cityRepo = cityRepo;
                _contextService = contextService;
                _contextService = contextService;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {

                var codeExists = await _repo.CodeExistsAsync(request.Data.Code );
                if (codeExists)
                {
                    throw new ConflictException(_msg.CodeExists("Location", request.Data.Code));
                }

                if (request.Data.BranchId.HasValue)
                {
                    var branch = await _branchRepo.GetByIdAsync(request.Data.BranchId.Value);
                    if (branch == null)
                        throw new NotFoundException(_msg.NotFound("Branch", request.Data.BranchId.Value));
                }

                if (request.Data.DepartmentId.HasValue)
                {
                    var department = await _departmentRepo.GetByIdAsync(request.Data.DepartmentId.Value);
                    if (department == null)
                        throw new NotFoundException(_msg.NotFound("Department", request.Data.DepartmentId.Value));
                }

                if (request.Data.CityId.HasValue)
                {
                    var city = await _cityRepo.GetByIdAsync(request.Data.CityId.Value);
                    if (city == null)
                        throw new NotFoundException(_msg.NotFound("City", request.Data.CityId.Value));
                }

                var entity = new Domain.System.MasterData.Location(
                    code: request.Data.Code,
                     
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    cityId: request.Data.CityId,
                    branchId: request.Data.BranchId,
                    storeId: request.Data.StoreId,
                    departmentId: request.Data.DepartmentId,
                    remarks: request.Data.Remarks,
                    costCenterCode1: request.Data.CostCenterCode1,
                    costCenterCode2: request.Data.CostCenterCode2,
                    costCenterCode3: request.Data.CostCenterCode3,
                    costCenterCode4: request.Data.CostCenterCode4
                );

                await _repo.AddAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}