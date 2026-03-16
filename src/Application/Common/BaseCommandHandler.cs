using Application.Common.Abstractions;

namespace Application.Common.BaseHandlers
{
    public abstract class BaseCommandHandler
    {
        protected readonly ILocalizationService _localization;

        protected BaseCommandHandler(ILocalizationService localization)
        {
            _localization = localization;
        }

        protected string GetMessage(string key, int lang)
        {
            return _localization.GetMessage(key, lang);
        }

        protected string GetFormattedMessage(string key, int lang, params object[] args)
        {
            var message = _localization.GetMessage(key, lang);
            return string.Format(message, args);
        }
    }
}