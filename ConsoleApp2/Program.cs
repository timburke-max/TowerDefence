using System.Numerics;
using Raylib_cs;
using TowerDefence.Interfaces;
using TowerDefence.Model;
using TowerDefence.Utilities;

namespace TowerDefence;

class Program
{
    static int schermBreedte;
    static int schermHoogte;
    static List<IVijand> enemies = new List<IVijand>();
    static List<IToren> towers = new List<IToren>();
    static Vector2 spawnPunt;
    static Vector2 doelPunt;
    static List<Vector2> routeWaypoints;
    static Utilities.TorenPlacement placement = new Utilities.TorenPlacement();
    static float spawnTimer = 0.0f;
    static float spawnTime = 4.0f;
    static readonly float minSpawnTime = 0.5f;
    static readonly float spawnDecreaseAmount = 0.3f;
    static int nextSpawnDecreaseAt = 200;
    static int score = 0;
    static int punten = 0;
    static Random random = new Random();
    static int endHealth = 1000;
    static bool isGameOver = false;

    static void Main(string[] args)
    {
        Raylib.InitWindow(800, 600, "OOP Tower Defense Simulation");
        schermBreedte = 800;
        schermHoogte = 600;
        Raylib.SetTargetFPS(60);

        spawnPunt = new Vector2(50, 300);
        doelPunt = new Vector2(750, 300);

        TorenShop.SetStock(1, 1);

        float amplitude = 120f;
        routeWaypoints = new List<Vector2>
        {
            spawnPunt,
            new Vector2(spawnPunt.X + (doelPunt.X - spawnPunt.X) * 0.33f, spawnPunt.Y - amplitude),
            new Vector2(spawnPunt.X + (doelPunt.X - spawnPunt.X) * 0.66f, spawnPunt.Y + amplitude),
            doelPunt
        };

        while (!Raylib.WindowShouldClose())
        {
            update();
            draw();
        }

        Raylib.CloseWindow();
    }

    public static void update()
    {
        if (isGameOver)
        {
            if (Raylib.IsKeyPressed(KeyboardKey.R))
            {
                RestartGame();
            }
            return;
        }

        float deltaTime = Raylib.GetFrameTime();
        updateEntities(deltaTime);
        placement.Bijwerken(towers, routeWaypoints, ref punten);
        if (placement.HeeftMinstensEenGeplaatst)
            spawnEnemies(deltaTime);
    }

