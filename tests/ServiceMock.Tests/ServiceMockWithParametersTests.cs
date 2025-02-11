using ServiceMock.Tests.Common;

namespace ServiceMock.Tests;

public class ServiceMockWithParametersTests
{
    [Test]
    public void CreatingAServiceWithAClassParameter()
    {
        // Act / assert
        Assert.DoesNotThrow(() => { _ = new ServiceMock<ServiceWithClassParameter>(); });
    }

    [Test]
    public void GettingParameterClass()
    {
        // Arrange
        var service = new ServiceMock<ServiceWithClassParameter>();

        // Act / assert
        Assert.DoesNotThrow(() => { _ = service.GetParameter<Parameter>(); });
    }

    [Test]
    public void GettingParameterClassMock()
    {
        // Arrange
        var service = new ServiceMock<ServiceWithClassParameter>();

        // Act / assert
        Assert.Throws<ArgumentException>(() => { _ = service.GetParameterMock<Parameter>(); });
    }

    [Test]
    public void CreatingAServiceWithAInterfaceParameter()
    {
        // Act / assert
        Assert.DoesNotThrow(() => { _ = new ServiceMock<ServiceWithInterfaceParameter>(); });
    }

    [Test]
    public void GettingParameterInterface()
    {
        // Arrange
        var service = new ServiceMock<ServiceWithInterfaceParameter>();

        // Act / assert
        Assert.DoesNotThrow(() => { _ = service.GetParameter<IParameter>(); });
    }

    [Test]
    public void GettingParameterInterfaceMock()
    {
        // Arrange
        var service = new ServiceMock<ServiceWithInterfaceParameter>();

        // Act / assert
        Assert.DoesNotThrow(() => { _ = service.GetParameterMock<IParameter>(); });
    }
}