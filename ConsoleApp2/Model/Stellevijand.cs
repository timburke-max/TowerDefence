using System.Numerics;
using Raylib_cs;

namespace TowerDefence.Model;

public class StelleVijand : Vijand {
    public StelleVijand(List<Vector2> padPunten, Difficulty moeilijkheid) : base(padPunten) {
        float hpMultiplier = moeilijkheid switch {
            Difficulty.Easy => 0.7f,
            Difficulty.Medium => 1.0f,
            Difficulty.Hard => 1.5f,
            _ => 1.0f
        };
        float snelheidMultiplier = moeilijkheid switch {
            Difficulty.Easy => 0.8f,
            Difficulty.Medium => 1.0f,
            Difficulty.Hard => 1.3f,
            _ => 1.0f
        };

        MaximumLevensPunten = (int)(80 * hpMultiplier);
        LevensPunten = MaximumLevensPunten;
        Snelheid = 90.0f * snelheidMultiplier;
        Grootte = 18.0f;
        EnemyColor = Color.Red;
        Beloning = 10;  // snel maar minder waard: 10g
    }
}
