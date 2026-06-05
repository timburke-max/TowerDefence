using Raylib_cs;
using System.Numerics;
using System.Collections.Generic;
using TowerDefence.Interfaces;

namespace TowerDefence.Model;

public class AdvancedToren : IToren
{
    private float _cooldownTimer = 0.0f;
    private IVijand _currentTarget;

    public int Schade { get; }
    public float Afstand { get; }
    public float VuurRatio { get; }
    public Vector2 Positie { get; }

    public AdvancedToren(Vector2 position)
    {
        Positie = position;
        Schade = 75;
        Afstand = 200.0f;
        VuurRatio = 0.8f;
    }

    public void Update(List<IVijand> enemies, float deltaTime)
    {
        // Update cooldown timer
        if (_cooldownTimer > 0.0f)
        {
            _cooldownTimer -= deltaTime;
            if (_cooldownTimer < 0.0f) _cooldownTimer = 0.0f;
        }

        ZoekDoelwit(enemies);
        ValAan(deltaTime);
    }

    private void ZoekDoelwit(List<IVijand> enemies)
    {
        // Behoud huidig target als het nog steeds geldig is
        if (_currentTarget != null && _currentTarget.IsAlive)
        {
            if (Vector2.Distance(Positie, _currentTarget.Position) <= Afstand)
            {
                return;
            }
        }

        // Zoek het dichtstbijzijnde vijand binnen bereik
        IVijand nearest = null;
        float nearestDist = float.MaxValue;

        foreach (var e in enemies)
        {
            if (!e.IsAlive) continue;
            float dist = Vector2.Distance(Positie, e.Position);
            if (dist <= Afstand && dist < nearestDist)
            {
                nearest = e;
                nearestDist = dist;
            }
        }

        _currentTarget = nearest;
    }

    public void Draw()
    {
        Raylib.DrawRectangle((int)Positie.X - 20, (int)Positie.Y - 20, 40, 40, Color.Purple);

        // Teken de range-cirkel (lichtgrijs/transparant)
        Raylib.DrawCircleLines((int)Positie.X, (int)Positie.Y, Afstand, Color.LightGray);

        // Als de toren een doelwit heeft, teken een laserstraal
        if (_currentTarget != null && _currentTarget.IsAlive)
        {
            Raylib.DrawLineV(Positie, _currentTarget.Position, Color.Yellow);
        }
    }

    public void ValAan(float deltaTime)
    {
        if (_currentTarget == null) return;
        if (!_currentTarget.IsAlive)
        {
            _currentTarget = null;
            return;
        }

        float dist = Vector2.Distance(Positie, _currentTarget.Position);
        if (dist > Afstand)
        {
            _currentTarget = null;
            return;
        }

        // Als de cooldown voorbij is, val aan
        if (_cooldownTimer <= 0.0f)
        {
            _currentTarget.TakeDamage(Schade);
            _cooldownTimer = 1.0f / VuurRatio;
        }
    }
}