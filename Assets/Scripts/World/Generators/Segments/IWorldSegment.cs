using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace World.Generators.Segments
{
    public interface IWorldSegment
    {
        void Init(int height, int width);
        void GenerateGrid();
        Vector2 GetSegmentSize();
    }
}
