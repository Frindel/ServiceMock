using ServiceMock.Tests.Common;

namespace ServiceMock.Tests;

public class ServiceMockWithoutParametersTests
{
    [Test]
    public void SuccessfulCreatingService()
    {
        // Act / assert
        Assert.DoesNotThrow(() => { _ = new ServiceMock<ServiceMockWithSettingParametersTests>(); });
    }

    [Test]
    public void GettingANonExistentParameterClass()
    {
        // Arrange
        var service = new ServiceMock<ServiceMockWithSettingParametersTests>();

        // Act / assert
        Assert.Throws<ArgumentException>(() => service.GetParameter<Parameter>());
    }


    [Test]
    public void GettingANonExistentParameterInterface()
    {
        // Arrange
        var service = new ServiceMock<ServiceMockWithSettingParametersTests>();

        // Act / assert
        Assert.Throws<ArgumentException>(() => service.GetParameter<IParameter>());
    }

    [Test]
    public void GettingANonExistentParameterClassMock()
    {
        // Arrange
        var service = new ServiceMock<ServiceMockWithSettingParametersTests>();

        // Act / assert
        Assert.Throws<ArgumentException>(() => service.GetParameterMock<Parameter>());
    }


    [Test]
    public void GettingANonExistentParameterInterfaceMock()
    {
        // Arrange
        var service = new ServiceMock<ServiceMockWithSettingParametersTests>();

        // Act / assert
        Assert.Throws<ArgumentException>(() => service.GetParameterMock<IParameter>());
    }
}