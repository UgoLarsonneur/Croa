using UnityEngine;

namespace PlayerStates
{
    public class Drowning : State<Player>
    {
        public Drowning(StateMachine<Player> stateMachine) : base(stateMachine) {}

        public override void Enter()
        {
            Debug.Log("Perdu");
			Owner.transform.parent = null;
            EventManager.TriggerEvent("Drown");
        }
    }
}