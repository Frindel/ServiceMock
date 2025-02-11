namespace NotesService.Tests.Logic;

public class AddNoteCommandHandler
{
    private readonly INotesRepository _notes;

    public AddNoteCommandHandler(INotesRepository notes)
    {
        _notes = notes;
    }

    public async Task<int> AddNoteAsync(Note note)
    {
        return await _notes.AddAsync(note);
    }
}