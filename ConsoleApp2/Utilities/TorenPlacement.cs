using System;
using System.Collections.Generic;
using System.Numerics;
using Raylib_cs;
using TowerDefence.Model;
using TowerDefence.Interfaces;

namespace TowerDefence.Utilities;

internal class TorenPlacement
{
    public enum TorenType { Basic, Advanced }
    public TorenType GekozenType { get; private set; } = TorenType.Basic;
    public bool IsPlaatsen { get; private set; } = true;
    public bool HeeftMinstensEenGeplaatst { get; private set; } = false;

    private const int MinDistanceToRoute = 60;
    private const int MinSeparationXY = 50;

    private Vector2 plaatsPos = Vector2.Zero;

    public void Bijwerken(List<IToren> torens, List<Vector2> routeWaypoints, ref int punten)
    {
        if (Raylib.IsKeyPressed(KeyboardKey.One))
        {
            GekozenType = TorenType.Basic;
            IsPlaatsen = true;
        }
        if (Raylib.IsKeyPressed(KeyboardKey.Two))
        {
            GekozenType = TorenType.Advanced;
            IsPlaatsen = true;
        }

        if (Raylib.IsKeyPressed((KeyboardKey)321))
        {
            int cost = 500;
            if (punten >= cost)
            {
                punten -= cost;
                TorenShop.AddStock(TorenType.Basic, 1);
            }
        }
        if (Raylib.IsKeyPressed((KeyboardKey)322))
        {
            int cost = 750;
            if (punten >= cost)
            {
                punten -= cost;
                TorenShop.AddStock(TorenType.Advanced, 1);
            }
        }

        if (Raylib.IsKeyPressed(KeyboardKey.Zero))
        {
            if (!HeeftMinstensEenGeplaatst)
            {
                TorenShop.SaveStock(punten);
            }
            IsPlaatsen = false;
        }

        if (!IsPlaatsen) return;

        plaatsPos = Raylib.GetMousePosition();

        bool geldig = IsGeldigePlaatsing(plaatsPos, torens, routeWaypoints);

        if (Raylib.IsKeyPressed(KeyboardKey.Nine))
        {
            if (!geldig) return;

            if (!TorenShop.HasStock(GekozenType)) return;

            if (GekozenType == TorenType.Basic)
                torens.Add(new BasicToren(plaatsPos));
            else
                torens.Add(new AdvancedToren(plaatsPos));

            HeeftMinstensEenGeplaatst = true;
            TorenShop.DecrementStock(GekozenType);
            if (TorenShop.GetStock(GekozenType) <= 0)
            {
                IsPlaatsen = false;
            }
        }
    }

    public void Draw()
    {
        if (!IsPlaatsen) return;

        var pos = plaatsPos;

        var rectColor = new Color(0, 0, 255, 90);
        var rangeColor = new Color(128, 128, 128, 90);
        float range = GekozenType == TorenType.Basic ? 150f : 200f;

        Raylib.DrawRectangle((int)pos.X - 20, (int)pos.Y - 20, 40, 40, rectColor);
        Raylib.DrawCircle((int)pos.X, (int)pos.Y, range, rangeColor);

        Raylib.DrawText(GekozenType == TorenType.Basic ? "Plaatsen: Basis (1)" : "Plaatsen: Advanced (2)", 10, Raylib.GetScreenHeight() - 60, 20, Color.DarkGray);
        Raylib.DrawText($"Voorraad: {TorenShop.GetStock(GekozenType)}", 10, Raylib.GetScreenHeight() - 40, 20, Color.DarkGray);
    }

    public bool IsGeldigePlaatsing(Vector2 pos, List<IToren> torens, List<Vector2> routeWaypoints)
    {
        if (!TorenShop.HasStock(GekozenType)) return false;

        if (routeWaypoints != null && routeWaypoints.Count >= 2)
        {
            for (int i = 0; i < routeWaypoints.Count - 1; i++)
            {
                if (DistancePointToSegment(pos, routeWaypoints[i], routeWaypoints[i + 1]) < MinDistanceToRoute)
                    return false;
            }
        }

        foreach (var t in torens)
        {
            var tpos = ((dynamic)t).Positie;
            if (Math.Abs(tpos.X - pos.X) < MinSeparationXY && Math.Abs(tpos.Y - pos.Y) < MinSeparationXY)
                return false;
        }

        if (pos.X < 0 || pos.Y < 0 || pos.X > Raylib.GetScreenWidth() || pos.Y > Raylib.GetScreenHeight())
            return false;

        return true;
    }

    public void Reset()
    {
        GekozenType = TorenType.Basic;
        IsPlaatsen = true;
        HeeftMinstensEenGeplaatst = false;
    }

    private float DistancePointToSegment(Vector2 p, Vector2 a, Vector2 b)
    {
        Vector2 ab = b - a;
        Vector2 ap = p - a;
        float abLen2 = Vector2.Dot(ab, ab);
        if (abLen2 == 0f) return Vector2.Distance(p, a);
        float t = MathF.Max(0f, MathF.Min(1f, Vector2.Dot(ap, ab) / abLen2));
        Vector2 proj = a + ab * t;
        return Vector2.Distance(p, proj);
    }
}
