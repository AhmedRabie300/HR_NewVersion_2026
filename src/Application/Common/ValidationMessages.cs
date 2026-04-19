
using Application.Abstractions;
using Application.Common.Abstractions;

namespace Application.Common;

public sealed class ValidationMessages : IValidationMessages
{
    private readonly ILocalizationService _localizer;
    private readonly ICurrentUser _currentUser;

    public ValidationMessages(ILocalizationService localizer, ICurrentUser currentUser)
    {
        _localizer = localizer;
        _currentUser = currentUser;
    }

    public string Get(string key)
        => _localizer.GetMessage(key, _currentUser.Language);

    public string Format(string key, params ReadOnlySpan<object> args)
    {
        var template = _localizer.GetMessage(key, _currentUser.Language);
        return args.Length switch
        {
            0 => template,
            1 => string.Format(template, args[0]),
            2 => string.Format(template, args[0], args[1]),
            3 => string.Format(template, args[0], args[1], args[2]),
            _ => string.Format(template, args.ToArray())
        };
    }

    public string CodeExists(string entityKey, string code)
        => Format("CodeExists", Get(entityKey), code);

    public string NotFound(string entityKey, object id)
        => Format("NotFound", Get(entityKey), id);

    public string CreatedSuccessfully(string entityKey)
        => Format("CreatedSuccessfully", Get(entityKey));

    public string UpdatedSuccessfully(string entityKey)
        => Format("UpdatedSuccessfully", Get(entityKey));

    public string DeletedSuccessfully(string entityKey)
        => Format("DeletedSuccessfully", Get(entityKey));

    public string CannotDeleteHasChildren(string entityKey)
        => Format("CannotDeleteHasChildren", Get(entityKey));
}