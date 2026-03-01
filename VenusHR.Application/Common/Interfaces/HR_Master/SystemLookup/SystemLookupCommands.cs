using MediatR;
using VenusHR.Application.Common.DTOs.Shared;
using VenusHR.Application.Common.Interfaces.HR_Master;
using VenusHR.Core.Master;
using WorkFlow_EF;

namespace VenusHR.Application.Common.Interfaces
{
    // =============== QUERIES ===============
    public class GetSystemLookupsQuery : IRequest<ApiResponse<object>>
    {
        public int Lang { get; set; }
        public GetSystemLookupsQuery(int lang = 0) => Lang = lang;
    }

    public class GetLookupByTypeQuery : IRequest<ApiResponse<object>>
    {
        public string LookupType { get; set; }
        public int Lang { get; set; }
        public GetLookupByTypeQuery(string lookupType, int lang = 0)
        {
            LookupType = lookupType;
            Lang = lang;
        }
    }

    public class SearchLookupsQuery : IRequest<ApiResponse<object>>
    {
        public string SearchTerm { get; set; }
        public string? LookupType { get; set; }
        public int Lang { get; set; }
        public SearchLookupsQuery(string searchTerm, string? lookupType = null, int lang = 0)
        {
            SearchTerm = searchTerm;
            LookupType = lookupType;
            Lang = lang;
        }
    }

    // =============== HANDLER ===============
    public class SystemLookupHandler :
        IRequestHandler<GetSystemLookupsQuery, ApiResponse<object>>,
        IRequestHandler<GetLookupByTypeQuery, ApiResponse<object>>,
        IRequestHandler<SearchLookupsQuery, ApiResponse<object>>
    {
        private readonly IHRMaster _hrMaster;

        public SystemLookupHandler(IHRMaster hrMaster) => _hrMaster = hrMaster;

        private ApiResponse<object> ConvertToApiResponse(GeneralOutputClass<object> result)
        {
            if (result.ErrorCode == 0)
                return ApiResponse<object>.Succeeded(result.ResultObject, result.ErrorMessage ?? "Success");
            else
                return ApiResponse<object>.Failed(result.ErrorMessage ?? "Failed", result.ErrorCode.ToString());
        }

        public async Task<ApiResponse<object>> Handle(GetSystemLookupsQuery request, CancellationToken ct)
            => ConvertToApiResponse(await _hrMaster.GetSystemLookupsAsync(request.Lang));

        public async Task<ApiResponse<object>> Handle(GetLookupByTypeQuery request, CancellationToken ct)
            => ConvertToApiResponse(await _hrMaster.GetLookupByTypeAsync(request.LookupType, request.Lang));

        public async Task<ApiResponse<object>> Handle(SearchLookupsQuery request, CancellationToken ct)
            => ConvertToApiResponse(await _hrMaster.SearchLookupsAsync(request.SearchTerm, request.LookupType, request.Lang));
    }
}