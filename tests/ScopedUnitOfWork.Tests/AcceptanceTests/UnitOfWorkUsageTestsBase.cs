using System;
using FluentAssertions;
using NUnit.Framework;
using ScopedUnitOfWork.Interfaces;
using ScopedUnitOfWork.Interfaces.Exceptions;
using ScopedUnitOfWork.Tests.AcceptanceTests.SampleApplication.SimpleDomain;

namespace ScopedUnitOfWork.Tests.AcceptanceTests
{
    [TestFixture]
    public abstract class UnitOfWorkUsageTestsBase : AcceptanceTestBase
    {
        [Test]
        public void MultipleCommitCallsShouldFail()
        {
            using (var uow = GetFactory().Create())
            {
                uow.Commit();

                Action action = () => uow.Commit();
                action.Should().Throw<InvalidOperationException>("*already commited*");
            }
        }

        [Test]
        public void MultipleDisposeCallsShouldWork()
        {
            var uow = GetFactory().Create();

            Action action = () =>
            {
                uow.Dispose();
                uow.Dispose();
                uow.Dispose();
            };

            action.Should().NotThrow();
        }
         
        [Test]
        public void UsingUnitsOfWorkWithoutUsingStatementShouldFail()
        {
            // using "using" statements guarantees stack-like nature of units of work, which is 
            // how it is internally implemented. This means disposing outside uow before inner is disposed
            // is not allowed!

            // case 1
            using (var uow = GetFactory().Create())
            {
                using (GetFactory().Create())
                {
                    Action action1 = () => uow.Dispose();
                    action1.Should().Throw<IncorrectUnitOfWorkUsageException>();
                }
            }

            // case 2
            var uowA = GetFactory().Create();
            var uowB = GetFactory().Create();

            Action action2 = () => uowA.Dispose();
            action2.Should().Throw<IncorrectUnitOfWorkUsageException>();

            // should be fine
            uowB.Dispose();
            uowA.Dispose();
        }

        [Test]
        public void Create_ShouldThrow_WhenRequestingTransactionalButTransactionsNotSupported()
        {
            var uow = GetFactory().Create();

            Action action = () =>
            {
                uow.Dispose();
                uow.Dispose();
                uow.Dispose();
            };

            action.Should().NotThrow();
        }

        [Test]
        public void Commit_ShouldThrow_WhenInnerTransactionalUnitOfWorkRolledBack()
        {
            using (var uow = GetFactory().Create(ScopeType.Transactional))
            {
                using (GetFactory().Create(ScopeType.Transactional))
                {
                    // not commit called here, means will rollback (dispose)
                }

                Action action = () => uow.Commit();
                action.Should().Throw<TransactionFailedException>();
            }
        }

        [Test]
        public void Commit_ShouldThrow_WhenUnitOfWorkAlreadyDisposed()
        {
            var uow = GetFactory().Create();
            uow.Dispose();

            Action action = () => uow.Commit();
            action.Should().Throw<InvalidOperationException>("*disposed*");
        }

        [Test]
        public void Commit_ShouldNotThrow_WhenInnerTransactionalUnitOfWorkRolledBack_ButItselfIsNotTransactional()
        {
            // note here: not transactional!
            using (var uow = GetFactory().Create())
            {
                using (GetFactory().Create(ScopeType.Transactional))
                {
                    // not commit called here, means will rollback (dispose)
                }

                Action action = () => uow.Commit();
                action.Should().NotThrow();
            }
        }

        [Test]
        public void ShouldRollbackCorrectlyWhenExplodesDueToExceptions()
        {
            var shouldNotBeThereCustomer = CustomerGenerator.Generate();

            using (IUnitOfWork uowOutter = GetFactory().Create(ScopeType.Transactional))
            {
                uowOutter.GetRepository<ICustomerRepository>()
                    .Add(shouldNotBeThereCustomer);

                using (IUnitOfWork uowInner = GetFactory().Create())
                {
                    try
                    {
                        uowInner.GetRepository<ICustomerRepository>()
                            .Add(new Customer()); // well this should fail
                        uowInner.Commit();
                    }
                    catch (Exception)
                    {
                        // ignored completely. Altough supressing all
                        // it is the only way to support all implementations
                        // since they differ in exceptions thrown
                    }
                }

                try
                {
                    uowOutter.Commit();
                }
                catch (Exception)
                {
                    // ignored completely.
                }
            }

            using (IUnitOfWork uow = GetFactory().Create())
            {
                uow.GetRepository<ICustomerRepository>()
                    .FindByName(shouldNotBeThereCustomer.Name)
                    .Should().BeNull();
            }
        }

        [Test]
        public void ComplexUsageScenario()
        {
            var firstCustomer = CustomerGenerator.Generate();
            var secondCustomer = CustomerGenerator.Generate();
            var thirdCustomer = CustomerGenerator.Generate();

            // Arrange
            using (var uow = GetFactory().Create())
            {
                uow.GetRepository<ICustomerRepository>().Add(firstCustomer);
                uow.Commit();
            }

            // Act
            // our controller or some other top level service (maybe on application services layer)
            // opens a spanning context 
            using (var spanningUnitOfWork = GetFactory().Create())
            {
                var repository = spanningUnitOfWork.GetRepository<ICustomerRepository>();
                Customer customer = repository.FindByName(firstCustomer.Name);

                // let's say this was a authorization check or something
                customer.Name.Should().Be(firstCustomer.Name);

                // now we want to call a business service which would have its own transactional unit of work in there
                using (var businessUnitOfWork = GetFactory().Create(ScopeType.Transactional))
                {
                    using (var firstDomainService = GetFactory().Create())
                    {
                        firstDomainService.GetRepository<ICustomerRepository>()
                            .Add(secondCustomer);

                        firstDomainService.Commit();
                    }

                    using (var secondDomainService = GetFactory().Create())
                    {
                        secondDomainService.GetRepository<ICustomerRepository>()
                            .Add(thirdCustomer);

                        secondDomainService.Commit();
                    }

                    // this should commit the transaction as its the top most transactional uow
                    businessUnitOfWork.Commit();
                }
            }

            // Assert
            using (var verificationUoW = GetFactory().Create())
            {
                verificationUoW.GetRepository<ICustomerRepository>()
                    .FindByName(secondCustomer.Name).Should().NotBeNull();
                verificationUoW.GetRepository<ICustomerRepository>()
                    .FindByName(thirdCustomer.Name).Should().NotBeNull();
            }
        }
    }
}