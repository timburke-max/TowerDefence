using System.Numerics;
using Raylib_cs;
using TowerDefence.Interfaces;

namespace TowerDefence.Model;

public abstract class Vijand : IVijand {
    // MaximumLevensPunten slaat het maximale leven op (voor de health bar)
    public int MaximumLevensPunten { get; protected set; }

    // Backing field: de echte opslag van LevensPunten
    private int _levensPunten;

    // De setter gebruikt Math.Clamp zodat LP nooit onder 0 of boven het maximum gaat
    public int LevensPunten {
        get => _levensPunten;
        protected set => _levensPunten = Math.Clamp(value, 0, MaximumLevensPunten);
    }

    public Vector2 Position { get; protected set; }

    public bool IsAlive {
        get {
            return LevensPunten > 0;
        }
    }

    protected float Snelheid;
    protected Vector2 Target;
    protected Color EnemyColor;
    protected float Grootte;

    public Vijand(Vector2 startPosition, Vector2 targetPosition) {
        Position = startPosition;
        Target = targetPosition;
    }

    public void Update(float deltaTijd) {
        // Bereken de afstand tussen de vijand en het doel
        float afstand = Vector2.Distance(Position, Target);

        // Als de vijand dicht genoeg bij het doel is, stop dan (anders beweegt hij er voorbij)
        if (afstand < 1.0f) {
            Position = Target;
            return;
        }

        // Bereken de richting: een genormaliseerde vector van lengte 1 die naar het doel wijst
        Vector2 richting = Vector2.Normalize(Target - Position);

        // Beweeg de vijand: richting * snelheid * tijd = aantal pixels deze frame
        Position += richting * Snelheid * deltaTijd;
    }

    public void TakeDamage(int amount) {
        LevensPunten -= amount;
    }

    public virtual void Draw() {
        if (IsAlive) {
            Raylib.DrawCircleV(Position, Grootte, EnemyColor);

            // Health bar: breedte schaalt mee met het percentage LP dat nog over is
            int breedte = (int)(30.0f * LevensPunten / MaximumLevensPunten);
            Raylib.DrawRectangle((int)Position.X - 15, (int)Position.Y - (int)Grootte - 10, breedte, 4, Color.Green);
        }
    }
}
