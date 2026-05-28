using System.Numerics;
using Raylib_cs;
using TowerDefence.Interfaces;

namespace TowerDefence.Model;

public abstract class Vijand : IVijand
{
    // TODO: Voeg extra voorwaarden toe, dat levenspunten van een 
    // vijand niet onder nul kunnen en niet boven een maximum
    // TODO: voeg een extra property MaximumLevensPunten toe
    public int LevensPunten { get; protected set; }
    public Vector2 Position { get; protected set; }
    public bool IsAlive
    {
        get {
            //TODO zorgt dat deze getter juist teruggeeft of 
            // de vijand nog leeft of niet.
            return true;
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
        //TODO: Update de vijand
        // Zorg dat hij beweegt naar het doel en stopt op het doel
        // Hint: gebruik de functies van Vector2
    }

    public void TakeDamage(int amount)
    {
        //TODO: zorg dat de vijand schade krijgt
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