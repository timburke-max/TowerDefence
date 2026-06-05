using System.Numerics;
using Raylib_cs;
using System.Collections.Generic;

namespace TowerDefence.Model;

public class BasicVijand : Vijand
{

    public BasicVijand(List<Vector2> route) : base(route)
    {
        MaximumLevensPunten = 200;
        LevensPunten = 200;
        Snelheid = 40.0f;
        Grootte = 20.0f;
        EnemyColor = Color.DarkGreen;
    }
}
