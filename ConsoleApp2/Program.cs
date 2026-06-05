using Raylib_cs;
using System.Numerics;
using TowerDefence.Interfaces;
using TowerDefence.Model;

namespace TowerDefence;

class Program {
    const int schermBreedte = 1920;
    const int schermHoogte  = 1080;

    static GameState  huidigeState        = GameState.StartMenu;
    static Difficulty huidigeMoeilijkheid = Difficulty.Medium;
    static MapDefinition? gekozenMap      = null;

    static List<IVijand> enemies = new();
    static List<IToren>  towers  = new();
    static float spawnTimer = 0.0f;
    static float spawnTime  = 1.5f;
    static int score  = 0;
    static int levens = 5;
    static Random random = new();

    // ── MAPPEN ───────────────────────────────────────────────────────────────
    // Alle Vector2 waarden zijn geschaald van 800×600 naar 1920×1080
    // Schaal: X × 2.4  |  Y × 1.8
    static List<MapDefinition> mappen = new()
    {
        new MapDefinition(
            naam:        "Bochtige weg",
            beschrijving:"Een S-vormig pad met meerdere bochten.",
            padPunten: new List<Vector2>
            {
                new Vector2( 120,  504),
                new Vector2(1032,  504),
                new Vector2(1032,  756),
                new Vector2( 384,  756),
                new Vector2( 384,  270),
                new Vector2(1656,  270),
                new Vector2(1656, 1060),
            },
            torenPosities: new List<Vector2>
            {
                new Vector2(1176, 387),
                new Vector2( 240, 612),
                new Vector2( 744, 639),
                new Vector2(1524, 486),
            }
        ),
        new MapDefinition(
            naam:        "Spiraal weg",
            beschrijving:"Een simpele spirale weg.",
            padPunten: new List<Vector2>
            {
                new Vector2( 120,  90),
                new Vector2(1800,  90),
                new Vector2(1800, 990),
                new Vector2( 300, 990),
                new Vector2( 300, 225),
                new Vector2(1620, 225),
                new Vector2(1620, 855),
                new Vector2( 480, 855),
                new Vector2( 480, 540),
                new Vector2(1200, 540),
            },
            torenPosities: new List<Vector2>
            {
                new Vector2( 720, 324),
                new Vector2(1200, 756),
                new Vector2( 960, 324),
            }
        ),
        new MapDefinition(
            naam:        "Diagonale weg",
            beschrijving:"Vijanden lopen schuin van linksboven naar rechtsonder.",
            padPunten: new List<Vector2>
            {
                new Vector2( 120,  90),
                new Vector2(1800, 990),
            },
            torenPosities: new List<Vector2>
            {
                new Vector2( 600, 270),
                new Vector2(1200, 630),
                new Vector2( 840, 720),
            }
        )
    };

    // ── KNOPPEN ───────────────────────────────────────────────────────────────
    static Button btnEasy     = new( 720, 468, 480, 90, "Makkelijk");
    static Button btnMedium   = new( 720, 594, 480, 90, "Normaal");
    static Button btnHard     = new( 720, 720, 480, 90, "Moeilijk");
    static Button btnNaarMaps = new( 720, 900, 480, 90, "Kies kaart");

    static Button btnMap1  = new(600, 396, 720, 99, mappen[0].Naam);
    static Button btnMap2  = new(600, 540, 720, 99, mappen[1].Naam);
    static Button btnMap3  = new(600, 684, 720, 99, mappen[2].Naam);
    static Button btnTerug = new(720, 900, 480, 90, "Terug");

    static Button btnOpnieuw      = new(600, 666, 720, 99, "Opnieuw spelen");
    static Button btnNaarMainMenu = new(600, 801, 720, 99, "Terug naar menu");

    // ─────────────────────────────────────────────────────────────────────────

