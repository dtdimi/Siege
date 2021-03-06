﻿using System;
using System.Collections.Generic;
using System.Linq;
using Siege.ServiceLocation;
using StructureMap.Attributes;
using StructureMap.Configuration.DSL;
using StructureMap.Pipeline;

namespace Siege.Container.UnitTests.RegistrationExtensions.StructureMap
{
    public class DecoratorUseCaseBinding<TService> : IDecoratorUseCaseBinding<TService>
    {
        private global::StructureMap.Container container;
        private IServiceLocatorAdapter locator;

        public DecoratorUseCaseBinding(global::StructureMap.Container container, IServiceLocatorAdapter locator)
        {
            this.container = container;
            this.locator = locator;
        }

        public void Bind(IUseCase useCase)
        {
            Bind((IDecoratorUseCase<TService>)useCase);
        }

        public object Resolve(Type typeToResolve, Type argumentType, object rootObject)
        {
            string parameterName = typeToResolve.GetConstructor(new[] { argumentType }).GetParameters().Where(parameter => parameter.ParameterType == argumentType).First().Name;

            Dictionary<string, object> dictionary = new Dictionary<string, object> { { parameterName, rootObject } };
            return container.GetInstance(typeToResolve, new ExplicitArguments(dictionary));
        }

        private void Bind(IDecoratorUseCase<TService> useCase)
        {
            Registry registry = new Registry();

            //registry.ForRequestedType(useCase.GetBoundType()).CacheBy(InstanceScope.PerRequest).TheDefaultIsConcreteType(useCase.GetBoundType());
            registry.ForRequestedType(useCase.GetDecoratorType()).CacheBy(InstanceScope.PerRequest).TheDefaultIsConcreteType(useCase.GetDecoratorType());
            
            container.Configure(configure => configure.AddRegistry(registry));
        }
    }
}