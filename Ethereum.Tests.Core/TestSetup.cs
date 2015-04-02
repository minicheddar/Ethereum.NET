using System;
using System.Collections.Generic;
using System.Linq;
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
                        //y.AddAllTypesOf(typeof(IHandler<>));
                        y.WithDefaultConventions();
                    }));
        }
    }
}
