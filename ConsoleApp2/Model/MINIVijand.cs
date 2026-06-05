using System;
using System.Collections.Generic;
using System.Numerics;
using Raylib_cs;
using System.Text;

namespace TowerDefence.Model;

public class MINIVijand : Vijand
{
    public MINIVijand(Vector2 start, Vector2 target) : base(start, target)
    {
        LevensPunten = 100;
        Snelheid = 60.0f;
        Grootte = 15.0f;
        EnemyColor = Color.Green;
    }
}
