using System.Numerics;
using Raylib_cs;
using TowerDefence.Interfaces;
using TowerDefence.Model;

namespace VisualDefense;

class Programa
{
    
    const int schermBreedte = 800;
    const int schermHoogte = 600;
    
    static List<IVijand> enemies = new List<IVijand>();
    static List<IToren> towers = new List<IToren>();
    
    static Vector2 spawnPunt = new Vector2(50, 300);
    static Vector2 doelPunt = new Vector2(750, 300);
 
   
    static float spawnTimer = 0.0f;
    static float spawnTime = 1.5f;
    
    static int score = 0;
    static Random random = new Random();
    static void Main(string[] args)
    {
        Raylib.InitWindow(schermBreedte, schermHoogte, "OOP Tower Defense Simulation");
        Raylib.SetTargetFPS(60); Matrix3x2:

        towers.Add(new BasicToren(new Vector2(300, 200)));
        towers.Add(new BasicToren(new Vector2(500, 400)));
        towers.Add(new BasicToren(new Vector2(434, 348)));
        towers.Add(new GeavanceerdeToren(new Vector2(450, 392)));
     

        while (!Raylib.WindowShouldClose())
        {
            update();
            draw();
        }

        Raylib.CloseWindow();
    }
    public static void update()
    {
     
        float deltaTime = Raylib.GetFrameTime();
        updateEntities(deltaTime);
        spawnEnemies(deltaTime);
    }

   
    public static void updateEntities(float deltaTime)
    {
        
        foreach (var toren in towers)
        {
            toren.Update(enemies, deltaTime);
        }

        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            IVijand vijand = enemies[i];
            vijand.Update(deltaTime);

            
            float afstandTotDoel = Vector2.Distance(vijand.Position, doelPunt);
            if (afstandTotDoel <= 5.0f)
            {
                enemies.RemoveAt(i);
                Console.WriteLine("VIJAND HEEFT ZIJN DOEL BEREIKT!");
            }
            else if (vijand.IsAlive == false)
            {
                score = score + 10;
                enemies.RemoveAt(i);
            }
        }
    }
 
    public static void spawnEnemies(float deltaTime)
    {
        
        spawnTimer += deltaTime;
        
        if (spawnTimer >= spawnTime)
        {
            

            int keuze = random.Next(1, 3);
            if (keuze == 1)
            {
                enemies.Add(new BasicVijand(spawnPunt, doelPunt));
            }
            else
            {
                enemies.Add(new SnelleVijand(spawnPunt, doelPunt));
            }
        
          spawnTimer = 0.0f;
        }
    }
  
    public static void draw()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.RayWhite);
      
        Raylib.DrawLineV(spawnPunt, doelPunt, Color.Gold);
        Raylib.DrawCircleV(doelPunt, 15, Color.Gold);
        
        foreach (var tower in towers)
        {
            tower.Draw();
        }
     
        foreach (var enemy in enemies)
        {
            enemy.Draw();
        }
        Raylib.DrawText("Tower Defence", 10, 10, 18, Color.DarkGray);
        Raylib.DrawText("Score: " + score, 10, 40, 20, Color.Black);
        Raylib.EndDrawing();
    }
}