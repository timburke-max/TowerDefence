using Raylib_cs;
using System.Numerics;
using TowerDefence.Interfaces;

namespace TowerDefence.Model;

public class SprayToren : IToren
{
    private float _cooldownTimer = 0.0f;
    private IVijand _currentTarget;

    public int Schade { get; }
    public float Afstand { get; }
    public float VuurRatio { get; }
    public Vector2 Positie { get; }

    public SprayToren(Vector2 position)
    {
        Positie = position;
        Schade = 10;
        Afstand = 50.0f;
        VuurRatio = 0.5f;
    }

    public void WerkBij(List<IVijand> vijanden, float deltaTime)
    {
        ZoekDoelwit(vijanden);
        ValAan(deltaTime);
    }

    private void ZoekDoelwit(List<IVijand> enemies)
    {
        _currentTarget = null;
        foreach (var e in enemies)
        {
            if (!e.IsAlive)
                continue;

            var dist = Vector2.Distance(Positie, e.Position);
            if (dist <= Afstand)
            {
                _currentTarget = e;
                break;
            }
        }
    }

    public void Draw()
    {
        // Teken de toren als een blauw vierkant
        Raylib.DrawRectangle((int)Positie.X - 20, (int)Positie.Y - 20, 40, 40, Color.Gold);

        // Teken de range-cirkel (lichtgrijs/transparant)
        Raylib.DrawCircleLines((int)Positie.X, (int)Positie.Y, Afstand, Color.SkyBlue);

        // Als de toren een doelwit heeft, teken een laserstraal
        if (_currentTarget != null && _currentTarget.IsAlive)
        {
            Raylib.DrawLineV(Positie, _currentTarget.Position, Color.Black);
        }
    }

    public void ValAan(float deltaTime)
    {
        if (_currentTarget == null || !_currentTarget.IsAlive)
            return;

        _cooldownTimer -= deltaTime;
        if (_cooldownTimer <= 0.0f)
        {
            _currentTarget.TakeDamage(Schade);
            _cooldownTimer = VuurRatio;
        }
    }
}