    static void Main(string[] args) {
        Raylib.InitWindow(schermBreedte, schermHoogte, "OOP Tower Defense Simulation");
        Raylib.ToggleFullscreen();
        Raylib.SetTargetFPS(60);

        while (!Raylib.WindowShouldClose()) {
            switch (huidigeState) {
                case GameState.StartMenu: UpdateStartMenu(); DrawStartMenu(); break;
                case GameState.MapMenu:   UpdateMapMenu();   DrawMapMenu();   break;
                case GameState.Playing:   UpdateGame();      DrawGame();      break;
                case GameState.GameOver:  UpdateGameOver();  DrawGameOver();  break;
            }
        }

        Raylib.CloseWindow();
    }

    // ── START MENU ────────────────────────────────────────────────────────────

    static void UpdateStartMenu() {
        if (btnEasy.IsClicked())     huidigeMoeilijkheid = Difficulty.Easy;
        if (btnMedium.IsClicked())   huidigeMoeilijkheid = Difficulty.Medium;
        if (btnHard.IsClicked())     huidigeMoeilijkheid = Difficulty.Hard;
        if (btnNaarMaps.IsClicked()) huidigeState = GameState.MapMenu;
    }

    static void DrawStartMenu() {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.RayWhite);
        Raylib.DrawText("Tower Defence", 516, 180, 120, Color.DarkBlue);
        Raylib.DrawText("Kies moeilijkheid:", 708, 387, 52, Color.DarkGray);
        btnEasy.Draw(  huidigeMoeilijkheid == Difficulty.Easy   ? Color.LightGray : Color.White, Color.DarkGray);
        btnMedium.Draw(huidigeMoeilijkheid == Difficulty.Medium ? Color.LightGray : Color.White, Color.DarkGray);
        btnHard.Draw(  huidigeMoeilijkheid == Difficulty.Hard   ? Color.LightGray : Color.White, Color.DarkGray);
        btnNaarMaps.Draw(Color.DarkBlue, Color.White);
        Raylib.EndDrawing();
    }

    // ── MAP MENU ──────────────────────────────────────────────────────────────

    static void UpdateMapMenu() {
        if (btnTerug.IsClicked()) huidigeState = GameState.StartMenu;
        if (btnMap1.IsClicked())  StartSpel(mappen[0]);
        if (btnMap2.IsClicked())  StartSpel(mappen[1]);
        if (btnMap3.IsClicked())  StartSpel(mappen[2]);
    }

    static void DrawMapMenu() {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.RayWhite);
        Raylib.DrawText("Kies een kaart:", 636, 234, 84, Color.DarkBlue);
        DrawMapBeschrijving(btnMap1, mappen[0], 396);
        DrawMapBeschrijving(btnMap2, mappen[1], 540);
        DrawMapBeschrijving(btnMap3, mappen[2], 684);
        btnMap1.Draw(Color.White, Color.DarkGray);
        btnMap2.Draw(Color.White, Color.DarkGray);
        btnMap3.Draw(Color.White, Color.DarkGray);
        btnTerug.Draw(Color.LightGray, Color.DarkGray);
        Raylib.EndDrawing();
    }

    static void DrawMapBeschrijving(Button knop, MapDefinition map, int y) {
        if (Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), knop.Bounds))
            Raylib.DrawText(map.Beschrijving, 420, y + 108, 34, Color.Gray);
    }

    // ── GAME START ────────────────────────────────────────────────────────────

    static void StartSpel(MapDefinition map) {
        gekozenMap = map;
        enemies.Clear();
        towers.Clear();
        score      = 0;
        levens     = 5;
        spawnTimer = 0.0f;

        spawnTime = huidigeMoeilijkheid switch {
            Difficulty.Easy   => 2.0f,
            Difficulty.Medium => 1.5f,
            Difficulty.Hard   => 0.9f,
            _                 => 1.5f
        };

        for (int i = 0; i < map.TorenPosities.Count; i++) {
            if (i % 2 == 0)
                towers.Add(new BasicToren(map.TorenPosities[i]));
            else
                towers.Add(new SniperToren(map.TorenPosities[i]));
        }

        huidigeState = GameState.Playing;
    }

    // ── GAME LOOP ─────────────────────────────────────────────────────────────

    static void UpdateGame() {
        float deltaTime = Raylib.GetFrameTime();
        UpdateEntities(deltaTime);
        SpawnEnemies(deltaTime);
    }

    static void UpdateEntities(float deltaTime) {
        foreach (var tower in towers)
            tower.Update(enemies, deltaTime);

        foreach (var enemy in enemies)
            enemy.Update(deltaTime);

        List<IVijand> teVerwijderen = new();

        foreach (var enemy in enemies) {
            if (!enemy.IsAlive) {
                score++;
                teVerwijderen.Add(enemy);
            }
            else if (enemy.HeeftDoelBereikt) {
                levens--;
                teVerwijderen.Add(enemy);
                if (levens <= 0)
                    huidigeState = GameState.GameOver;
            }
        }

        foreach (var enemy in teVerwijderen)
            enemies.Remove(enemy);
    }

    static void SpawnEnemies(float deltaTime) {
        if (huidigeState != GameState.Playing) return;

        spawnTimer += deltaTime;
        if (spawnTimer >= spawnTime) {
            var pad = gekozenMap!.PadPunten;
            if (random.Next(0, 2) == 0)
                enemies.Add(new BasicVijand(pad, huidigeMoeilijkheid));
            else
                enemies.Add(new StelleVijand(pad, huidigeMoeilijkheid));
            spawnTimer = 0.0f;
        }
    }

    static void DrawGame() {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.RayWhite);

        // Teken het pad
        var punten = gekozenMap!.PadPunten;
        for (int i = 0; i < punten.Count - 1; i++)
            Raylib.DrawLineV(punten[i], punten[i + 1], Color.Gold);
        Raylib.DrawCircleV(gekozenMap.SpawnPunt, 24, Color.Green);
        Raylib.DrawCircleV(gekozenMap.DoelPunt,  36, Color.Gold);

        foreach (var tower in towers)  tower.Draw();
        foreach (var enemy in enemies) enemy.Draw();

        // HUD
        Raylib.DrawText($"Score: {score}",           24, 18, 48, Color.DarkGray);
        Raylib.DrawText($"Kaart: {gekozenMap.Naam}", 24, 72, 44, Color.DarkGray);
        DrawLevens();

        Raylib.EndDrawing();
    }

    static void DrawLevens() {
        int startX = schermBreedte - 72;   // begin rechts (was -30 op 800px)

        for (int i = 0; i < 5; i++) {
            int x = startX - i * 84;       // spatiëring (was 35 op 800px)
            Color kleur = i < levens ? Color.Red : Color.LightGray;
            Raylib.DrawCircleV(new Vector2(x, 36), 28, kleur);
        }

        Raylib.DrawText("Levens:", startX - 5 * 84 - 168, 22, 44, Color.DarkGray);
    }

    // ── GAME OVER ─────────────────────────────────────────────────────────────

    static void UpdateGameOver() {
        if (btnOpnieuw.IsClicked())      StartSpel(gekozenMap!);
        if (btnNaarMainMenu.IsClicked()) huidigeState = GameState.StartMenu;
    }

    static void DrawGameOver() {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.RayWhite);

        int titelBreedte = Raylib.MeasureText("GAME OVER", 168);
        Raylib.DrawText("GAME OVER",
            schermBreedte / 2 - titelBreedte / 2, 216, 168, Color.Red);

        string scoreTekst = $"Score: {score}";
        Raylib.DrawText(scoreTekst,
            schermBreedte / 2 - Raylib.MeasureText(scoreTekst, 72) / 2, 414, 72, Color.DarkGray);

        string label = huidigeMoeilijkheid switch {
            Difficulty.Easy   => "Makkelijk",
            Difficulty.Medium => "Normaal",
            Difficulty.Hard   => "Moeilijk",
            _                 => ""
        };
        string infotekst = $"{gekozenMap!.Naam}  |  {label}";
        Raylib.DrawText(infotekst,
            schermBreedte / 2 - Raylib.MeasureText(infotekst, 48) / 2, 513, 48, Color.Gray);

        btnOpnieuw.Draw(Color.DarkBlue, Color.White);
        btnNaarMainMenu.Draw(Color.LightGray, Color.DarkGray);

        Raylib.EndDrawing();
    }
}