using Raylib_cs;
using System.Numerics;
using TowerDefence.Interfaces;

namespace TowerDefence.Model;

public class BasicToren2 : IToren
{
    private float _cooldownTimer = 0.0f;
    private IVijand _currentTarget;

    public int Schade { get; }
    public float Afstand { get; }
    public float VuurRatio { get; }
    public Vector2 Positie { get; }

    public IVijand CurrentTarget => _currentTarget;
    public IVijand Target => _currentTarget;

    public BasicToren2(Vector2 position)
    {
        Positie = position;

        
        Schade = 50;         
        Afstand = 150.0f; 
        VuurRatio = 0.6f;    
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
            float distance = Vector2.Distance(Positie, _currentTarget.Position);
            if (distance <= Afstand)
                return;
        }

        _currentTarget = null;

        float closestDistance = float.MaxValue;

        foreach (var enemy in enemies)
        {
            if (!enemy.IsAlive) continue;

            float distance = Vector2.Distance(Positie, enemy.Position);

            if (distance <= Afstand && distance < closestDistance)
            {
                closestDistance = distance;
                _currentTarget = enemy;
            }
        }
    }

    public void ValAan(float deltaTime)
    {
        if (_currentTarget == null || !_currentTarget.IsAlive)
            return;

        _cooldownTimer += deltaTime;

        if (_cooldownTimer >= VuurRatio)
        {
            _currentTarget.TakeDamage(Schade);
            _cooldownTimer = 0f;
        }
    }

    public void Draw()
    {
        Raylib.DrawRectangle((int)Positie.X - 20, (int)Positie.Y - 20, 40, 40, Color.Red);

        Raylib.DrawCircleLines((int)Positie.X, (int)Positie.Y, Afstand, Color.LightGray);

        if (_currentTarget != null && _currentTarget.IsAlive)
        {
            Raylib.DrawLineV(Positie, _currentTarget.Position, Color.Orange);
        }
    }
}