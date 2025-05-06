using UnityEngine;

public abstract class NPCState : ScriptableObject
{
    public abstract void EnterState(VRShooterNPC npc);
    public abstract void UpdateState(VRShooterNPC npc);
    public abstract void ExitState(VRShooterNPC npc);
}