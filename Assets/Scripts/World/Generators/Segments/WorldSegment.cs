using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using World.Generators.Grids;

namespace World.Generators.Segments
{
    public class WorldSegment : MonoBehaviour, IWorldSegment
    {
        [SerializeField]
        [Range(0, 100)]
        private int _height = 1;
        [SerializeField]
        [Range(1, 100)]
        private int _width = 1;

        [SerializeField]
        private WorldCell _cellPrefab;
        [SerializeField]
        private Transform _gridParetnTransform;

        private WorldCellsGrid _grid;


        public void Init(int height, int width)
        {
            _height = height;
            _width = width;
        }
        public void GenerateGrid()
        {
            if (_grid == null)
            {
                _grid = new WorldCellsGrid(_height, _width, _cellPrefab, transform, _gridParetnTransform);
            }
            _grid.GenerateGrid();
        }
 
        #region Get 

        public Vector2 GetSegmentSize()
        {
            Vector2 result = new Vector2();
            result.x = _cellPrefab.GetSize().x * _width;
            result.y = _cellPrefab.GetSize().y * _height;
            return result;
        }
        #endregion
    }
}
