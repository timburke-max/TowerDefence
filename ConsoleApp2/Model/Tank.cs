using Raylib_cs;
using System.Numerics;

namespace TowerDefence.Model;

internal class Tank : Vijand
{
    public Tank(Vector2 start, Vector2 target) : base(start, target)
    {
        MaximumLevensPunten = 170;
        LevensPunten = 170;
        Snelheid = 150.0f;
        Grootte = 30.0f;
        EnemyColor = Color.Purple;

    }
}
