using MediatR;
using VenusHR.Application.Common.DTOs.Shared;
using VenusHR.Application.Common.Interfaces.HR_Master;
using VenusHR.Core.Master;
using WorkFlow_EF;
namespace VenusHR.Application.Common.Interfaces
{
 
    public class GetAllBloodGroupsQuery : IRequest<ApiResponse<object>>
    {
        public int Lang { get; set; }
        public GetAllBloodGroupsQuery(int lang = 0) => Lang = lang;
    }

    public class GetBloodGroupByIdQuery : IRequest<ApiResponse<object>>
    {
        public int Id { get; set; }
        public int Lang { get; set; }
        public GetBloodGroupByIdQuery(int id, int lang = 0)
        {
            Id = id;
            Lang = lang;
        }
    }

 
    public class CreateBloodGroupCommand : IRequest<ApiResponse<object>>
    {
        public hrs_BloodGroups BloodGroup { get; set; }
        public CreateBloodGroupCommand(hrs_BloodGroups bloodGroup) => BloodGroup = bloodGroup;
    }

    public class UpdateBloodGroupCommand : IRequest<ApiResponse<object>>
    {
        public int Id { get; set; }
        public hrs_BloodGroups BloodGroup { get; set; }
        public UpdateBloodGroupCommand(int id, hrs_BloodGroups bloodGroup)
        {
            Id = id;
            BloodGroup = bloodGroup;
        }
    }

    public class DeleteBloodGroupCommand : IRequest<ApiResponse<object>>
    {
        public int Id { get; set; }
        public DeleteBloodGroupCommand(int id) => Id = id;
    }

 
    public class BloodGroupHandler :
        IRequestHandler<GetAllBloodGroupsQuery, ApiResponse<object>>,
        IRequestHandler<GetBloodGroupByIdQuery, ApiResponse<object>>,
        IRequestHandler<CreateBloodGroupCommand, ApiResponse<object>>,
        IRequestHandler<UpdateBloodGroupCommand, ApiResponse<object>>,
        IRequestHandler<DeleteBloodGroupCommand, ApiResponse<object>>
    {
        private readonly IHRMaster _hrMaster;

        public BloodGroupHandler(IHRMaster hrMaster)
        {
            _hrMaster = hrMaster;
        }

         public async Task<ApiResponse<object>> Handle(GetAllBloodGroupsQuery request, CancellationToken ct)
        {
            var result = await _hrMaster.GetAllBloodGroupsAsync(request.Lang);
            return ConvertToApiResponse(result);
        }

        public async Task<ApiResponse<object>> Handle(GetBloodGroupByIdQuery request, CancellationToken ct)
        {
            var result = await _hrMaster.GetBloodGroupByIdAsync(request.Id, request.Lang);
            return ConvertToApiResponse(result);
        }

         public async Task<ApiResponse<object>> Handle(CreateBloodGroupCommand request, CancellationToken ct)
        {
            var result = await _hrMaster.CreateBloodGroupAsync(request.BloodGroup);
            return ConvertToApiResponse(result);
        }

        public async Task<ApiResponse<object>> Handle(UpdateBloodGroupCommand request, CancellationToken ct)
        {
            var result = await _hrMaster.UpdateBloodGroupAsync(request.Id, request.BloodGroup);
            return ConvertToApiResponse(result);
        }

        public async Task<ApiResponse<object>> Handle(DeleteBloodGroupCommand request, CancellationToken ct)
        {
            var result = await _hrMaster.DeleteBloodGroupAsync(request.Id);
            return ConvertToApiResponse(result);
        }

         private ApiResponse<object> ConvertToApiResponse(GeneralOutputClass<object> result)
        {
            if (result.ErrorCode == 0)
            {
                return ApiResponse<object>.Succeeded(
                    result.ResultObject,
                    result.ErrorMessage ?? "Operation completed successfully"
                );
            }
            else
            {
                return ApiResponse<object>.Failed(
                    result.ErrorMessage ?? "Operation failed",
                    result.ErrorCode.ToString()
                );
            }
        }
    }
}