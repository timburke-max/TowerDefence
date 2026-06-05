using Raylib_cs;
using System.Numerics;
using TowerDefence.Interfaces;

namespace TowerDefence.Model;

public class Sniper : IToren
{
    private float _cooldownTimer = 0.0f;

    public int Schade { get; }
    public float Afstand { get; }
    public float VuurRatio { get; }
    public Vector2 Positie { get; }

    private List<IVijand> _targetsInRange = new();

    public Sniper(Vector2 position)
    {
        Positie = position;
        Schade = 100;
        Afstand = 250.0f;
        VuurRatio = 2.0f;
    }

    public void Update(List<IVijand> enemies, float deltaTime)
    {
        // Val aan volgens cooldown
        ValAan(deltaTime);

        _targetsInRange = enemies
        .Where(e => e.IsAlive &&
                Vector2.Distance(Positie, e.Position) <= Afstand)
        .OrderBy(e => Vector2.Distance(Positie, e.Position))
        .Take(3)
        .ToList();

    }

    public void ValAan(float deltaTime)
    {
        _cooldownTimer += deltaTime;

        if (_cooldownTimer >= VuurRatio)
        {
            foreach (var enemy in _targetsInRange)
            {
                enemy.TakeDamage(Schade);
            }

            _cooldownTimer = 0.0f;
        }
    }

    public void Draw()
    {
        // Toren
        Raylib.DrawRectangle((int)Positie.X - 20, (int)Positie.Y - 20, 40, 40, Color.Red);

        // Bereik
        Raylib.DrawCircleLines((int)Positie.X, (int)Positie.Y, Afstand, Color.LightGray);

        // Lasers naar alle vijanden binnen bereik
        foreach (var enemy in _targetsInRange)
        {
            Raylib.DrawLineV(Positie, enemy.Position, Color.Red);
        }
    }
}
