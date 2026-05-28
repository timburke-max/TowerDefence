using System.Numerics;
using Raylib_cs;

namespace TowerDefence.Model;

public class BasicVijand : Vijand
{
    public BasicVijand(Vector2 start, Vector2 target) : base(start, target)
    {
        //TODO: maak andere vijanden met andere statistieken
        LevensPunten = 200;
        Snelheid = 40.0f;
        Grootte = 20.0f;
        EnemyColor = Color.DarkGreen;
    }
}
