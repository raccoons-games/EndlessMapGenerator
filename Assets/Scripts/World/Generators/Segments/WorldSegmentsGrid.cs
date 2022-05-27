using Mechanics.Grid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace World.Generators.Segments
{
    public class WorldSegmentsGrid : GridObjects<WorldSegment>
    {
        public WorldSegmentsGrid(int height, int width, WorldSegment prefab, Transform centerTransforn, Transform parentSpawnTransform)
        {
            base.Init(height, width, prefab, centerTransforn, parentSpawnTransform, "Segment");
        }
        public override void GenerateGrid()
        {
            base.GenerateGrid();
            for (int i = 0; i < _grid.GetLength(0); i++)
            {
                for (int j = 0; j < _grid.GetLength(1); j++)
                {
                    _grid[i, j].GenerateGrid();
                }
            }
        }
        public override Vector2 GetCellSize()
        {
            return _prefab.GetSegmentSize();
        }
    }
}
