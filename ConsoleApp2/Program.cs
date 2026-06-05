using System.Numerics;
using Raylib_cs;
using TowerDefence.Interfaces;
using TowerDefence.Model;

namespace VisualDefense;

class Program {
    const int schermBreedte = 1900;
    const int schermHoogte = 1000;

    static List<IVijand> enemies = new List<IVijand>();
    static List<IToren> towers = new List<IToren>();

    static Vector2 spawnPunt = new Vector2(50, 300);
    static Vector2 doelPunt = new Vector2(950, 300);

    static float spawnTimer = 0.0f;
    static float spawnTime = 1.5f;

    // Score: telt hoeveel vijanden gedood zijn
    static int score = 0;

    // Deaths: Aantal keer doodgegaan
    static int deaths = 0;

    // Random object voor willekeurige vijandkeuze
    static Random random = new Random();

    static void Main(string[] args) {
        Raylib.InitWindow(schermBreedte, schermHoogte, "OOP Tower Defense Simulation");
        Raylib.SetTargetFPS(60);

        // Voeg twee BasicTorens en één SniperToren toe
        towers.Add(new BasicToren(new Vector2(300, 200)));
        towers.Add(new BasicToren(new Vector2(500, 400)));
        towers.Add(new Sniper(new Vector2(400, 150))); // Andere soort toren




        while (!Raylib.WindowShouldClose()) {
            update();
            draw();
        }

        Raylib.CloseWindow();
    }

    public static void update() {
        float deltaTime = Raylib.GetFrameTime();
        updateEntities(deltaTime);
        spawnEnemies(deltaTime);
    }

    public static void updateEntities(float deltaTime) {
        // Update alle torens (elke toren zoekt een doelwit en valt aan)
        foreach (var tower in towers) {
            tower.Update(enemies, deltaTime);
        }

        // Update alle vijanden (elke vijand beweegt richting het doel)
        foreach (var enemy in enemies) {
            enemy.Update(deltaTime);
        }

        // Bewaar welke vijanden verwijderd moeten worden
        // (je kunt niet verwijderen TIJDENS een foreach-loop)
        List<IVijand> teVerwijderen = new List<IVijand>();

        foreach (var enemy in enemies) {
            // Vijand is doodgeschoten
            if (!enemy.IsAlive) {
                score++;                        // Score omhoog
                teVerwijderen.Add(enemy);
            }
            // Vijand heeft het doel bereikt
            else if (Vector2.Distance(enemy.Position, doelPunt) < 5.0f) {
                teVerwijderen.Add(enemy);       // Verwijder zonder score
                score = 0;                      // Reset score bij verlies
                deaths++;
            }
        }

        // Verwijder nu pas de vijanden, buiten de loop
        foreach (var enemy in teVerwijderen) {
            enemies.Remove(enemy);
        }
    }


    public static void spawnEnemies(float deltaTime)
    {
        spawnTimer += deltaTime;

        if (spawnTimer >= spawnTime)
        {
            // Altijd minstens 1 vijand
            enemies.Add(new BasicVijand(spawnPunt, doelPunt));

            // 50% kans op een snelle vijand
            if (random.NextDouble() < 0.5)
                enemies.Add(new Snellevijand(spawnPunt, doelPunt));

            // 20% kans op een tank
            if (random.NextDouble() < 0.2)
                enemies.Add(new Tank(spawnPunt, doelPunt));

            // Reset timer
            spawnTimer = 0.0f;

            // Tijd tot volgende spawn
            spawnTime = random.NextSingle() * 1.5f + 1.0f;
            // tussen 1.0 en 2.5 seconden
        }
    }






    public static void draw() {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.RayWhite);

        Raylib.DrawLineV(spawnPunt, doelPunt, Color.Gold);
        Raylib.DrawCircleV(doelPunt, 15, Color.Gold);

        foreach (var tower in towers) {
            tower.Draw();
        }

        foreach (var enemy in enemies) {
            enemy.Draw();
        }

        Raylib.DrawText("Tower Defence", 10, 10, 18, Color.DarkGray);

        // Toon de score linksboven
        Raylib.DrawText($"Score: {score}", 10, 35, 18, Color.DarkGray);

        // Toon de deaths onder de score
        Raylib.DrawText($"Deaths: {deaths}", 10, 50, 18, Color.DarkGray);

        Raylib.EndDrawing();
    }
}
