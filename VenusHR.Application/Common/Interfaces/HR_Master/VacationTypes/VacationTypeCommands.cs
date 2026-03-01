using MediatR;
using VenusHR.Application.Common.DTOs.Shared;
using VenusHR.Application.Common.Interfaces.HR_Master;
using VenusHR.Core.Master;
using WorkFlow_EF;

namespace VenusHR.Application.Common.Interfaces
{
    // =============== QUERIES ===============
    public class GetAllVacationTypesQuery : IRequest<ApiResponse<object>>
    {
        public int Lang { get; set; }
        public GetAllVacationTypesQuery(int lang = 0) => Lang = lang;
    }

    public class GetVacationTypeByIdQuery : IRequest<ApiResponse<object>>
    {
        public int Id { get; set; }
        public int Lang { get; set; }
        public GetVacationTypeByIdQuery(int id, int lang = 0)
        {
            Id = id;
            Lang = lang;
        }
    }

    // =============== HANDLER ===============
    public class VacationTypeHandler :
        IRequestHandler<GetAllVacationTypesQuery, ApiResponse<object>>,
        IRequestHandler<GetVacationTypeByIdQuery, ApiResponse<object>>
    {
        private readonly IHRMaster _hrMaster;

        public VacationTypeHandler(IHRMaster hrMaster) => _hrMaster = hrMaster;

        private ApiResponse<object> ConvertToApiResponse(GeneralOutputClass<object> result)
        {
            if (result.ErrorCode == 0)
                return ApiResponse<object>.Succeeded(result.ResultObject, result.ErrorMessage ?? "Success");
            else
                return ApiResponse<object>.Failed(result.ErrorMessage ?? "Failed", result.ErrorCode.ToString());
        }

        public async Task<ApiResponse<object>> Handle(GetAllVacationTypesQuery request, CancellationToken ct)
            => ConvertToApiResponse(await _hrMaster.GetAllVacationTypesAsync(request.Lang));

        public async Task<ApiResponse<object>> Handle(GetVacationTypeByIdQuery request, CancellationToken ct)
            => ConvertToApiResponse(await _hrMaster.GetVacationTypeByIdAsync(request.Id, request.Lang));
    }
}