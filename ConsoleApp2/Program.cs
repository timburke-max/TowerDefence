using System.ComponentModel.Design;
using System.Numerics;
using System.Threading.Tasks.Sources;
using Raylib_cs;
using TowerDefence.Interfaces;
using TowerDefence.Model;
namespace VisualDefense;

class Program
{

    // Scherm groottes
    const int schermBreedte = 800;
    const int schermHoogte = 600;

    static List<IVijand> vijanden = new List<IVijand>();
    static List<IToren> torens = new List<IToren>();

    // Start- en eindpunten voor de vijanden
    static Vector2 spawnPunt = new Vector2(50, 300);
    static Vector2 doelPunt = new Vector2(750, 300);

    //Timer die bijhoudt wanneer vijanden moeten spawnen
    static float spawnTimer = 0.0f;
    static float spawnTime = 2.5f;
    static int score = 0;
    static int lives = 10;
    static int level = 1;
    static Random random = new Random();
    static int scorenodig = 50;
    public static void Levels()
    {

        if (score >= scorenodig)
        {
            level++;
            spawnTime = spawnTime - 0.25f;
            score = 0;
            scorenodig = scorenodig + 50;

        }
    }
    static void Main(string[] args)
    {
        Raylib.InitWindow(schermBreedte, schermHoogte, "OOP Tower Defense Simulation");
        Raylib.SetTargetFPS(60);

        torens.Add(new BasicToren(new Vector2(300, 250)));
        torens.Add(new BasicToren(new Vector2(450, 350)));
        torens.Add(new SniperToren(new Vector2(400, 150)));
        torens.Add(new BasicToren(new Vector2(600, 250)));
        torens.Add(new SprayToren(new Vector2(650, 300)));
        torens.Add(new SprayToren(new Vector2(100, 300)));

        while (!Raylib.WindowShouldClose())
        {
            WerkBij();
            Tekenen();
        }

        Raylib.CloseWindow();
    }

    public static void WerkBij()
    {
        float deltaTime = Raylib.GetFrameTime();
        WerkBijEntiteiten(deltaTime);
        SpawnVijanden(deltaTime);
    }

    public static void WerkBijEntiteiten(float deltaTime)
    {
        foreach (var t in torens)
        {
            t.WerkBij(vijanden, deltaTime);
        }

        for (int i = vijanden.Count - 1; i >= 0; i--)
        {
            var vijand = vijanden[i];
            vijand.Update(deltaTime);

            if (vijand.HeeftDoelBereikt)
            {
                lives -= 1;
                vijanden.RemoveAt(i);
                continue;
            }

            if (!vijand.IsAlive)
            {
                score += 10;
                vijanden.RemoveAt(i);
            }

        }
    }

    public static void SpawnVijanden(float deltaTime)
    {
        spawnTimer += deltaTime;
        if (spawnTimer >= spawnTime)
        {
            int randomkeuze = random.Next(3);
            {
                if (randomkeuze == 0)
                    vijanden.Add(new BasicVijand(spawnPunt, doelPunt));
                else if (randomkeuze == 1)
                    vijanden.Add(new MINIVijand(spawnPunt, doelPunt));
                else
                {
                    vijanden.Add(new TankVijand(spawnPunt, doelPunt));
                }
                spawnTimer = 0.0f;
            }
        }

    }


    public static void Tekenen()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.RayWhite);

        Raylib.DrawLineV(spawnPunt, doelPunt, Color.Gold);
        Raylib.DrawCircleV(doelPunt, 15, Color.Gold);
        Levels();

        foreach (var toren in torens)
        {
            toren.Draw();
        }

        foreach (var vijand in vijanden)
        {
            vijand.Draw();
        }

        Raylib.DrawText("Tower Defence", 10, 10, 18, Color.DarkGray);
        Raylib.DrawText($"Score: {score}/{scorenodig}", 10, 30, 16, Color.DarkGray);
        Raylib.DrawText($"Levens: {lives}", 10, 50, 16, Color.DarkGray);
        Raylib.DrawText($"level: {level}/10", 10, 70, 16, Color.DarkGray);
        Raylib.EndDrawing();
        if (lives <= 0)
        {
            Raylib.DrawText("Game Over!", schermBreedte / 2 - 50, schermHoogte / 2, 24, Color.Red);
            Raylib.EndDrawing();
            Task.Delay(2000).Wait();
            Raylib.CloseWindow();
        }
        if (level > 10)
        {
            Raylib.DrawText("You Win!", schermBreedte / 2 - 50, schermHoogte / 2, 24, Color.Green);
            Raylib.EndDrawing();
            Task.Delay(2000).Wait();
            Raylib.CloseWindow();
        }
    }

}

