using World.Generators.Segments;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mechanics.Grid;

namespace World.Generators
{
    public class WorldGenerator : MonoBehaviour, IWorldGenerator
    {
        [SerializeField]
        private Transform startCenterTransform;
        [SerializeField]
        private Transform playerTransform;
        [SerializeField]
        [Range(0,100)]
        private float minDistancePlayerToWorldBounds = 5;

        [Header("Generator settings")]
        [SerializeField]
        [Range(3, 50)]
        private int countHeigthSegments = 3;
        [SerializeField]
        [Range(3, 50)]
        private int countWidthSegments = 3;
        [SerializeField]
        private WorldSegment segmentPrefab;
        [SerializeField]
        private Transform parentSegmentsTransform;
        [SerializeField]
        private WorldSegmentsGrid gridSegments;


        #region Awake/Start/Update
        private void Start()
        {
            GenerateWorld();
            gridSegments.OnFlippedCells += GridSegments_OnFlippedCells;
        }

        private void FixedUpdate()
        {
            CheckPlayerDistanceToWorldBounds();
        }
        #endregion

      
        private void CheckPlayerDistanceToWorldBounds()
        {
            if (playerTransform.position.z + minDistancePlayerToWorldBounds > GetWorldVerticalBounds().y)
            {
                gridSegments.FlipBorders(GridBorderType.Top);
            }
            else if (playerTransform.position.z - minDistancePlayerToWorldBounds < GetWorldVerticalBounds().x)
            {
                gridSegments.FlipBorders(GridBorderType.Bottom);
            }

            if (playerTransform.position.x + minDistancePlayerToWorldBounds > GetWorldHorizontalBounds().y)
            {
                gridSegments.FlipBorders(GridBorderType.Right);
            }
            else if (playerTransform.position.x - minDistancePlayerToWorldBounds < GetWorldHorizontalBounds().x)
            {
                gridSegments.FlipBorders(GridBorderType.Left);
            }

        }
        public void GenerateWorld()
        {
            if (gridSegments == null)
            {
                gridSegments = new WorldSegmentsGrid(countHeigthSegments, countWidthSegments, segmentPrefab, startCenterTransform, parentSegmentsTransform);
            }
            gridSegments.GenerateGrid();

            List<WorldSegment> segments = gridSegments.GetListGridElements();
        }


        #region Get
        private Vector2 GetWorldHorizontalBounds()
        {
            return gridSegments.GetHorizontalBounds();
        }
        private Vector2 GetWorldVerticalBounds()
        {
            return gridSegments.GetVerticalBounds();
        }
        #endregion

        #region Events
        private void GridSegments_OnFlippedCells(object sender, List<WorldSegment> e)
        {
           
        }
        #endregion

        #region OnDestroy
        private void OnDestroy()
        {
            gridSegments.OnFlippedCells -= GridSegments_OnFlippedCells;
        }
        #endregion
    }
}
