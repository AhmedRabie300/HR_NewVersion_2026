using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Abstractions
{
    public interface ILocalizationService
    {
        string GetMessage(string key, int lang);
        string GetMessage(string key, string lang);
        string this[string key, int lang] { get; }
    }
}
