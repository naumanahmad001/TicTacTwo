using Components;
using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static WebApp.Pages.TheGame.PlayModel;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : Controller
    {
        [HttpPost("SaveTempGameState")]
        public IActionResult SaveTempGameState([FromBody] SaveTempGameRequest request)
        {
            try
            {
                // Parse top-left grid coordinates
                var gridTopLeftParts = request.Grid.TopLeft.Split(',');
                var grid = new Grid
                {
                    TopLeft = (int.Parse(gridTopLeftParts[0]), int.Parse(gridTopLeftParts[1]))
                };

                // Convert positions to Dictionary<(int, int), char>
                var positions = request.Positions.ToDictionary(
                    kvp =>
                    {
                        var coords = kvp.Key.Split(',');
                        return (int.Parse(coords[0]), int.Parse(coords[1]));
                    },
                    kvp => kvp.Value[0] // Convert string to char
                );

                IFileSaveLoad fileSaveLoad = FileSaveLoadFactory.GetFileSaveLoadImplementation();
                fileSaveLoad.DeleteAllTempGameStates(request.GameId);
                fileSaveLoad.SaveTempGameState(positions, grid, request.GameId);
                
                //string saveResult = fileSaveLoad.SaveGameState(new CustomConfig(), positions, grid, request.GameSaveName, string.Empty);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("SaveGameState")]
        public IActionResult SaveGameState([FromBody] SaveTempGameRequest request)
        {
            try
            {
                // Parse top-left grid coordinates
                var gridTopLeftParts = request.Grid.TopLeft.Split(',');
                var grid = new Grid
                {
                    TopLeft = (int.Parse(gridTopLeftParts[0]), int.Parse(gridTopLeftParts[1]))
                };

                // Convert positions to Dictionary<(int, int), char>
                var positions = request.Positions.ToDictionary(
                    kvp =>
                    {
                        var coords = kvp.Key.Split(',');
                        return (int.Parse(coords[0]), int.Parse(coords[1]));
                    },
                    kvp => kvp.Value[0] // Convert string to char
                );

                IFileSaveLoad fileSaveLoad = FileSaveLoadFactory.GetFileSaveLoadImplementation();
                if (request.SkipDeleteTempStates != null && request.SkipDeleteTempStates == true) { }
                else
                {
                    fileSaveLoad.DeleteAllTempGameStates(request.GameId);
                }
                string saveResult = fileSaveLoad.SaveGameState(new CustomConfig(), positions, grid, request.GameId, string.Empty);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
