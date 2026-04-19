using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Abstractions
{
    public interface IValidationMessages
    {
        string Get(string key);
        string Format(string key, params ReadOnlySpan<object> args);

        // Common patterns used across all handlers
        string CodeExists(string entityKey, string code);
        string NotFound(string entityKey, object id);
        string CreatedSuccessfully(string entityKey);
        string UpdatedSuccessfully(string entityKey);
        string DeletedSuccessfully(string entityKey);
        string CannotDeleteHasChildren(string entityKey);
    }
}
