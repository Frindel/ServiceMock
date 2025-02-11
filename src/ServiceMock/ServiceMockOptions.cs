namespace ServiceMock;

public class ServiceMockOptions
{
    public readonly IList<ParameterFactory> Parameters = new List<ParameterFactory>();

    public void SetParameter<TParameter>(TParameter parameter)
        where TParameter : class
    {
        SetParameter(_ => parameter);
    }

    public void SetParameter<TParameter>(Func<Func<Type, object>, TParameter> implementationFactory)
        where TParameter : class
    {
        Parameters.Add(new ParameterFactory(typeof(TParameter), implementationFactory));
    }
}