using System.Numerics;
using Raylib_cs;
using TowerDefence.Interfaces;
using TowerDefence.Model;

namespace VisualDefense;

class Program
{

    // Scherm groottes
    const int schermBreedte = 800;
    const int schermHoogte = 600;
    
    //Lijsten van vijanden en torens
    static List<IVijand> enemies = new List<IVijand>();
    static List<IToren> towers = new List<IToren>();
    
    // Start- en eindpunten voor de vijanden
    static Vector2 spawnPunt = new Vector2(50, 300);
    static Vector2 doelPunt = new Vector2(750, 300);

    //Timer die bijhoudt wanneer vijanden moeten spawnen
    static float spawnTimer = 0.0f;
    static float spawnTime = 1.5f;
    static void Main(string[] args)
    {
        Raylib.InitWindow(schermBreedte, schermHoogte, "OOP Tower Defense Simulation");
        Raylib.SetTargetFPS(60);
        
        // Voeg wat torens toe
        towers.Add(new BasicToren(new Vector2(300, 200)));
        towers.Add(new BasicToren(new Vector2(500, 400)));
        //TODO: Voeg ook andere soorten torens toe

        // De echte Visuele Game Loop
        while (!Raylib.WindowShouldClose())
        {
            update();
            draw();
        }

        Raylib.CloseWindow();
    }

    public static void update()
    {
        //Bereken de tijd sinds de laatste frame
        float deltaTime = Raylib.GetFrameTime();
        updateEntities(deltaTime);
        spawnEnemies(deltaTime);         
    }

    /**
     * Functie verantwoordelijk om alle entiteiten te updaten
     */
    public static void updateEntities(float deltaTime) {

        //TODO: Zorg dat alle vijanden en torens updaten
        //TODO: Zorg dat er iets gebeurt als een vijand het doel bereikt heeft
    }

    /**
     * Functie verantwoordelijk voor het spawnen van vijanden
     */
    public static void spawnEnemies(float deltaTime) {
        //Update de spawn timer
        spawnTimer += deltaTime;
        // Spawn elke paar seconden een willekeurige vijand
        if (spawnTimer >= spawnTime)
        {
            //TODO: Zorg dat een willekeurige vijand kan spawnen
            enemies.Add(new BasicVijand(spawnPunt, doelPunt));
            // Reset de spawn timer
            spawnTimer = 0.0f;
        }
    }

    //TODO: zorg dat er ergens een score wordt bijgehouden en getoond

    public static void draw() {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.RayWhite);

        // Teken het pad/de weg
        Raylib.DrawLineV(spawnPunt, doelPunt, Color.Gold);
        Raylib.DrawCircleV(doelPunt, 15, Color.Gold);

        // Teken alle torens
        foreach (var tower in towers)
        {
            tower.Draw();
        }

        // Teken alle levende vijanden
        foreach (var enemy in enemies)
        {
            enemy.Draw();
        }

        Raylib.DrawText("Tower Defence", 10, 10, 18, Color.DarkGray);
        Raylib.EndDrawing();
    }
}