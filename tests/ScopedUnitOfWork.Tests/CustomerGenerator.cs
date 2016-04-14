using System;
using System.Collections.Generic;
using ScopedUnitOfWork.Tests.AcceptanceTests.SampleApplication.SimpleDomain;

namespace ScopedUnitOfWork.Tests
{
    public static class CustomerGenerator
    {
        // will take care that names are unique•
        private static readonly IList<string> AlreadyGeneratedNames = new List<string>();

        public static Customer Generate()
        {
            var newName = Faker.NameFaker.Name();

            while (AlreadyGeneratedNames.Contains(newName))
            {
                newName = Faker.NameFaker.Name();
            }

            AlreadyGeneratedNames.Add(newName);

            Console.WriteLine("Created customer with name: " + newName);
            return new Customer { Name = newName };
        }
    }
}