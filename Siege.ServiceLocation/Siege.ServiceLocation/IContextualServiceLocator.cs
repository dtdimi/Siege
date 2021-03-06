using System;
using System.Collections.Generic;

namespace Siege.ServiceLocation
{
    public interface IContextualServiceLocator : IServiceLocator
    {
        IList<object> Context { get; }
        void AddContext(object contextItem);
        IList<IUseCase> GetRegisteredUseCasesForType(Type type);
    }
}