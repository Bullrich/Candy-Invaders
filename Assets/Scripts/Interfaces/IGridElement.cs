﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//By @JavierBullrich

namespace Game.Grid {
    public interface IGridElement
    {
        bool isActive();
        Vector2 getPosition();
        GameObject getGameobject();
        void ExecuteMovement();
        void SetGrid(GridSystem gr);
        int getColorType();
        void ChainDestroy();
    }
}