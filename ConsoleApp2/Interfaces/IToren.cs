using System.Numerics;

namespace TowerDefence.Interfaces;

public interface IToren : IDrawable
{
    int Schade { get; set; }
    float Afstand { get; set; }
    float VuurRatio { get; set; }
    Vector2 Positie { get; set;}
    void ValAan(float deltaTime);
    void Update(List<IVijand> enemies, float deltaTime);
}
