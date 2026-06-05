using System;
using System.Collections.Generic;
using System.Text;

namespace TowerDefence;

public enum GameState {
    StartMenu,  // Het startscherm met moeilijkheidsknopjes
    MapMenu,    // Het kaartenscherm om een map te kiezen
    Playing,     // De echte game
    GameOver   // Het scherm dat verschijnt als je verliest
}
