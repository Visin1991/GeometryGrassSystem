using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace V.GrassSystem
{
    public class TerrainGrassPainter : MonoBehaviour
    {
        public enum Mode
        {
            Editing,
            View
        }

        public Mode mode = Mode.View;
    }
}