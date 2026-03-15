using Application.System.MasterData.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Repositories.System
{
    public class BloodGroupRepository : IBloodGroupRepository
    {
        public Task<object> GetDetailsForEndofservuice(int empid)
        {
            throw new NotImplementedException();//common
        }
     

        public Task<object> GetDetailsForEndofservuiceNonCommon(int empid)
        {
            throw new NotImplementedException();
        }
    }
}
