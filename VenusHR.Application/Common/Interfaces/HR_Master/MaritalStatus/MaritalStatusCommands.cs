using MediatR;
using VenusHR.Application.Common.DTOs.Shared;
using WorkFlow_EF;

namespace VenusHR.Application.Common.Interfaces.HR_Master.MaritalStatus
{
     public class GetAllMaritalStatusQuery : IRequest<ApiResponse<object>>
    {
        public int Lang { get; set; }
        public GetAllMaritalStatusQuery(int lang = 0) => Lang = lang;
    }

    public class GetMaritalStatusByIdQuery : IRequest<ApiResponse<object>>
    {
        public int Id { get; set; }
        public int Lang { get; set; }
        public GetMaritalStatusByIdQuery(int id, int lang = 0)
        {
            Id = id;
            Lang = lang;
        }
    }

     public class MaritalStatusHandler :
        IRequestHandler<GetAllMaritalStatusQuery, ApiResponse<object>>,
        IRequestHandler<GetMaritalStatusByIdQuery, ApiResponse<object>>
    {
        private readonly IHRMaster _hrMaster;

        public MaritalStatusHandler(IHRMaster hrMaster) => _hrMaster = hrMaster;

        private ApiResponse<object> ConvertToApiResponse(GeneralOutputClass<object> result)
        {
            if (result.ErrorCode == 0)
                return ApiResponse<object>.Succeeded(result.ResultObject, result.ErrorMessage ?? "Success");
            else
                return ApiResponse<object>.Failed(result.ErrorMessage ?? "Failed", result.ErrorCode.ToString());
        }

        public async Task<ApiResponse<object>> Handle(GetAllMaritalStatusQuery request, CancellationToken ct)
            => ConvertToApiResponse(await _hrMaster.GetAllMaritalStatusAsync(request.Lang));

        public async Task<ApiResponse<object>> Handle(GetMaritalStatusByIdQuery request, CancellationToken ct)
            => ConvertToApiResponse(await _hrMaster.GetMaritalStatusByIdAsync(request.Id, request.Lang));
    }
}