using Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Common
{
    public sealed class CurrentCompany : ICurrentCompany
    {
        public int CompanyId { get; private set; } = default!;

        public void SetCompanyId(int companyId)
        {
            CompanyId = companyId;
        }
    }
}
