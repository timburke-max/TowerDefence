using Raylib_cs;
using System.Numerics;
using TowerDefence.Interfaces;

namespace TowerDefence.Model;

public class BasicToren : IToren
{
    private float _cooldownTimer = 0.0f;
    private IVijand _currentTarget;
  public int Schade { get; set; }
    public float Afstand { get; set; }
    public float VuurRatio { get; set; }
    public Vector2 Positie { get; set; }
    
    public BasicToren (Vector2 position)
    {
        Positie = position;
        Schade = 10;
        Afstand = 150.0f;
        VuurRatio = 0.5f;
        
    }

    
   

 public void Update(List<IVijand> enemies, float deltaTime)
    {
       
        ZoekDoelwit(enemies);
        ValAan(deltaTime);
    }
    private void ZoekDoelwit(List<IVijand> enemies)
    {
        if (_currentTarget != null && _currentTarget.IsAlive)
        {
            if (Vector2.Distance(Positie, _currentTarget.Position) <= Afstand)
            {
                return;
            }
        }
        _currentTarget = null;
        foreach (var vijand in enemies)
        {
            if (vijand.IsAlive)
            {
                if (Vector2.Distance(Positie, vijand.Position) <= Afstand)
                {
                    _currentTarget = vijand;
                    break;
                }
            }
        }
    }
    public void Draw()
    {
       
        Raylib.DrawRectangle((int)Positie.X - 20, (int)Positie.Y - 20, 40, 40, Color.Blue);

        
        Raylib.DrawCircleLines((int)Positie.X, (int)Positie.Y, Afstand, Color.LightGray);

        
        if (_currentTarget != null && _currentTarget.IsAlive)
        {
            Raylib.DrawLineV(Positie, _currentTarget.Position, Color.Yellow);
        }
    }
    public virtual void ValAan(float deltaTime)
    {
        
        _cooldownTimer = _cooldownTimer + deltaTime;

        if (_currentTarget != null && _currentTarget.IsAlive)
        {
            if (_cooldownTimer >= VuurRatio)
            {
                _currentTarget.TakeDamage(Schade);

                _cooldownTimer = 0.0f;
            }
        }
    }
}