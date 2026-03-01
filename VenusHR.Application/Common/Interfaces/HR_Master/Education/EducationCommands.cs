using MediatR;
using VenusHR.Application.Common.DTOs.Shared;
using VenusHR.Application.Common.Interfaces.HR_Master;
using VenusHR.Core.Master;
using WorkFlow_EF;

namespace VenusHR.Application.Common.Interfaces.HR_Master.Education
{
     public class GetAllEducationsQuery : IRequest<ApiResponse<object>>
    {
        public int Lang { get; set; }
        public GetAllEducationsQuery(int lang = 0) => Lang = lang;
    }

    public class GetEducationByIdQuery : IRequest<ApiResponse<object>>
    {
        public int Id { get; set; }
        public int Lang { get; set; }
        public GetEducationByIdQuery(int id, int lang = 0)
        {
            Id = id;
            Lang = lang;
        }
    }

     public class CreateEducationCommand : IRequest<ApiResponse<object>>
    {
        public hrs_Educations Education { get; set; }
        public CreateEducationCommand(hrs_Educations education) => Education = education;
    }

    public class UpdateEducationCommand : IRequest<ApiResponse<object>>
    {
        public int Id { get; set; }
        public hrs_Educations Education { get; set; }
        public UpdateEducationCommand(int id, hrs_Educations education)
        {
            Id = id;
            Education = education;
        }
    }

     public class EducationHandler :
        IRequestHandler<GetAllEducationsQuery, ApiResponse<object>>,
        IRequestHandler<GetEducationByIdQuery, ApiResponse<object>>,
        IRequestHandler<CreateEducationCommand, ApiResponse<object>>,
        IRequestHandler<UpdateEducationCommand, ApiResponse<object>>
    {
        private readonly IHRMaster _hrMaster;

        public EducationHandler(IHRMaster hrMaster) => _hrMaster = hrMaster;

        private ApiResponse<object> ConvertToApiResponse(GeneralOutputClass<object> result)
        {
            if (result.ErrorCode == 0)
                return ApiResponse<object>.Succeeded(result.ResultObject, result.ErrorMessage ?? "Success");
            else
                return ApiResponse<object>.Failed(result.ErrorMessage ?? "Failed", result.ErrorCode.ToString());
        }

        public async Task<ApiResponse<object>> Handle(GetAllEducationsQuery request, CancellationToken ct)
            => ConvertToApiResponse(await _hrMaster.GetAllEducationsAsync(request.Lang));

        public async Task<ApiResponse<object>> Handle(GetEducationByIdQuery request, CancellationToken ct)
            => ConvertToApiResponse(await _hrMaster.GetEducationByIdAsync(request.Id, request.Lang));

        public async Task<ApiResponse<object>> Handle(CreateEducationCommand request, CancellationToken ct)
            => ConvertToApiResponse(await _hrMaster.CreateEducationAsync(request.Education));

        public async Task<ApiResponse<object>> Handle(UpdateEducationCommand request, CancellationToken ct)
            => ConvertToApiResponse(await _hrMaster.UpdateEducationAsync(request.Id, request.Education));
    }
}
