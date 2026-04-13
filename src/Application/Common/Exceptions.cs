using System;

namespace Application.Common
{
    public class NotFoundException : Exception
    {
        public string ResourceName { get; }
        public object ResourceId { get; }

        public NotFoundException(string resourceName, object resourceId)
            : base($"{resourceName} with identifier {resourceId} was not found.")
        {
            ResourceName = resourceName;
            ResourceId = resourceId;
        }

        public NotFoundException(string resourceName, object resourceId, string message)
            : base(message)
        {
            ResourceName = resourceName;
            ResourceId = resourceId;
        }
    }

    public class ConflictException : Exception
    {
        public string ResourceName { get; }
        public string FieldName { get; }
        public string FieldValue { get; }

        public ConflictException(string resourceName, string fieldName, string fieldValue)
            : base($"{resourceName} with {fieldName} '{fieldValue}' already exists.")
        {
            ResourceName = resourceName;
            FieldName = fieldName;
            FieldValue = fieldValue;
        }

        public ConflictException(string resourceName, string fieldName, string fieldValue, string message)
            : base(message)
        {
            ResourceName = resourceName;
            FieldName = fieldName;
            FieldValue = fieldValue;
        }

        public ConflictException(string message) : base(message) { }
    }

    public class RequiredFieldException : Exception
    {
        public string ResourceName { get; }
        public string FieldName { get; }
        public string FieldDisplayName { get; }

        public RequiredFieldException(string resourceName, string fieldName, string message)
            : base(message)
        {
            ResourceName = resourceName;
            FieldName = fieldName;
            FieldDisplayName = fieldName;
        }

        public RequiredFieldException(string resourceName, string fieldName, string fieldDisplayName, string message)
            : base(message)
        {
            ResourceName = resourceName;
            FieldName = fieldName;
            FieldDisplayName = fieldDisplayName;
        }
    }
}