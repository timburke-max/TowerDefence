using System.Numerics;
using Raylib_cs;
using TowerDefence.Interfaces;

namespace TowerDefence.Model;

public abstract class Vijand : IVijand
{
    // Ensure hitpoints stay within 0..MaximumLevensPunten
    public int LevensPunten { get; protected set; }
    public int MaximumLevensPunten { get; protected set; }
    public Vector2 Position { get; protected set; }
    public bool IsAlive
    {
        get
        {
            return LevensPunten > 0;
        }
    }

    public bool HeeftDoelBereikt { get; protected set; }

    protected float Snelheid;
    protected Vector2 Target;
    protected Color EnemyColor;
    protected float Grootte;

    public Vijand(Vector2 startPosition, Vector2 targetPosition)
    {
        Position = startPosition;
        Target = targetPosition;
        HeeftDoelBereikt = false;
    }

    public void Update(float deltaTime)
    {
        if (HeeftDoelBereikt || !IsAlive)
            return;

        var toTarget = Target - Position;
        var distance = toTarget.Length();

        if (distance <= 0.1f)
        {
            Position = Target;
            HeeftDoelBereikt = true;
            return;
        }

        var direction = Vector2.Zero;
        if (distance > 0)
            direction = Vector2.Normalize(toTarget);

        var move = direction * Snelheid * deltaTime;
        if (move.Length() >= distance)
        {
            Position = Target;
            HeeftDoelBereikt = true;
        }
        else
        {
            Position += move;
        }
    }

    public void TakeDamage(int amount)
    {
        if (!IsAlive)
            return;

        LevensPunten -= amount;
        if (LevensPunten < 0)
            LevensPunten = 0;
    }


    public virtual void Draw()
    {
        if (IsAlive)
        {
            Raylib.DrawCircleV(Position, Grootte, EnemyColor);
            // Teken een klein rood levensbalkje boven hun hoofd
            Raylib.DrawRectangle((int)Position.X - 10, (int)Position.Y - (int)Grootte - 10, (int)(Grootte), 4, Color.Green);
        }
    }
}