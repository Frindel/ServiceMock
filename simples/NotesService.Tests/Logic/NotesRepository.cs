namespace NotesService.Tests.Logic;

public class NotesRepository : INotesRepository
{
    private int _oldNoteNumber;

    public NotesRepository(int firstNoteNumber)
    {
        _oldNoteNumber = firstNoteNumber;
    }

    public Task<int> AddAsync(Note note)
    {
        return Task.FromResult(++_oldNoteNumber);
    }
}