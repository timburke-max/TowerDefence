using System.Numerics;

namespace TowerDefence.Interfaces;

public interface IVijand : IDrawable
{
    int LevensPunten { get; }
    Vector2 Position { get; }
    bool IsAlive { get; }
    void Update(float deltaTime);
    void TakeDamage(int amount);
    void ApplySlow(float factor, float duration);
}
