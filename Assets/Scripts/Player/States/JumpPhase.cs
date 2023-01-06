using UnityEngine;

namespace PlayerStates
{

    public class JumpPhase : SuperState<Player>
    {
        public float Charge {get; set;} = 0f;
        //public float Angle {get; set;} = 0f;

        IStateMachine<JumpPhase> _subStateMachine;

        public JumpPhase(StateMachine<Player> stateMachine) : base(stateMachine) {}

        public override void Enter()
        {
            base.Enter();
            CurrentState = new ChargingTimed(this);
        }
    }
    
}