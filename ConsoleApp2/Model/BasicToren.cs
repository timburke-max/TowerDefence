using Raylib_cs;
using System.Numerics;
using TowerDefence.Interfaces;

namespace TowerDefence.Model;

public class BasicToren : IToren
{
    private float _cooldownTimer = 0.0f;
    private IVijand _currentTarget;

    public int Schade { get; }
    public float Afstand { get; }
    public float VuurRatio { get; }
    public Vector2 Positie { get ;}

    public BasicToren(Vector2 position)
    {
        Positie = position;
        Schade = 25;
        Afstand = 150.0f;
        VuurRatio = 0.5f;
        //TODO: Maak andere torens met andere statistieken
    }

    public void Update(List<IVijand> enemies, float deltaTime)
    {
        //TODO zorg dat de juiste functies worden opgeroepen met
        // de juiste parameters
    }

    private void ZoekDoelwit(List<IVijand> enemies) {
        // TODO: Zoek een vijand binnen het bereik van de toren(Afstand)
        // Denk na over wanneer van doelwit moeten gewisseld worden   
    }

    public void Draw()
    {
        // Teken de toren als een blauw vierkant
        Raylib.DrawRectangle((int)Positie.X - 20, (int)Positie.Y - 20, 40, 40, Color.Blue);

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
        // TODO: Zorg dat de toren schade toebrengt aan zijn doelwit
        // Zorg ervoor dat er een 'cooldown' is tussen aanvallen
        // Gebruik VuurRatio en _cooldownTimer 
    }
}
