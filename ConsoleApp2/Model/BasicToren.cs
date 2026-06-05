using Raylib_cs;
using System.Numerics;
using TowerDefence.Interfaces;

namespace TowerDefence.Model;

public class BasicToren : IToren
{
    private float _cooldownTimer = 0.0f;
    private IVijand _currentTarget;
  public int Schade { get; }
    public float Afstand { get; }
    public float VuurRatio { get; }
    public Vector2 Positie { get; }
    public Vector3 positie { get; }
    public BasicToren (Vector2 position)
    {
        Positie = position;
        Schade = 10;
        Afstand = 150.0f;
        VuurRatio = 0.5f;
        
    }

    
    public BasicToren_1 (Vector2 position)
    {
        Positie = position;
        Schade = 8;
        Afstand = 150.0f;
        VuurRatio = 0.5f;
            if (BasicToren_1 = _currentTarget)
        {
          Snelheid -= 10;
        }
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
    public void ValAan(float deltaTime)
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