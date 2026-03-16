    using System;
using System.Collections.Generic;
using System.Text;

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

        public ConflictException(string message) : base(message) { }
    }
}
