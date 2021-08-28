using System;

namespace Application.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(Type type, string propertyValue, string propertyName = "id") :
            this($"Entity of type {type.Name} with {propertyName} {propertyValue} is not found")
        { }
        public NotFoundException(string message) : base(message)
        { }
    }
}
