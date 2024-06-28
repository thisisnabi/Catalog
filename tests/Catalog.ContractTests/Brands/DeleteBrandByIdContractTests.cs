using Catalog.Apis;
using Catalog.Infrastructure;
using Catalog.Models;
using Catalog.Services;
using FluentAssertions;
using MassTransit;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace Catalog.ContractTests.Brands;

public class DeleteBrandByIdContractTests
{
    private CatalogServices SetupServices(string databaseName)
    {
        var options = new DbContextOptionsBuilder<CatalogDbContext>()
                        .UseInMemoryDatabase(databaseName)
                        .Options;

        var optionsMock = new Mock<IOptions<CatalogOptions>>();
        var loggerMock = new Mock<ILogger<CatalogServices>>();
        var publisherMock = new Mock<IPublishEndpoint>();

        var context = new CatalogDbContext(options);
        var mockServices = new CatalogServices(context, optionsMock.Object, loggerMock.Object, publisherMock.Object);

        return mockServices;
    }

    [Fact]
    public async Task DeleteBrandById_InvalidId_ReturnsBadRequest()
    {
        // Arrange
        var mockServices = SetupServices(databaseName: "DeleteBrandById_InvalidId_ReturnsBadRequest");

        // Act
        var results = await CatalogBrandApi.DeleteBrandById(mockServices, -1, CancellationToken.None);

        // Assert
        results.Result.Should().BeOfType<BadRequest<string>>()
                       .Which.Value.Should().Be("Id is not valid.");
    }

    [Fact]
    public async Task DeleteBrandById_BrandNotFound_ReturnsNotFound()
    {
        // Arrange
        var mockServices = SetupServices(databaseName: "DeleteBrandById_BrandNotFound_ReturnsNotFound");

        // Act
        var results = await CatalogBrandApi.DeleteBrandById(mockServices, 999, CancellationToken.None);

        // Assert
        results.Result.Should().BeOfType<NotFound>();
    }

    [Fact]
    public async Task DeleteBrandById_ValidId_ReturnsNoContent()
    {
        // Arrange
        var mockServices = SetupServices(databaseName: "DeleteBrandById_ValidId_ReturnsNoContent");

        // Adding a brand to the in-memory database
        var context = mockServices.Context;
        var brand = CatalogBrand.Create("Test Brand");

        context.CatalogBrands.Add(brand);
        await context.SaveChangesAsync();

        // Act
        var results = await CatalogBrandApi.DeleteBrandById(mockServices, brand.Id, CancellationToken.None);

        // Assert
        results.Result.Should().BeOfType<NoContent>();
        var brandFind = await context.CatalogBrands.FindAsync(brand.Id);
        brandFind.Should().BeNull();
    }
}
