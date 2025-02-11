using ServiceMock;
using Moq;
using NotesService.Tests.Logic;

namespace NotesService.Tests;

public class AddingNoteTests
{
    [Test]
    public async Task ReturningCorrectIdOfAddedNote()
    {
        // Arrange
        var service = new ServiceMock<AddNoteCommandHandler>();
        const int newNoteId = 1;
        service.GetParameterMock<INotesRepository>()
            .Setup(nr => nr.AddAsync(It.IsAny<Note>()))
            .ReturnsAsync(newNoteId);
        var newNote = new Note("Note 1", "Note 1 description");

        // Act
        int addedNoteId = await service.Service.AddNoteAsync(newNote);

        // Assert
        Assert.That(addedNoteId, Is.EqualTo(newNoteId));
    }

    [Test]
    public async Task ReturningCorrectIdOfAddedNoteWithHimSetting()
    {
        // Arrange
        const int firstNoteId = 1;
        var service = new ServiceMock<AddNoteCommandHandler>(options =>
            options.SetParameter<INotesRepository>(new NotesRepository(firstNoteId)));
        var newNote = new Note("Note 1", "Note 1 description");

        // Act
        int addedNoteId = await service.Service.AddNoteAsync(newNote);

        // Assert
        Assert.That(addedNoteId, Is.EqualTo(firstNoteId + 1));
    }
}