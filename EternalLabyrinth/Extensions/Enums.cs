using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EternalLabyrinth
{
    public enum CollidingSide
    {
        TOP, RIGHT, BOTTOM, LEFT, NOCOLLISION
    }

    public enum T
    {
        Player, RockWall, IceGlazier, IceHold, FireBall, ElectroCharge, LightningEffect,
        Skeltri, SkeltriShard, SkeltriStomp,
        Flame, FlamingBullet,
        Waterne,
        Smauken, SmokeScreen,
        Skull,
        Bouldren,
        TestEnemy
    }
}
