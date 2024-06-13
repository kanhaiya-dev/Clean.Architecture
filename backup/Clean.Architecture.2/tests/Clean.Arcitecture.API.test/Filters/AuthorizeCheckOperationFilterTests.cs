using Xunit;
using Moq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Clean.Architecture.API.Filter;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public class AuthorizeCheckOperationFilterTests
{
    [Fact]
    public void Apply_AddsSecurityRequirement_WhenAuthorizeAttributePresentOnMethod()
    {
        // Arrange
        var operation = new OpenApiOperation();
        var filterContext = CreateOperationFilterContext(typeof(AuthorizeMethodController).GetMethod(nameof(AuthorizeMethodController.AuthorizedMethod)));

        var filter = new AuthorizeCheckOperationFilter();

        // Act
        filter.Apply(operation, filterContext);

        // Assert
        Assert.NotNull(operation.Security);
        Assert.Single(operation.Security);
        var securityRequirement = operation.Security.First();
        Assert.Contains(securityRequirement, req => req.Key.Reference.Id == "Bearer");
    }

    [Fact]
    public void Apply_AddsSecurityRequirement_WhenAuthorizeAttributePresentOnClass()
    {
        // Arrange
        var operation = new OpenApiOperation();
        var filterContext = CreateOperationFilterContext(typeof(AuthorizeClassController).GetMethod(nameof(AuthorizeClassController.SomeMethod)));

        var filter = new AuthorizeCheckOperationFilter();

        // Act
        filter.Apply(operation, filterContext);

        // Assert
        Assert.NotNull(operation.Security);
        Assert.Single(operation.Security);
        var securityRequirement = operation.Security.First();
        Assert.Contains(securityRequirement, req => req.Key.Reference.Id == "Bearer");
    }

    [Fact]
    public void Apply_DoesNotAddSecurityRequirement_WhenAuthorizeAttributeNotPresent()
    {
        // Arrange
        var operation = new OpenApiOperation();
        var filterContext = CreateOperationFilterContext(typeof(NoAuthorizeController).GetMethod(nameof(NoAuthorizeController.NoAuthMethod)));

        var filter = new AuthorizeCheckOperationFilter();

        // Act
        filter.Apply(operation, filterContext);

        // Assert
        Assert.True(operation.Security.Count < 1);
    }

    private OperationFilterContext CreateOperationFilterContext(MethodInfo methodInfo)
    {
        var apiDescription = new Microsoft.AspNetCore.Mvc.ApiExplorer.ApiDescription();
        var schemaRepository = new SchemaRepository();
        var schemaGenerator = new Mock<ISchemaGenerator>();

        return new OperationFilterContext(apiDescription, schemaGenerator.Object, schemaRepository, methodInfo);
    }
}

// Helper classes to simulate controllers with different authorization attributes
public class AuthorizeMethodController
{
    [Authorize]
    public void AuthorizedMethod() { }
}

[Authorize]
public class AuthorizeClassController
{
    public void SomeMethod() { }
}

public class NoAuthorizeController
{
    public void NoAuthMethod() { }
}
