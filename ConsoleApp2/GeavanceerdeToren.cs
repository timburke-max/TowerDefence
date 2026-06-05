using System;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using TowerDefence.Interfaces;
using TowerDefence.Model;

namespace TowerDefence;

internal class GeanvanceerdeToren : BasicToren
{



    protected float Snelheid;


    public GeanvanceerdeToren(Vector2 position) : base(position)
    {
        Positie = position;
        Schade = 8;
        Afstand = 100;
        VuurRatio = 0.5f;
    }
    private float _currentTarget;
   
    public override void ValAan(float deltaTime)
    {
        _currentTarget.TakeDamage(Snelheid -= 10f);
    }



    

}