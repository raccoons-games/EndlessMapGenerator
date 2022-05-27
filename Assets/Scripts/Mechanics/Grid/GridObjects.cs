using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mechanics.Grid
{
    public abstract class GridObjects<T> where T : MonoBehaviour
    {
        protected T[,] _grid;
        protected int _height;
        protected int _width;
        protected string _cellName;

        protected T _prefab;
        protected Transform _centerTransforn;
        protected Transform _parentSpawnTransform;

        public event System.EventHandler<List<T>> OnFlippedCells;

        protected void Init(int height, int width, T prefab, Transform centerTransforn, Transform parentSpawnTransform, string cellName)
        {
            _height = height;
            _width = width;
            _prefab = prefab;
            _centerTransforn = centerTransforn;
            _parentSpawnTransform = parentSpawnTransform;
            _cellName = cellName;
        }
        public virtual void GenerateGrid()
        {
            if (_grid != null)
            {
                ClearGrid();
            }
            _grid = new T[_height, _width];

            Vector3 startSpawnPosition = _centerTransforn.position;
            startSpawnPosition.x -= ((float)(_width - 1) / 2) * (GetCellSize().x);
            startSpawnPosition.z -= ((float)(_height - 1) / 2) * (GetCellSize().y);

            for (int i = 0; i < _height; i++)
            {
                Vector3 currentSpawnPosition = startSpawnPosition;
                currentSpawnPosition.z += GetCellSize().y * i;

                for (int j = 0; j < _width; j++)
                {
                    T cell = CreateCell(new Vector2Int(i, j), _parentSpawnTransform);

                    cell.transform.position = currentSpawnPosition;
                    cell.name = $"{_cellName}_{i}_{j}";
                    _grid[i, j] = cell;

                    currentSpawnPosition.x += GetCellSize().x;
                }
            }
        }
        private T CreateCell(Vector2Int index, Transform parent)
        {
            T result =  Object.Instantiate<T>(_prefab, parent);
            return result;
        }
        private void ClearGrid()
        {
            if (_grid != null)
            {
                if (_grid.GetLength(0) > 0)
                {
                    for (int i = 0; i < _grid.GetLength(0); i++)
                    {
                        if (_grid.GetLength(1) > 1)
                        {
                            for (int j = 0; j < _grid.GetLength(1); j++)
                            {
                                Object.Destroy(_grid[i, j].gameObject);
                            }
                        }
                    }
                }
                _grid = null;
            }
        }

        #region Flip borders
        public void FlipBorders(GridBorderType borderType)
        {
            T[,] tempGrid = GetTempGrid();
            List<T> borderCells = new List<T>();

            bool isFlipHorizontal = false ;

            Vector3 targetPosition = new Vector3();

            switch (borderType)
            {
                case GridBorderType.Left:
                    targetPosition.x = GetHorizontalBounds().x - (GetCellSize().x / 2);
                    borderCells.AddRange(GetRightBordersCells());
                    isFlipHorizontal = true;
                    break;
                case GridBorderType.Right:
                    targetPosition.x = GetHorizontalBounds().y + (GetCellSize().x / 2);
                    borderCells.AddRange(GetLeftBordersCells());
                    isFlipHorizontal = true;
                    break;
                case GridBorderType.Top:
                    targetPosition.z = GetVerticalBounds().y + (GetCellSize().y / 2);
                    borderCells.AddRange(GetBottomBordersCells());
                    isFlipHorizontal = false;
                    break;
                case GridBorderType.Bottom:
                    targetPosition.z = GetVerticalBounds().x - (GetCellSize().y / 2);
                    borderCells.AddRange(GetTopBordersCells());
                    isFlipHorizontal = false;
                    break;
            }

            for (int i = 0; i < borderCells.Count; i++)
            {
                if (isFlipHorizontal == true)
                {
                    targetPosition.z = borderCells[i].transform.position.z;
                }
                else
                {
                    targetPosition.x = borderCells[i].transform.position.x;
                }
                targetPosition.y = borderCells[i].transform.position.y;

                borderCells[i].transform.position = targetPosition;
            }

            _grid = new T[tempGrid.GetLength(0), tempGrid.GetLength(1)];

            for (int i = 0; i < tempGrid.GetLength(0); i++)
            {
                for (int j = 0; j < tempGrid.GetLength(1); j++)
                {
                    if (isFlipHorizontal == true)
                    {
                        if (borderType == GridBorderType.Right)
                        {
                            if (j + 1 < tempGrid.GetLength(1))
                            {
                                _grid[i, j] = tempGrid[i, j + 1];
                            }
                            else
                            {
                                _grid[i, j] = tempGrid[i, tempGrid.GetLength(1) - 1 - j];
                            }
                        }
                        else // Left
                        {
                            if (j == 0)
                            {
                                _grid[i, j] = tempGrid[i, tempGrid.GetLength(1) - 1];
                            }
                            else
                            {
                                _grid[i, j] = tempGrid[i, j - 1];
                            }
                        }
                    }
                    else
                    {
                        if (borderType == GridBorderType.Top)
                        {
                            if (i + 1 < tempGrid.GetLength(0))
                            {
                                _grid[i, j] = tempGrid[i + 1, j];
                            }
                            else
                            {
                                _grid[i, j] = tempGrid[tempGrid.GetLength(0) - 1 - i, j];
                            }
                        }
                        else // Bottom
                        {
                            if (i == 0)
                            {
                                _grid[i, j] = tempGrid[tempGrid.GetLength(0) - 1,j];
                            }
                            else
                            {
                                _grid[i, j] = tempGrid[i -1 , j];
                            }
                        }
                    }
                    _grid[i, j].name = $"{_cellName}_{i}_{j}";
                }
            }
            OnFlippedCells?.Invoke(this,borderCells);
        }
        #endregion

        #region Get

        #region Get grid cells
        public T[,] GetGrid()
        {
            return _grid;
        }
        public List<T> GetListGridElements()
        {
            List<T> result = new List<T>();
            T[,] grid = GetGrid();

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    result.Add(grid[i, j]);
                }
            }
            return result;
        }

        private T[,] GetTempGrid()
        {
            T[,] result = new T[_grid.GetLength(0), _grid.GetLength(1)];
            for (int i = 0; i < _grid.GetLength(0); i++)
            {
                for (int j = 0; j < _grid.GetLength(1); j++)
                {
                    result[i, j] = _grid[i, j];
                }
            }
            return result;
        }
        private T[] GetLeftBordersCells()
        {
            T[] result = new T[_grid.GetLength(0)];
            for (int i = 0; i < _grid.GetLength(0); i++)
            {
                result[i] = _grid[i, 0];
            }
            return result;
        }
        private T[] GetRightBordersCells()
        {
            T[] result = new T[_grid.GetLength(0)];
            for (int i = 0; i < _grid.GetLength(0); i++)
            {
                result[i] = _grid[i, _grid.GetLength(1) - 1];
            }
            return result;
        }
        private T[] GetBottomBordersCells()
        {
            T[] result = new T[_grid.GetLength(1)];
            for (int i = 0; i < _grid.GetLength(1); i++)
            {
                result[i] = _grid[0, i];
            }
            return result;
        }
        private T[] GetTopBordersCells()
        {
            T[] result = new T[_grid.GetLength(1)];
            for (int i = 0; i < _grid.GetLength(1); i++)
            {
                result[i] = _grid[_grid.GetLength(0) - 1, i];
            }
            return result;
        }
        #endregion

        #region Get grid parameters
        public abstract Vector2 GetCellSize();
        public Vector2 GetHorizontalBounds()
        {
            Vector2 result = new Vector2();

            float offset = (GetCellSize().x / 2);

            T leftCell = _grid[0, 0];
            T rightCell = _grid[0, _grid.GetLength(1) - 1];

            result.x = leftCell.transform.position.x - offset;
            result.y = rightCell.transform.position.x + offset;
            return result;
        }
        public Vector2 GetVerticalBounds()
        {
            Vector2 result = new Vector2();

            float offset = (GetCellSize().y / 2);

            T bottomCell = _grid[0, 0];
            T topCell = _grid[_grid.GetLength(0) - 1, 0];

            result.x = bottomCell.transform.position.z - offset;
            result.y = topCell.transform.position.z + offset;
            return result;
        }
        public Vector3 GetCenterPosition()
        {
            Vector3 result = new Vector3();
            Vector3 widthBorders = GetHorizontalBounds();
            Vector3 heigthBorders = GetVerticalBounds();
            result.x = (widthBorders.x + widthBorders.y) / 2;
            result.y = 0;
            result.z = (heigthBorders.x + heigthBorders.y) / 2;
            return result;
        }
        public Vector2 GetCenterLeftBorderPosition()
        {
            Vector2 result = GetCenterPosition();
            result.x = GetHorizontalBounds().x;
            return result;
        }
        public Vector2 GetCenterRightBorderPosition()
        {
            Vector2 result = GetCenterPosition();
            result.x = GetHorizontalBounds().y;
            return result;
        }
        public Vector2 GetCenterBottomBorderPosition()
        {
            Vector2 result = GetCenterPosition();
            result.y = GetVerticalBounds().x;
            return result;
        }
        public Vector2 GetCenterTopBorderPosition()
        {
            Vector2 result = GetCenterPosition();
            result.y = GetVerticalBounds().y;
            return result;
        }
        #endregion
        #endregion
    }
    public enum GridBorderType
    {
        Left = 0,
        Right = 1,
        Bottom = 2,
        Top = 3
    }
}
