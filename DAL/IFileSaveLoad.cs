namespace DAL;
using Components;
public interface IFileSaveLoad
{
    void SaveConfiguration(CustomConfig customConfig, string configName);
    
    CustomConfig LoadConfiguration(string configName);

    List<CustomConfig> DisplayAllConfigurations();

    string SaveGameState(CustomConfig customConfig, Dictionary<(int, int), char> pieces, Grid grid,
        string saveName, string configName);

    void SaveTempGameState(Dictionary<(int, int), char> positions, Grid grid, string gameSaveName);

    List<string> DisplayAllGames();

    void SaveInitialGame(string saveName, int configId, string firstPlayerPassword, string secondPlayerPassword);
    
    (CustomConfig config, Dictionary<(int, int), char> pieces, Grid grid) LoadGame(string saveName);

    void DeleteAllTempGameStates(string GameName);

}