using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Player : IComparable <Player> {
    public string playerName;
    public string level;
    public int jumps;

    public Player(string playerName, string level, int jumps)
    {
        this.playerName = playerName;
        this.level = level;
        this.jumps = jumps;
    }

    /*
     * if other's jump is bigger than this player should be placed at front.
     */ 
    public int CompareTo(Player other) {
        if (jumps == other.jumps) { return -1; }
        return jumps - other.jumps;
    }
}
