using ServiceMock.Tests.Common;

namespace ServiceMock.Tests;

public class ServiceMockWithSettingParametersTests
{
    [Test]
    public void CreatingAServiceWithSpecifiedParameters()
    {
        // Arrange
        var specifiedParameter = new Parameter();

        // Act / assert
        Assert.DoesNotThrow(() =>
        {
            _ = new ServiceMock<ServiceWithClassParameter>(options =>
                options.SetParameter(specifiedParameter));
        });
    }

    [Test]
    public void GettingSpecifiedParameter()
    {
        // Arrange
        var specifiedParameter = new Parameter();
        var service = new ServiceMock<ServiceWithClassParameter>(options =>
            options.SetParameter(specifiedParameter));

        // Act
        var selectedParameter = service.GetParameter<Parameter>();

        // Assert
        Assert.That(specifiedParameter, Is.EqualTo(selectedParameter));
    }
}