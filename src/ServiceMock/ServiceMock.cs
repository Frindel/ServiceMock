using Moq;

namespace ServiceMock;

public class ServiceMock<TService>
    where TService : class
{
    private Dictionary<Type, Func<Func<Type, object>, object>> _parameterFactories = null!;
    private readonly Dictionary<Type, Parameter> _parameters = new();

    public TService Service = null!;

    public ServiceMock(Action<ServiceMockOptions>? configure = null!)
    {
        Configure(configure);
        InstantiateService();
    }

    private void Configure(Action<ServiceMockOptions>? configure)
    {
        var options = new ServiceMockOptions();
        configure?.Invoke(options);
        _parameterFactories = options
            .Parameters
            .ToDictionary(factory => factory.ParameterType, factory => factory.Implementation);
    }

    #region Service Creation

    void InstantiateService()
    {
        TService serviceObj = (TService)InstantiateObject(typeof(TService)).Value;
        Service = serviceObj;
    }

    Parameter InstantiateObject(Type classType)
    {
        Type[] constructorParameterTypes = GetConstructorParameterTypes(classType);
        Parameter[] parameters = CreateAndStoreParameters(constructorParameterTypes);

        var constructor = classType.GetConstructors()[0];
        var createdObj = new Parameter(
            constructor.Invoke(GetParameterValues(parameters)));
        return createdObj;
    }

    Type[] GetConstructorParameterTypes(Type classType)
    {
        var constructors = classType.GetConstructors();
        if (constructors.Length > 1)
            throw new ArgumentException($"The number of class {classType} constructors is more than one");
        return constructors[0]
            .GetParameters().Select(p => p.ParameterType).ToArray();
    }

    Parameter[] CreateAndStoreParameters(Type[] parameterTypes)
    {
        return parameterTypes
            .Select(parameterType => CreateAndStoreParameter(parameterType)!)
            .ToArray();
    }

    Parameter CreateAndStoreParameter(Type parameterType)
    {
        // return the already saved parameter if it exists
        if (_parameters.TryGetValue(parameterType, out var existingParameter))
            return existingParameter;

        var parameter = CreateParameterInstance(parameterType);
        StoreParameter(parameter);

        return parameter;
    }

    Parameter CreateParameterInstance(Type parameterType)
    {
        if (_parameterFactories.TryGetValue(parameterType, out var specifiedParameter))
            return new Parameter(specifiedParameter.Invoke(CreateAndStoreParameter));

        if (parameterType.IsClass)
            return InstantiateObject(parameterType);

        if (parameterType.IsInterface)
            return new Parameter(CreateMock(parameterType), true);

        throw new ArgumentException(
            "The parameter is a structure. The objects that use these parameters must be set explicitly.");
    }

    Mock CreateMock(Type mockType)
    {
        return (Mock)Activator.CreateInstance(typeof(Mock<>)
            .MakeGenericType(mockType))!;
    }

    void StoreParameter(Parameter parameterToStore)
    {
        if (parameterToStore.IsMock)
            _parameters[parameterToStore.Value.GetType()] = parameterToStore;

        _parameters[parameterToStore.Type] = parameterToStore;
    }

    object[] GetParameterValues(Parameter[] parameters)
    {
        return parameters.Select(parameter =>
            parameter.IsMock ? (parameter.Value as Mock)!.Object : parameter.Value
        ).ToArray();
    }

    #endregion

    #region Accessors

    public Mock<TMock> GetParameterMock<TMock>() where TMock : class
    {
        return GetParameter<Mock<TMock>>();
    }

    public TParameter GetParameter<TParameter>() where TParameter : class
    {
        Type parameterType = typeof(TParameter);
        if (!_parameters.TryGetValue(parameterType, out var parameter))
            throw new ArgumentException($"{parameterType} not found");

        if (parameter.IsMock && parameter.Type == parameterType)
            return ((parameter.Value as Mock)!.Object as TParameter)!;

        return (TParameter)parameter.Value;
    }

    #endregion

    private class Parameter
    {
        public Object Value { get; private set; }

        public Type Type { get; }

        public bool IsMock { get; }

        public Parameter(object value, bool isMock = false)
        {
            IsMock = isMock;
            Value = value;
            Type = isMock ? value.GetType().GetGenericArguments()[0] : value.GetType();
        }
    }
}