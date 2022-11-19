using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layer
{
    public const int Default = 0;
    public const int Transparent = 1;
    public const int IgnoreRaycast = 2;

    public const int Water = 4;
    public const int UI = 5;
    public const int Zombie = 6;

    public const int Bullet = 8;
    public const int Explosion = 9;
}

public class Tag
{
    public const string Tank = "Tank";
    public const string DefenseWall = "DefenseWall";
    public const string Monster = "Monster";
    public const string ZombieTarget = "ZombieTarget";
}

public class Scene
{

}