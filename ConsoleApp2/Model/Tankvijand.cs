using System.Numerics;

using Raylib_cs;

namespace TowerDefence.Model;

public class TankVijand : Vijand
{
    public TankVijand(Vector2 start, Vector2 target) : base(start, target)
    {

        LevensPunten = 450;
        Snelheid = 20.0f;
        Grootte = 30.0f;
        EnemyColor = Color.DarkBlue;
    }
}
