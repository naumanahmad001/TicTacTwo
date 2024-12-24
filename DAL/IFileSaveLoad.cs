namespace DAL;
using Components;
public interface IFileSaveLoad
{
    void SaveConfiguration(CustomConfig customConfig, string configName);
    
    CustomConfig LoadConfiguration(string configName);

    List<CustomConfig> DisplayAllConfigurations();

    string SaveGameState(CustomConfig customConfig, Dictionary<(int, int), char> pieces, Grid grid,
        int gameId, string configName);

    void SaveTempGameState(Dictionary<(int, int), char> positions, Grid grid, int gameId);

    List<string> DisplayAllGames();

    int SaveInitialGame(string saveName, int configId, string firstPlayerPassword, string secondPlayerPassword);
    
    (CustomConfig config, Dictionary<(int, int), char> pieces, Grid grid) LoadGame(int saveId);

    void DeleteAllTempGameStates(int GameId);

}