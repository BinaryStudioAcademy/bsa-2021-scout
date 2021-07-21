using System;

namespace Application.Common.Exceptions
{
    public class NotFoundException: Exception
    {
        public NotFoundException(Type type, Guid id) :
            this($"Entity of type {type.Name} with id {id} is not found")
        { }
        public NotFoundException(string message) : base(message)
        { }
    }
}
