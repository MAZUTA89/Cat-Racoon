using System.Collections.Generic;
using System.Linq;


namespace Assets.Code.Scripts.Gameplay.PlantingTerritory
{
    public class TerritoryService
    {
        List<PlantTerritory> _plantTerritories;

        public TerritoryService()
        {
            _plantTerritories = new List<PlantTerritory>();
        }

        public void AddTerritory(PlantTerritory plantTerritory)
        {
            _plantTerritories.Add(plantTerritory);
        }

        public PlantTerritory GetTerritiryByObjectName(string name)
        {
            return _plantTerritories.FirstOrDefault(terr => terr.gameObject.name == name);
        }
    }
}
