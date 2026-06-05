using System.Numerics;
using Raylib_cs;

namespace TowerDefence.Model;


public class BasicVijand : Vijand
{
    public BasicVijand(Vector2 start, Vector2 target) : base(start, target)
    {
        
        MaximumLevensPunten = 200;
        LevensPunten = 200;
        Snelheid = 40.0f;
        Grootte = 20.0f;
        EnemyColor = Color.DarkGreen;
    }
}
public class SnelleVijand : Vijand
{   public SnelleVijand(Vector2 start, Vector2 target) : base(start, target)
    {
        MaximumLevensPunten = 80;
        LevensPunten = 80;
        Snelheid = 50f;
        Grootte = 12.0f;
        EnemyColor = Color.Red;
    }


}
