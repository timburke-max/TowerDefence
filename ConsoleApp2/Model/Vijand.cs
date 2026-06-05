using System.Numerics;
using Raylib_cs;
using TowerDefence.Interfaces;

namespace TowerDefence.Model;

public abstract class Vijand : IVijand
{
    
    public int MaximumLevensPunten { get; protected set; }


    public int LevensPunten { get; protected set; }
    public Vector2 Position { get; protected set; }
    public bool IsAlive
    {
        get
        {
            
            if (LevensPunten > 0)
           {
              return true;
           }
          else
          {
             return false;
          }
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
     
        float afstand = Vector2.Distance(Position, Target);
        if (afstand > 2.0f)
       {
            Vector2 richting = Vector2.Normalize(Target - Position);
            Position = Position + richting * Snelheid * deltaTime;
      }
       else
        {
            Position = Target;
      }
   }

    public void TakeDamage(int amount)
    {
        
        LevensPunten = LevensPunten - amount;
        if (LevensPunten < 0)
        {
            LevensPunten = 0;
        }
    }
    public virtual void Draw()   
  {
        if (IsAlive)
    {
        Raylib.DrawCircleV(Position, Grootte, EnemyColor);
        
        Raylib.DrawRectangle((int)Position.X - 15, (int)Position.Y - (int)Grootte - 10, (int)(LevensPunten / 3.33f), 4, Color.Green);
    }
  }
}