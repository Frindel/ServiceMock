# Генератор mock-зависимостей

Ключевым классом библиотеки является `ServiceMock`. Он позволяет автоматически создавать mock-объекты, используемые в
качестве параметров конструктора, тестируемого сервиса.

Для параметров-интерфейсов создаются mock-объекты, которые доступны через метод `getParameterMock<TParameter>()`.
Получить доступ к самому объекту можно с помощью метода `getParameter<TParameter>()`

Для параметров-классов создаются экземпляры этих классов, доступные с помощью метода `getParameter<TParameter>()`.

Для параметров-классов, не создаются mock-объекты.

Пример:

```
AddNoteCommandHandler
└── NotesHelper
    ├── INotesRepository
    └── ILogger
```

```csharp
var handler = new ServiceMock<AddNoteCommandHandler>();

// получение тестируемого сервиса
var service = handler.Service;

// получение mock-объекта интерфейса ILogger
var loggerMock = handler.GetParameterMock<ILogger>();

// получение объекта интерейса ILogger
var logger = handler.GetParameter<ILogger>();

// получение объекта класса
var notesHelper = handler.GetParameter<NotesHelper>();
```

## Задание реализации интерфейсов

Для ручного задания реализации интерфейса, используется метод `SetParameter<TParameter>(TParameter parameter)`:

```csharp
var handler = new ServiceMock<AddNoteCommandHanler>(options => {
    options.SetParameter<ILogger>(new Logger());    
})
```

## Ограничения

В случаях если параметры-классы имеют несколько конструкторов или в конструкторе используется параметры типа значения,
будет возникать исключение `ArgumentException`.
Такие параметры требуется задавать явно:

## Использование классов с несколькими конструкторами / параметрами типа значения

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

В случае, когда `NotesHelper` зависит от `INotesRepository`, объект реализующий данный интерфейс можно получить
следующим образом:

```csharp
var handler = new ServiceMock<AddNoteCommandHanler>(options => {
    options.SetParameter(getParameter => {
        var notesRepository = getParameter<INotesRepository>();
        return new NotesHelper(notesRepository);
    }
})
```