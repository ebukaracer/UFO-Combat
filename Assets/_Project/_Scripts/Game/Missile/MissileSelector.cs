using Racer.Utilities;
using System;
using System.Linq;
using UnityEngine;


internal enum MissileType { Default, Missile01, Fireball01, Fireball02 }

[Serializable]
internal class MissileProperty
{
     public MissileType missileType;

     public Sprite sprite;

     public int dmgPerHit;
}

internal class MissileSelector : SingletonPattern.StaticInstance<MissileSelector>
{
    [SerializeField] private MissileProperty[] missileProperties;

    public MissileProperty GetMissile(MissileType missileType) => missileProperties.Single(mt => mt.missileType == missileType);
    public MissileProperty GetMissile(int index) => missileProperties[index];

    public int Length => missileProperties.Length;
}
