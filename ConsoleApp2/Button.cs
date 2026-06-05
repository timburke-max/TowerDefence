using Raylib_cs;
using System.Numerics;

namespace TowerDefence;

// Een herbruikbare knop die je kan tekenen en klikken
public class Button {
    public Rectangle Bounds { get; }   // De positie en grootte van de knop
    public string Label { get; }       // De tekst op de knop

    public Button(float x, float y, float breedte, float hoogte, string label) {
        Bounds = new Rectangle(x, y, breedte, hoogte);
        Label = label;
    }

    // Geeft true terug als de speler op de knop klikt (muisklik + overlap)
    public bool IsClicked() {
        return Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), Bounds)
               && Raylib.IsMouseButtonPressed(MouseButton.Left);
    }

    // Tekent de knop — lichter als de muis erop staat (hover-effect)
    public void Draw(Color achtergrond, Color tekstkleur) {
        bool hovered = Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), Bounds);
        Color gebruikteKleur = hovered ? Color.LightGray : achtergrond;

        Raylib.DrawRectangleRec(Bounds, gebruikteKleur);
        Raylib.DrawRectangleLinesEx(Bounds, 2, Color.DarkGray); // Rand

        // Tekst gecentreerd in de knop
        int fontSize = 20;
        int tekstBreedte = Raylib.MeasureText(Label, fontSize);
        Raylib.DrawText(
            Label,
            (int)(Bounds.X + Bounds.Width / 2 - tekstBreedte / 2),
            (int)(Bounds.Y + Bounds.Height / 2 - fontSize / 2),
            fontSize,
            tekstkleur
        );
    }
}