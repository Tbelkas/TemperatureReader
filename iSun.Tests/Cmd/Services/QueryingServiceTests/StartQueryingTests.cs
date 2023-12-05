using iSun.Cmd.Services;
using iSun.Domain.Models;
using iSun.Domain.Models.Responses;
using iSun.Domain.Services;
using iSun.Domain.Services.External;
using iSun.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace iSun.Tests.Cmd.Services.QueryingServiceTests;

public class StartQueryingTests
{
    private IConfigurationRoot _configurationRoot;

    public StartQueryingTests()
    {
        _configurationRoot = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
    }

    [Fact]
    public async Task AuthenticationFails_ReturnsUnsuccessfulResponse()
    {
        // Arrange
        var authServiceMock = new Mock<IAuthService>();
        authServiceMock.Setup(x => x.GetToken(_configurationRoot["ApiCredentials:Username"]!, _configurationRoot["ApiCredentials:Password"]!))
            .ReturnsAsync(Response<AuthorizeResponseModel>.Error("Auth failed"));
        
        var sut = PrepareSut(authServiceMock: authServiceMock);

        // Act
        var result = await sut.StartQuerying(["Vilnius", "Kaunas"]);

        // Assert
        authServiceMock.Verify(x => x.GetToken(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(1));
        Assert.False(result.isSuccess);
    }

    private IQueryingService PrepareSut(Mock<IContextService>? contextServiceMock = null,
        Mock<ITemperatureService>? temperatureServiceMock = null,
        Mock<IAuthService>? authServiceMock = null,
        Mock<ILogger<IQueryingService>>? loggerMock = null,
        Mock<ITemperatureRepository>? temperatureRepositoryMock = null)
    {
        contextServiceMock ??= new Mock<IContextService>();
        temperatureServiceMock ??= new Mock<ITemperatureService>();
        authServiceMock ??= new Mock<IAuthService>();
        loggerMock ??= new Mock<ILogger<IQueryingService>>();
        temperatureRepositoryMock ??= new Mock<ITemperatureRepository>();

        return new QueryingService(_configurationRoot, contextServiceMock.Object, temperatureServiceMock.Object, authServiceMock.Object, loggerMock.Object, temperatureRepositoryMock.Object);
    }
}