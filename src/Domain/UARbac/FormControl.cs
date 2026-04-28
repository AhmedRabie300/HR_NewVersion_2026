using Domain.Common;
using Domain.Common.Exceptions;

namespace Domain.UARbac;

public sealed class FormControl : LegacyEntity 
{
    public int FormId { get; private set; }
    public string Name { get; private set; } = default!;
    public string? FieldName { get; private set; } 
    public int? Section { get; private set; }

    public int? SearchID { get; private set; }
    public string? Format { get; set; }
    public string? EngCaption { get; private set; }
    public string? ArbCaption { get; private set; }

    public bool? Compulsory { get; private set; } = true;
    public bool? IsHide { get; private set; } = false;
    public bool? IsDisabled { get; private set; } = false;
    public int? Rank { get; set; }
    public DateTime RegDate { get; private set; }
    public DateTime? CancelDate { get; private set; }

    private FormControl() { }

    // If your app truly never creates these, you can keep this internal/private.
    internal FormControl(int formId, string name, string fieldName, int section)
    {
        SetRequired(formId, name, fieldName, section);
        RegDate = DateTime.Now;
    }

    public void UpdateUiSettings(string? engCaption, string? arbCaption, bool? isDisabled, bool? isHide, bool? isCompulsory)
    {
        EngCaption = Normalize(engCaption);
        ArbCaption = Normalize(arbCaption);

        if (isDisabled.HasValue) IsDisabled = isDisabled.Value;
        if (isHide.HasValue) IsHide = isHide.Value;
        if (isCompulsory.HasValue) Compulsory = isCompulsory.Value;
    }

    private void SetRequired(int formId, string name, string fieldName, int section)
    {
        if (formId <= 0) throw new DomainException("FormId is required.");
        FormId = formId;

        name = (name ?? "").Trim();
        if (string.IsNullOrWhiteSpace(name)) throw new DomainException("Name is required.");
        if (name.Length > 50) throw new DomainException("Name max length is 50.");
        Name = name;

        fieldName = (fieldName ?? "").Trim();
        if (string.IsNullOrWhiteSpace(fieldName)) throw new DomainException("FieldName is required.");
        if (fieldName.Length > 100) throw new DomainException("FieldName max length is 100.");
        FieldName = fieldName;

        if (section <= 0) throw new DomainException("Section is required.");
        Section = section;
    }

    private static string? Normalize(string? v) => string.IsNullOrWhiteSpace(v) ? null : v.Trim();
}
