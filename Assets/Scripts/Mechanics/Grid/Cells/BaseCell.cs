using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mechanics.Grid.Cells
{
    public abstract class BaseCell : MonoBehaviour
    {
        [Header("ReadOnly")]
        [SerializeField]
        private Vector2Int _index;

        #region Set
        public void SetIndex(Vector2Int index)
        {
            this._index = index;
        }
        #endregion
        #region Get
        public Vector2Int GetIndex()
        {
            return _index;
        }
        #endregion

    }
}
