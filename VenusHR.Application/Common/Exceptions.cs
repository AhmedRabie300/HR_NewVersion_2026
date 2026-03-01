using System;
using System.Collections.Generic;
using System.Text;

namespace VenusHR.Application.Common
{
    public sealed class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }

    public sealed class ConflictException : Exception
    {
        public ConflictException(string message) : base(message) { }
    }
}