    public static void updateEntities(float deltaTime)
    {
        foreach (var tower in towers)
        {
            tower.Update(enemies, deltaTime);
        }

        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (i >= enemies.Count) continue;
            enemies[i].Update(deltaTime);
            if (enemies[i].IsAlive == false)
            {
                var removed = enemies[i];
                enemies.RemoveAt(i);
                score += 20;
                if (removed is BasicVijand) punten += 50;
                else if (removed is AdvancedVijand) punten += 70;
                else if (removed is BossVijand) punten += 110;
                else punten += 100;
                HandleScoreEffects();
                continue;
            }
            if (Vector2.Distance(enemies[i].Position, doelPunt) < 20f)
            {
                int damageToEnd = 0;
                if (enemies[i] is BasicVijand) damageToEnd = 50;
                else if (enemies[i] is AdvancedVijand) damageToEnd = 65;
                else if (enemies[i] is BossVijand) damageToEnd = 100;
                else damageToEnd = 50;
                endHealth -= damageToEnd;
                enemies.RemoveAt(i);
                if (endHealth <= 0)
                {
                    endHealth = 0;
                    enemies.Clear();
                    isGameOver = true;
                }
                else
                {
                    HandleScoreEffects();
                }
            }
        }
    }

    public static void spawnEnemies(float deltaTime)
    {
        if (isGameOver) return;

        spawnTimer += deltaTime;
        if (spawnTimer >= spawnTime)
        {
            int keuze = random.Next(20);
            if (keuze <= 10)
            {
                enemies.Add(new BasicVijand(routeWaypoints));
            }
            else if (keuze <= 17)
            {
                enemies.Add(new AdvancedVijand(routeWaypoints));
            }
            else
            {
                enemies.Add(new BossVijand(routeWaypoints));
            }

            spawnTimer = 0.0f;
        }
    }

    static void HandleScoreEffects()
    {
        if (isGameOver) return;
        {
            while (score >= nextSpawnDecreaseAt && spawnTime > minSpawnTime)
            {
                spawnTime = MathF.Max(minSpawnTime, spawnTime - spawnDecreaseAmount);
                nextSpawnDecreaseAt += 200;
            }
        }
    }

    private static void RestartGame()
    {
        enemies.Clear();
        score = 0;
        punten = 0;
        spawnTime = 4.0f;
        spawnTimer = 0.0f;
        nextSpawnDecreaseAt = 200;
        endHealth = 100;
        isGameOver = false;
        spawnPunt = new Vector2(schermBreedte * 0.05f, schermHoogte * 0.5f);
        doelPunt = new Vector2(schermBreedte * 0.95f, schermHoogte * 0.5f);

        float amplitude = 120f;
        routeWaypoints = new List<Vector2>
        {
            spawnPunt,
            new Vector2(spawnPunt.X + (doelPunt.X - spawnPunt.X) * 0.33f, spawnPunt.Y - amplitude),
            new Vector2(spawnPunt.X + (doelPunt.X - spawnPunt.X) * 0.66f, spawnPunt.Y + amplitude),
            doelPunt
        };

        towers.Clear();
        towers.Add(new BasicToren(new Vector2(schermBreedte * 0.35f, schermHoogte * 0.33f)));
        towers.Add(new AdvancedToren(new Vector2(schermBreedte * 0.65f, schermHoogte * 0.66f)));
    }

    public static void draw()
    {
        schermBreedte = Raylib.GetScreenWidth();
        schermHoogte = Raylib.GetScreenHeight();
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.Black);
        Raylib.DrawRectangle(0, 0, schermBreedte, schermHoogte, Color.RayWhite);

        if (routeWaypoints != null && routeWaypoints.Count >= 2)
        {
            for (int i = 0; i < routeWaypoints.Count - 1; i++)
            {
                Raylib.DrawLineV(routeWaypoints[i], routeWaypoints[i + 1], Color.Gold);
            }
        }

        var goalScreen = doelPunt;
        int goalRadius = Math.Max(1, 15);
        Raylib.DrawCircleV(goalScreen, goalRadius, Color.Gold);
        float endPercentage = (float)endHealth / 100f;
        endPercentage = Math.Max(0f, Math.Min(1f, endPercentage));
        int fullBarWidth = 30;
        int barHeight = 4;
        var barTopLeft = new Vector2(doelPunt.X - fullBarWidth / 2f, doelPunt.Y - 25f);
        int barWidthScreen = Math.Max(1, fullBarWidth);
        int barHeightScreen = Math.Max(1, barHeight);
        Raylib.DrawRectangle((int)barTopLeft.X, (int)barTopLeft.Y, barWidthScreen, barHeightScreen, Color.Red);
        Raylib.DrawRectangle((int)barTopLeft.X, (int)barTopLeft.Y, Math.Max(1, (int)(barWidthScreen * endPercentage)), barHeightScreen, Color.Green);
        foreach (var tower in towers)
        {
            tower.Draw();
        }

        placement.Draw();

        if (placement.IsPlaatsen)
        {
            bool valid = placement.IsGeldigePlaatsing(Raylib.GetMousePosition(), towers, routeWaypoints);
            var pos = Raylib.GetMousePosition();
            Color rectColor = valid ? new Color(0, 255, 0, 120) : new Color(255, 0, 0, 120);
            Raylib.DrawRectangle((int)pos.X - 20, (int)pos.Y - 20, 40, 40, rectColor);
            float range = placement.GekozenType == Utilities.TorenPlacement.TorenType.Basic ? 150f : 200f;
            Raylib.DrawCircleLines((int)pos.X, (int)pos.Y, range, valid ? Color.Lime : Color.Red);
        }

        foreach (var enemy in enemies)
        {
            enemy.Draw();
        }

        Raylib.DrawText("Tower Defence", 10, 10, 24, Color.DarkGray);
        Raylib.DrawText($"Score: {score}", 10, 36, 16, Color.DarkGray);
        Raylib.DrawText($"Vijanden: {enemies.Count}", 10, 56, 16, Color.DarkGray);
        Raylib.DrawText($"Spawn Time: {spawnTime:0.00}s", 10, 76, 16, Color.DarkGray);
        Raylib.DrawText($"Health: {endHealth}", 10, 96, 16, Color.DarkGray);
        Raylib.DrawText($"Punten: {punten}", 10, 116, 16, Color.DarkGray);

        if (isGameOver)
        {
            int centerX = Raylib.GetScreenWidth() / 2;
            int centerY = Raylib.GetScreenHeight() / 2;
            Raylib.DrawText("GAME OVER", centerX - 120, centerY - 20, 48, Color.Red);
            Raylib.DrawText("R to restart", centerX - 110, centerY + 40, 20, Color.DarkGray);
        }

        var mousePos = Raylib.GetMousePosition();
        int mx = (int)mousePos.X;
        int my = (int)mousePos.Y;
        Raylib.DrawText($"X: {mx} Y: {my}", 10, Raylib.GetScreenHeight() - 20, 16, Color.DarkGray);

        Raylib.EndDrawing();
    }

}