using UnityEngine;

namespace PlayerStates
{


    public class Jumping : SubState<Player, JumpPhase>
    {
        private Vector3 _startPos;
        private float _startTime;
        private float _duration;
        

        public Jumping(JumpPhase superState) : base(superState)
        {
            _duration = Mathf.Lerp(Owner.MinJumpDuration, Owner.MaxJumpDuration, SuperState.Charge);
        }

        public override void Enter()
        {
            _startTime = Time.time;
            _startPos = Owner.transform.position;
            Owner.transform.parent = null;

            EventManager.TriggerEvent("Jump");
        }

        public override void Update()
        {
            float jumpTime = Mathf.Clamp01((Time.time - _startTime) / Mathf.Lerp(Owner.MinJumpDuration, Owner.MaxJumpDuration, SuperState.Charge));

            Owner.transform.position = _startPos + Quaternion.AngleAxis(Owner.Angle, Vector3.up) * 
                new Vector3( 0f,
                Owner.JumpShape.Evaluate(jumpTime) * Owner.ChargeMaxHeight.Evaluate(SuperState.Charge) * Owner.MaxJumpHeight,
                Owner.getJumpDistance(SuperState.Charge) * jumpTime);

            if(jumpTime >= 1f)
            {
                EventManager.TriggerEvent("Land");
                if(Owner.TryLand())
                {
                    Owner.StateMachine.CurrentState = new Idle(Owner.StateMachine);
                }
                else
                {
                    Owner.StateMachine.CurrentState = new Drowning(Owner.StateMachine);
                }

                
            }
                
        }   
    }

    
}