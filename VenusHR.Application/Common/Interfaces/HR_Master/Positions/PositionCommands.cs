using MediatR;
using VenusHR.Application.Common.DTOs.Shared;
using VenusHR.Application.Common.Interfaces.HR_Master;
using VenusHR.Core.Master;
using WorkFlow_EF;

namespace VenusHR.Application.Common.Interfaces
{
    // =============== QUERIES ===============
    public class GetAllPositionsQuery : IRequest<ApiResponse<object>>
    {
        public int Lang { get; set; }
        public GetAllPositionsQuery(int lang = 0) => Lang = lang;
    }

    public class GetPositionByIdQuery : IRequest<ApiResponse<object>>
    {
        public int Id { get; set; }
        public int Lang { get; set; }
        public GetPositionByIdQuery(int id, int lang = 0)
        {
            Id = id;
            Lang = lang;
        }
    }

    // =============== HANDLER ===============
    public class PositionHandler :
        IRequestHandler<GetAllPositionsQuery, ApiResponse<object>>,
        IRequestHandler<GetPositionByIdQuery, ApiResponse<object>>
    {
        private readonly IHRMaster _hrMaster;

        public PositionHandler(IHRMaster hrMaster) => _hrMaster = hrMaster;

        private ApiResponse<object> ConvertToApiResponse(GeneralOutputClass<object> result)
        {
            if (result.ErrorCode == 0)
                return ApiResponse<object>.Succeeded(result.ResultObject, result.ErrorMessage ?? "Success");
            else
                return ApiResponse<object>.Failed(result.ErrorMessage ?? "Failed", result.ErrorCode.ToString());
        }

        public async Task<ApiResponse<object>> Handle(GetAllPositionsQuery request, CancellationToken ct)
            => ConvertToApiResponse(await _hrMaster.GetAllPositionsAsync(request.Lang));

        public async Task<ApiResponse<object>> Handle(GetPositionByIdQuery request, CancellationToken ct)
            => ConvertToApiResponse(await _hrMaster.GetPositionByIdAsync(request.Id, request.Lang));
    }
}