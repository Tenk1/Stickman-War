using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

#region GameManager Events
public class GameMenuEvent : SDD.Events.Event
{
}
public class GamePlayEvent : SDD.Events.Event
{
}
public class GamePauseEvent : SDD.Events.Event
{
}
public class GameResumeEvent : SDD.Events.Event
{
}
public class GameOverEvent : SDD.Events.Event
{
}
public class GameVictoryEvent : SDD.Events.Event
{
}

public class GameStatisticsChangedEvent : SDD.Events.Event
{
	public int eNbLife { get; set; }
}
#endregion



#region MenuManager Events
public class EscapeButtonClickedEvent : SDD.Events.Event
{
}
public class PlayButtonClickedEvent : SDD.Events.Event
{
}
public class ResumeButtonClickedEvent : SDD.Events.Event
{
}
public class MainMenuButtonClickedEvent : SDD.Events.Event
{
}

public class QuitButtonClickedEvent : SDD.Events.Event
{ }
#endregion

#region Score Event
public class ScoreItemEvent : SDD.Events.Event
{
	public float eScore;
}
#endregion

#region Spawn Event
public class EnemySpawnEvent : SDD.Events.Event
{
	public int nbEnemie;
}

public class NoNextWaveEvent : SDD.Events.Event
{

}

public class SendNextWaveEvent : SDD.Events.Event
{

}

#endregion

#region Life Event
public class LifeEvent : SDD.Events.Event
{
    public int eLife;
}

public class EnemyLifeEvent : LifeEvent
{
    public GameObject obj;
}


public class EnemyDeath : SDD.Events.Event
{
    public GameObject obj;
}
#endregion

