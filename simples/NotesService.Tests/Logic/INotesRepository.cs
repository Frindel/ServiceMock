namespace NotesService.Tests.Logic;

public interface INotesRepository
{
    Task<int> AddAsync(Note note);
}