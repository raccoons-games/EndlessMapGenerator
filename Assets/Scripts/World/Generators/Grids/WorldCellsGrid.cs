using Mechanics.Grid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace World.Generators.Grids
{
    public class WorldCellsGrid : GridObjects<WorldCell>
    {
        public WorldCellsGrid(int height, int width, WorldCell prefab, Transform centerTransforn, Transform parentSpawnTransform)
        {
            base.Init(height, width, prefab, centerTransforn, parentSpawnTransform,"Cell");
        }
        public override Vector2 GetCellSize()
        {
            return _prefab.GetSize();
        }
    }
}
