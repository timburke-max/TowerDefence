using Raylib_cs;
using System.Numerics;
using TowerDefence.Interfaces;

namespace TowerDefence.Model;

public class BasicToren: IToren
{
    private float _cooldownTimer = 0f;
    private IVijand? _currentTarget;

    public int Schade { get; }
    public float Afstand { get; }
    public float VuurRatio { get; }
    public Vector2 Positie { get; }

    public BasicToren(Vector2 position)
    {
        Positie = position;
        Schade = 25;
        Afstand = 150f;
        VuurRatio = 0.5f;
    }

    public void Update(List<IVijand> enemies, float deltaTime)
    {
        ZoekDoelwit(enemies);

        if (_currentTarget is { IsAlive: true })
        {
            ValAan(deltaTime);
        }
    }

    private void ZoekDoelwit(List<IVijand> enemies)
    {
        // Huidig doelwit ongeldig? Reset.
        if (_currentTarget != null)
        {
            bool buitenBereik = Vector2.Distance(Positie, _currentTarget.Position) > Afstand;
            if (!_currentTarget.IsAlive || buitenBereik)
            {
                _currentTarget = null;
            }
        }

        // Nieuw doelwit zoeken
        if (_currentTarget == null)
        {
            _currentTarget = enemies
                .Where(e => e.IsAlive)
                .FirstOrDefault(e => Vector2.Distance(Positie, e.Position) <= Afstand);
        }
    }

    public void ValAan(float deltaTijd)
    {
        _cooldownTimer += deltaTijd;

        if (_cooldownTimer >= VuurRatio)
        {
            _currentTarget?.TakeDamage(Schade);
            _cooldownTimer = 0f;
        }
    }

    public void Draw()
    {
        // Toren
        Raylib.DrawRectangle((int)Positie.X - 20, (int)Positie.Y - 20, 40, 40, Color.Blue);

        // Bereik
        Raylib.DrawCircleLines((int)Positie.X, (int)Positie.Y, Afstand, Color.LightGray);

        // Laserstraal
        if (_currentTarget is { IsAlive: true })
        {
            Raylib.DrawLineV(Positie, _currentTarget.Position, Color.Yellow);
        }
    }
}
