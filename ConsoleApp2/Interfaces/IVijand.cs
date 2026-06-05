using System.Numerics;

namespace TowerDefence.Interfaces;

public interface IVijand : IDrawable {
    int LevensPunten { get; }
    Vector2 Position { get; }
    bool IsAlive { get; }
    bool HeeftDoelBereikt { get; }
    int Beloning { get; }   // hoeveel geld de speler krijgt als deze vijand sterft
    void Update(float deltaTime);
    void TakeDamage(int amount);
}
