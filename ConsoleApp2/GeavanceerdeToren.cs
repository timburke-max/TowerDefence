using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using TowerDefence.Interfaces;
using TowerDefence.Model;

namespace TowerDefence;

internal class SpeciaalToren : IToren
{
    public string GeavanceerdeToren;


    public void BasicToren_1(Vector2 position)
    {
        Positie = position;
        Schade = 8;
        Afstand = 150.0f;
        VuurRatio = 0.5f;
       
       if (BasicToren_1 = _currentTarget)
       {
            Snelheid -= 10;
       }
    }

}
