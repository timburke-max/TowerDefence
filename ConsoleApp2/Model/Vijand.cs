using System.Numerics;
using Raylib_cs;
using TowerDefence.Interfaces;

namespace TowerDefence.Model;

public abstract class Vijand : IVijand
{
   
    public int LevensPunten { get; protected set; }
    public Vector2 Position { get; protected set; }
    public int MaximumLevensPunten { get; protected set; }
    public bool IsAlive
    { 
        get {
            return LevensPunten > 0;

        }
    }

    protected float Snelheid;
    protected Vector2 Target;
    protected Color EnemyColor;
    protected float Grootte;

    public Vijand(Vector2 startPosition, Vector2 targetPosition)
    {
        Position = startPosition;
        Target = targetPosition;
    }

    public void Update(float deltaTime)
    {
        if (!IsAlive)
            return;

        Vector2 direction = Target - Position;

        float distance = direction.Length();

        if (distance < 1f)
            return;

        direction = Vector2.Normalize(direction);

        Position += direction * Snelheid * deltaTime;
    }

    public void TakeDamage(int amount)
    {
        LevensPunten -= amount;

        if (LevensPunten < 0)
            LevensPunten = 0;
        LevensPunten = Math.Clamp(LevensPunten, 0, MaximumLevensPunten);
    }

    
    public virtual void Draw()
    {
        if (IsAlive)
        {
            Raylib.DrawCircleV(Position, Grootte, EnemyColor);
            // Teken een klein rood levensbalkje boven hun hoofd
            Raylib.DrawRectangle((int)Position.X - 15, (int)Position.Y - (int)Grootte - 10, (int)(LevensPunten / 3.33f), 4, Color.Green);
        }
    }
}