using UnityEngine;

namespace PlayerStates
{


    public class Idle : State<Player>
    {
        public Idle(StateMachine<Player> stateMachine) : base(stateMachine) {}

        public override void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Z))
            {
                StateMachine.CurrentState = new JumpPhase(StateMachine);
            }
            
            float angleDelta = 0f;
            if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                angleDelta += 1f;
            }
            if(Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                angleDelta -= 1f;
            }

            Owner.Angle = Mathf.Clamp(Owner.Angle + angleDelta * Time.deltaTime * Owner.TurnSpeed, -Owner.MaxTurnAngle, Owner.MaxTurnAngle);
        }
    }

}