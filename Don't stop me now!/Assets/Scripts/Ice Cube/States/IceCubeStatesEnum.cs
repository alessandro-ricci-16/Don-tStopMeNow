using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace Ice_Cube.States
{
    public enum IceCubeStatesEnum
    {
        OnGround,
        OnAir,
        IsJumping,
        IsAccelerating,
        IsWallJumping,
        IsDashing,
        IsGroundPounding
    }
}