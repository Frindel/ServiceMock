# Mock dependency generator

The key class of the library is `ServiceMock`. It allows you to automatically create mock objects used as constructor parameters for the tested service.

For interface parameters, mock objects are created and are accessible through the `getParameterMock<TParameter>()` method.
You can access the actual object using the `getParameter<TParameter>()` method.

For class parameters, instances of these classes are created and can be accessed using the `getParameter<TParameter>()` method.

For class parameters, mock objects are not created.

Example:

```
AddNoteCommandHandler
└── NotesHelper
    ├── INotesRepository
    └── ILogger
```

```csharp
var handler = new ServiceMock<AddNoteCommandHandler>();

// retrieving the tested service
var service = handler.Service;

// retrieving the mock object of the ILogger interface
var loggerMock = handler.GetParameterMock<ILogger>();

// retrieving the actual ILogger object
var logger = handler.GetParameter<ILogger>();

// retrieving a class object
var notesHelper = handler.GetParameter<NotesHelper>();
```

## Defining interface implementations

To manually define an interface implementation, use the `SetParameter<TParameter>(TParameter parameter)` method:

```csharp
var handler = new ServiceMock<AddNoteCommandHanler>(options => {
    options.SetParameter<ILogger>(new Logger());    
})
```

## Limitations

If class parameters have multiple constructors or if value-type parameters are used in the constructor,
an `ArgumentException` will be thrown.
Such parameters must be explicitly defined:

## Using classes with multiple constructors / value-type parameters

```
AddNoteCommandHandler
└── NotesHelper
```

```csharp
class NotesHelper
{
    public NotesHelper(int storageSize) {...}
    public NotesHelper(INotesRepository notes) {...}
}
```

```csharp
const int storageSize = 1024;

var handler = new ServiceMock<AddNoteCommandHanler>(options => {
    options.SetParameter(new NotesHelper(storageSize));    
})
```

In the case where `NotesHelper` depends on `INotesRepository`,
an object implementing this interface can be obtained as follows:

```csharp
var handler = new ServiceMock<AddNoteCommandHanler>(options => {
    options.SetParameter(getParameter => {
        var notesRepository = getParameter<INotesRepository>();
        return new NotesHelper(notesRepository);
    }
})
```