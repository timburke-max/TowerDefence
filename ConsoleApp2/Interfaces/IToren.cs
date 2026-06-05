using System.Numerics;

namespace TowerDefence.Interfaces;

public interface IToren : IDrawable
{
    int Schade { get; }
    float Afstand { get; }
    float VuurRatio { get; }
    Vector2 Positie { get;}
    void ValAan(float deltaTime);
    void Update(List<IVijand> enemies, float deltaTime);
    void Draw();
}
