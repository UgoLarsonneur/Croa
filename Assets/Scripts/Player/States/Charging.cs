using UnityEngine;

namespace PlayerStates
{


    public abstract class Charging : SubState<Player, JumpPhase>
    {

        public Charging(JumpPhase jumpPhase) : base(jumpPhase) {}

        public override void Enter()
        {
            EventManager.TriggerEvent("Charge");
        }

        public override void Update()
        {
            //Owner.MoveAngle();
        }
    }


    public class ChargingTimed : Charging
    {
        private float _startTime;
        
        public ChargingTimed(JumpPhase jumpPhase) : base(jumpPhase) {}

        public override void Enter()
        {
            base.Enter();
            _startTime = Time.time;
        }

        public override void Update()
        {
            SuperState.Charge = Mathf.Clamp01((Time.time - _startTime) / Owner.ChargeDuration);
            if(Input.GetKeyUp(KeyCode.Space) || SuperState.Charge >= 1f)
            {
                StateMachine.CurrentState = new Jumping(SuperState); 
            }

            base.Update();
        }
    }


    
}