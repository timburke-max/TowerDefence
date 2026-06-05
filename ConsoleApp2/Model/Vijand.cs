using System.Numerics;
using Raylib_cs;
using TowerDefence.Interfaces;

namespace TowerDefence.Model;

public abstract class Vijand : IVijand {
    public int MaximumLevensPunten { get; protected set; }

    private int _levensPunten;
    public int LevensPunten {
        get => _levensPunten;
        protected set => _levensPunten = Math.Clamp(value, 0, MaximumLevensPunten);
    }

    public Vector2 Position { get; protected set; }
    public bool IsAlive => LevensPunten > 0;
    public bool HeeftDoelBereikt => _huidigWaypoint >= PadPunten.Count;
    public int Beloning { get; protected set; }  // geld dat de speler krijgt bij kill

    protected List<Vector2> PadPunten;
    private int _huidigWaypoint = 1;

    protected float Snelheid;
    protected Color EnemyColor;
    protected float Grootte;

    public Vijand(List<Vector2> padPunten) {
        PadPunten = padPunten;
        Position = padPunten[0];
    }

    public void Update(float deltaTime) {
        if (HeeftDoelBereikt) return;

        Vector2 huidigDoel = PadPunten[_huidigWaypoint];
        float afstand = Vector2.Distance(Position, huidigDoel);

        if (afstand < 2.0f) {
            Position = huidigDoel;
            _huidigWaypoint++;
        }
        else {
            Vector2 richting = Vector2.Normalize(huidigDoel - Position);
            Position += richting * Snelheid * deltaTime;
        }
    }

    public void TakeDamage(int amount) {
        LevensPunten -= amount;
    }

    public virtual void Draw() {
        if (IsAlive) {
            Raylib.DrawCircleV(Position, Grootte, EnemyColor);
            int breedte = (int)(30.0f * LevensPunten / MaximumLevensPunten);
            Raylib.DrawRectangle(
                (int)Position.X - 15,
                (int)Position.Y - (int)Grootte - 10,
                breedte, 4, Color.Green);
        }
    }
}
