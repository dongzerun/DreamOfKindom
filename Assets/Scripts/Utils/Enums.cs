using System;

[Flags]
public enum RoomType
{ 
    MinorEnemy = 1,
    EliteEnmy = 2,
    Shop = 4,
    Treasure = 8,
    RestRoom = 16,
    Boss = 32
}

public enum RoomState
{ 
    Locked,
    Visited,
    Attainable
}

public enum CardType
{ 
    Attack,
    Defense,
    Abilities,
    Heal,
}

public enum EffectTargetType
{ 
    Self,
    SingleTarget,
    ALLTargets,
}