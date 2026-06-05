using System.Numerics;
using Raylib_cs;

namespace TowerDefence.Model;

class BasicVijand2 : Vijand
{
    public BasicVijand2(Vector2 start, Vector2 target) : base(start, target)
    {
        LevensPunten = 150;
        Snelheid = 100.0f;
        Grootte = 20.0f;
        EnemyColor = Color.Orange;
    }
}
