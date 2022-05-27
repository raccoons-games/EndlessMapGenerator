using Mechanics.Grid.Cells;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace World.Generators.Grids
{
    public class WorldCell : BaseCell
    {
        [Header("World cell settings")]
        [SerializeField]
        private MeshRenderer _meshRenderer;
        [SerializeField]
        private BoxCollider _collider;

        #region Init
        public void Init(Vector2Int index)
        {
            SetIndex(index);
        }
        public void Init(Vector2Int index, Material material)
        {
            Init(index);
            SetMaterial(material);
        }
        #endregion

        #region Set
        public void SetMaterial(Material material)
        {
            _meshRenderer.material = material;
        }
        #endregion
        #region Get

        public Vector2 GetSize()
        {
            Vector2 result = new Vector2();
            result.x = _collider.size.x * transform.lossyScale.x;
            result.y = _collider.size.y * transform.lossyScale.z;
            return result; ;
        }
        #endregion
    }
}
