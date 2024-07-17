using Demo_Delivery.Api;
using Demo_Delivery.Application.CQRS;
using Demo_Delivery.Infrastructure.Data;
using Demo_Delivery.Infrastructure.TestBase;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;

namespace Demo_Delivery.IntegrationTests;

[Collection(IntegrationTestCollection.Name)]
public class DemoDeliveryIntegrationTestBase : TestBase<Demo_Delivery.Api.Program, ApplicationDbContext>
{
    public DemoDeliveryIntegrationTestBase(TestFixture<Program, ApplicationDbContext> integrationTestFixture) : base(integrationTestFixture)
    {
    }
    
     
}

 

[CollectionDefinition(Name)]
public class
    IntegrationTestCollection : ICollectionFixture<TestFixture<Demo_Delivery.Api.Program, ApplicationDbContext>>
{
    public const string Name = "Demo-Delivery Integration Tests";
}