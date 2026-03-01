using MediatR;
using VenusHR.Application.Common.DTOs.Shared;
using VenusHR.Application.Common.Interfaces.HR_Master;
using VenusHR.Core.Master;
using WorkFlow_EF;

namespace VenusHR.Application.Common.Interfaces
{
    // =============== QUERIES ===============
    public class GetAllDepartmentsQuery : IRequest<ApiResponse<object>>
    {
        public int Lang { get; set; }
        public GetAllDepartmentsQuery(int lang = 0) => Lang = lang;
    }

    public class GetDepartmentByIdQuery : IRequest<ApiResponse<object>>
    {
        public int Id { get; set; }
        public int Lang { get; set; }
        public GetDepartmentByIdQuery(int id, int lang = 0)
        {
            Id = id;
            Lang = lang;
        }
    }

    public class GetDepartmentsByCompanyQuery : IRequest<ApiResponse<object>>
    {
        public int CompanyId { get; set; }
        public int Lang { get; set; }
        public GetDepartmentsByCompanyQuery(int companyId, int lang = 0)
        {
            CompanyId = companyId;
            Lang = lang;
        }
    }

    // =============== HANDLER ===============
    public class DepartmentHandler :
        IRequestHandler<GetAllDepartmentsQuery, ApiResponse<object>>,
        IRequestHandler<GetDepartmentByIdQuery, ApiResponse<object>>,
        IRequestHandler<GetDepartmentsByCompanyQuery, ApiResponse<object>>
    {
        private readonly IHRMaster _hrMaster;

        public DepartmentHandler(IHRMaster hrMaster) => _hrMaster = hrMaster;

        private ApiResponse<object> ConvertToApiResponse(GeneralOutputClass<object> result)
        {
            if (result.ErrorCode == 0)
                return ApiResponse<object>.Succeeded(result.ResultObject, result.ErrorMessage ?? "Success");
            else
                return ApiResponse<object>.Failed(result.ErrorMessage ?? "Failed", result.ErrorCode.ToString());
        }

        public async Task<ApiResponse<object>> Handle(GetAllDepartmentsQuery request, CancellationToken ct)
            => ConvertToApiResponse(await _hrMaster.GetAllDepartmentsAsync(request.Lang));

        public async Task<ApiResponse<object>> Handle(GetDepartmentByIdQuery request, CancellationToken ct)
            => ConvertToApiResponse(await _hrMaster.GetDepartmentByIdAsync(request.Id, request.Lang));

        public async Task<ApiResponse<object>> Handle(GetDepartmentsByCompanyQuery request, CancellationToken ct)
            => ConvertToApiResponse(await _hrMaster.GetDepartmentsByCompanyAsync(request.CompanyId, request.Lang));
    }
}