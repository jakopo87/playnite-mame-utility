using MAMEUtility.Models;
using MAMEUtility.Services.Engine.Platforms;
using Playnite.SDK;
using Playnite.SDK.Models;
using System.Collections.Generic;
using System.Linq;

namespace MAMEUtility.Services.Engine
{
	class GameMetadataManager
	{
		////////////////////////////////////////////////////
		public enum MetaDataType { Year }

		////////////////////////////////////////////////////
		public static void setMetaDataOfSelectedGames(MetaDataType mediaType)
		{
			// Get machines
			Dictionary<string, RomsetMachine> machines = MachinesService.getMachines();
			if (machines == null)
			{
				UI.UIService.showError("No machine found", "Cannot get Machines. Please check plugin settings.");
				return;
			}

			int yearsApplied = 0;
			int selectedGamesCount = 0;
			GlobalProgressResult progressResult = UI.UIService.showProgress("Applying metadata years to selection", false, true, (progressAction) =>
			{
				// Get selected games
				IEnumerable<Game> selectedGames = MAMEUtilityPlugin.playniteAPI.MainView.SelectedGames;
				selectedGamesCount = selectedGames.Count();

				// Apply images for each game
				foreach (Game game in selectedGames)
				{
					var mameMachine = MachinesService.findMachineByPlayniteGame(machines, game);
					if (mameMachine == null || !int.TryParse(mameMachine.year, out int year))
					{
						continue;
					}
					game.ReleaseDate = new ReleaseDate(year);
					yearsApplied++;
				}
			});

			if (selectedGamesCount == 0)
			{
				UI.UIService.showMessage("No games selected. Please select games.");
				return;
			}

			switch (mediaType)
			{
				case MetaDataType.Year:
					UI.UIService.showMessage(yearsApplied + " years were set");
					break;
			}

		}

	}
}
