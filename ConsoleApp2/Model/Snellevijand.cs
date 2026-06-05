using System.Numerics;
using Raylib_cs;

namespace TowerDefence.Model;

// Een snelle maar zwakkere vijand — erft alles van Vijand, maar met andere statistieken
public class Snellevijand : Vijand {
    public Snellevijand(Vector2 start, Vector2 target) : base(start, target) {
        MaximumLevensPunten = 120;   // Minder leven dan BasicVijand
        LevensPunten = 120;
        Snelheid = 90.0f;           // sneller dan BasicVijand --> 40.0f
        Grootte = 14.0f;            // kleiner bolletje
        EnemyColor = Color.Pink;    
    }
}