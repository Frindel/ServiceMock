namespace ServiceMock;

public class ParameterFactory
{
    public Type ParameterType { get; }

    public Func<Func<Type, object>, object> Implementation { get; }

    public ParameterFactory(Type parameterType, Func<Func<Type, object>, object> implementation)
    {
        ParameterType = parameterType;
        Implementation = implementation;
    }
}