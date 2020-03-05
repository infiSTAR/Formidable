using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Formidable.Data;

namespace Formidable.Modules
{

    public class ModuleInformation
    {

        public List<GamePlayer> GamePlayers
        {
            get => this.gamePlayers;
        }

        public List<GameLootItem> GameLootItems
        {
            get => this.gameLootItems;
        }

        private List<GamePlayer> gamePlayers;
        private List<GameLootItem> gameLootItems;

        public ModuleInformation(List<GamePlayer> gamePlayers, List<GameLootItem> gameLootItems)
        {
            if (gamePlayers == null)
                throw new ArgumentNullException(nameof(gamePlayers));

            if (gameLootItems == null)
                throw new ArgumentNullException(nameof(gameLootItems));

            this.gamePlayers = gamePlayers;
            this.gameLootItems = gameLootItems;
        }

    }

}
