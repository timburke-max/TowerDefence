using System.Numerics;
using Raylib_cs;
using System.Collections.Generic;

namespace TowerDefence.Model;

public class BossVijand : Vijand
{
    public BossVijand(List<Vector2> route) : base(route)
    {
        MaximumLevensPunten = 800;
        LevensPunten = 800;
        Snelheid = 25.0f;
        Grootte = 35.0f;
        EnemyColor = Color.DarkPurple;
    }
}
