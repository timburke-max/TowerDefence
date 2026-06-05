using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace TowerDefence;

public class MapDefinition {
    public string Naam { get; }
    public string Beschrijving { get; }
    public List<Vector2> PadPunten { get; }
    public List<Vector2> TorenPosities { get; }

    // SpawnPunt en DoelPunt worden afgeleid uit PadPunten
    public Vector2 SpawnPunt => PadPunten[0];
    public Vector2 DoelPunt => PadPunten[PadPunten.Count - 1];

    public MapDefinition(
        string naam,
        string beschrijving,
        List<Vector2> padPunten,
        List<Vector2> torenPosities) {
        Naam = naam;
        Beschrijving = beschrijving;
        PadPunten = padPunten;
        TorenPosities = torenPosities;
    }
}