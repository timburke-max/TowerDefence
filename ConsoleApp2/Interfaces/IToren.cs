using System.Numerics;

namespace TowerDefence.Interfaces;

public interface IToren : IDrawable
{
    int Schade { get; }
    float Afstand { get; }
    float VuurRatio { get; }
    Vector2 Positie { get; }
    void ValAan(float deltaTime);
    void WerkBij(List<IVijand> vijanden, float deltaTime);
}
