using System;
using System.Collections.Generic;
using System.Text;

namespace Application.System.MasterData.Abstractions
{
    public interface IBloodGroupRepository
    {
        Task<object> GetDetailsForEndofservuice(int empid);
        Task<object> GetDetailsForEndofservuiceNonCommon(int empid);
    }
}
