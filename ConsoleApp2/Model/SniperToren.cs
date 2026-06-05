using Raylib_cs;
using System.Numerics;
using TowerDefence.Interfaces;

namespace TowerDefence.Model;

public class SniperToren : IToren {
    private float _cooldownTimer = 0.0f;
    private IVijand? _currentTarget;

    public int Schade { get; }
    public float Afstand { get; }
    public float VuurRatio { get; }
    public Vector2 Positie { get; }
    public int Kosten => 200;   // kost 200g om te kopen

    public SniperToren(Vector2 position) {
        Positie = position;
        Schade = 100;
        Afstand = 450.0f;
        VuurRatio = 2.0f;
    }

    public void Update(List<IVijand> enemies, float deltaTime) {
        ZoekDoelwit(enemies);
        if (_currentTarget != null && _currentTarget.IsAlive)
            ValAan(deltaTime);
    }

    private void ZoekDoelwit(List<IVijand> enemies) {
        if (_currentTarget != null &&
            (!_currentTarget.IsAlive || Vector2.Distance(Positie, _currentTarget.Position) > Afstand))
            _currentTarget = null;

        if (_currentTarget == null) {
            foreach (var enemy in enemies) {
                if (enemy.IsAlive && Vector2.Distance(Positie, enemy.Position) <= Afstand) {
                    _currentTarget = enemy;
                    break;
                }
            }
        }
    }

    public void ValAan(float deltaTime) {
        _cooldownTimer += deltaTime;
        if (_cooldownTimer >= VuurRatio) {
            _currentTarget!.TakeDamage(Schade);
            _cooldownTimer = 0.0f;
        }
    }

    public void Draw() {
        Raylib.DrawRectangle((int)Positie.X - 20, (int)Positie.Y - 20, 40, 40, Color.Purple);
        Raylib.DrawCircleLines((int)Positie.X, (int)Positie.Y, Afstand, Color.LightGray);
        if (_currentTarget != null && _currentTarget.IsAlive)
            Raylib.DrawLineV(Positie, _currentTarget.Position, Color.Red);
    }
}
