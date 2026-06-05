using System.Numerics;
using Raylib_cs;
using TowerDefence.Interfaces;
using System.Collections.Generic;

namespace TowerDefence.Model;

public abstract class Vijand : IVijand
{
    private int levensPunten;
    public int MaximumLevensPunten { get; protected set; }

    public int LevensPunten
    {
        get { return levensPunten; }
        protected set
        {
            levensPunten = Math.Clamp(value, 0, MaximumLevensPunten);
        }
    }

    public Vector2 Position { get; protected set; }
    public bool IsAlive
    {
        get
        {
            return LevensPunten > 0;
        }
    }

    protected float Snelheid;
    protected Vector2 Target;
    protected Color EnemyColor;
    protected float Grootte;

    protected List<Vector2> Waypoints;
    protected int currentWaypointIndex;
    protected Vector2 StartPosition;

    public Vijand(Vector2 startPosition, Vector2 targetPosition) : this(new List<Vector2> { startPosition, targetPosition })
    {
    }

    public Vijand(List<Vector2> waypoints)
    {
        Waypoints = waypoints;
        StartPosition = waypoints[0];
        Position = StartPosition;
        Target = waypoints[waypoints.Count - 1];
        currentWaypointIndex = 1;
    }

    public void Update(float deltaTime)
    {
        if (!IsAlive) return;
        if (Waypoints == null || Waypoints.Count == 0) return;
        if (currentWaypointIndex >= Waypoints.Count) return;

        Vector2 currentTarget = Waypoints[currentWaypointIndex];
        float afstandTotDoel = Vector2.Distance(Position, currentTarget);
        if (afstandTotDoel < 5f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= Waypoints.Count)
            {
                Position = currentTarget;
                return;
            }
            currentTarget = Waypoints[currentWaypointIndex];
        }

        Vector2 richting = Vector2.Normalize(currentTarget - Position);
        Position += richting * Snelheid * deltaTime;
    }

    public void TakeDamage(int amount)
    {
        if (LevensPunten > 0)
        {
            LevensPunten -= amount;
        }
        else if (LevensPunten - amount < 0)
        {
            LevensPunten = 0;
        }
        else
        {
            LevensPunten = 0;
        }
    }

    public virtual void Draw()
    {
        if (IsAlive)
        {
            Raylib.DrawCircleV(Position, Grootte, EnemyColor);
            float levenPercentage = (float)LevensPunten / MaximumLevensPunten;
            float balkBreedte = 30 * levenPercentage;
            Raylib.DrawRectangle((int)Position.X - 15, (int)Position.Y - (int)Grootte - 10, 30, 4, Color.Red);
            Raylib.DrawRectangle((int)Position.X - 15, (int)Position.Y - (int)Grootte - 10, (int)balkBreedte, 4, Color.Green);
        }
    }

    public Vector2 getPosition()
    {
        return Position;
    }

    public void MultiplyStrength(float factor)
    {
        if (factor <= 0f) return;
        MaximumLevensPunten = (int)Math.Ceiling(MaximumLevensPunten * factor);
        LevensPunten = (int)Math.Ceiling(LevensPunten * factor);
        Snelheid *= factor;
    }
}