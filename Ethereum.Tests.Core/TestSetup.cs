using System;
using Ethereum.Core;
using StructureMap;
using StructureMap.Graph;

namespace Ethereum.Tests
{
    public abstract class TestSetup
    {
        protected void Initialise()
        {
            ObjectFactory.Initialize(
                x => x.Scan(
                    y =>
                    {
                        y.AssembliesFromApplicationBaseDirectory(
                            a =>
                            a.FullName.StartsWith("Ethereum.", StringComparison.OrdinalIgnoreCase));

                        y.LookForRegistries();
                        y.AddAllTypesOf(typeof(IEventHandler<>));
                        y.WithDefaultConventions();
                    }));
        }
    }
}
