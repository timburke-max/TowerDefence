using System.Numerics;
using Raylib_cs;
using System.Collections.Generic;

namespace TowerDefence.Model;

public class AdvancedVijand : Vijand
{
    public AdvancedVijand(List<Vector2> route) : base(route)
    {
        MaximumLevensPunten = 350;
        LevensPunten = 350;
        Snelheid = 60.0f;
        Grootte = 25.0f;
        EnemyColor = Color.Red;
    }
}
