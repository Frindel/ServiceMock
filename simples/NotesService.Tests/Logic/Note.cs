namespace NotesService.Tests.Logic;

public class Note
{
    public string Header { get; }

    public string Description { get; }

    public Note()
    {
        Header = Description = string.Empty;
    }

    public Note(string header, string description)
    {
        Header = header;
        Description = description;
    }
}