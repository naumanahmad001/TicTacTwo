namespace DAL;

public static class FileSaveLoadFactory
{
    public static IFileSaveLoad GetFileSaveLoadImplementation()
    {
        bool useDb = true;

        if (useDb)
        {
            return new FileSaveLoadDb();
        }
        else
        {
            return new FileSaveLoadDb();
        }
    }
}