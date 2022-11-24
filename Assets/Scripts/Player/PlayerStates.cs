using UnityEngine;

namespace PlayerStates
{
    public class Idle : State<Player>
    {
        public Idle(StateMachine<Player> stateMachine) : base(stateMachine) {}

        public override void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                StateMachine.CurrentState = new JumpPhase(StateMachine);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }

    public class Idle2 : State<Player>
    {
        public Idle2(StateMachine<Player> stateMachine) : base(stateMachine) {}

        public override void Update()
        {
            if(Input.GetKeyDown(KeyCode.J))
            {
                StateMachine.CurrentState = new JumpPhase(StateMachine);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }

    public class JumpPhase : SuperState<Player>
    {
        public float Charge {get; set;} = 0f;

        public float Angle {get; set;} = 0f;

        StateMachine<JumpPhase> _subStateMachine;

        public JumpPhase(StateMachine<Player> stateMachine) : base(stateMachine) {}

        public override void Enter()
        {
            base.Enter();
            SubStateMachine.CurrentState = new ChargingTimed(this);
        }
    }

    public abstract class Charging : SubState<Player, JumpPhase>
    {
        
        public Charging(JumpPhase superState) : base(superState) {}

        public override void Enter()
        {
            EventManager.TriggerEvent("Charge");
        }

        public override void Update()
        {
            float angleDelta = 0f;
            if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.LeftArrow))
            {
                angleDelta += 1f;
            }
            if(Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.RightArrow))
            {
                angleDelta -= 1f;
            }

            SuperState.Angle = Mathf.Clamp(SuperState.Angle + angleDelta * Time.deltaTime * Owner.TurnSpeed, -Owner.MaxTurnAngle, Owner.MaxTurnAngle);
        }
    }

    public class ChargingTimed : Charging
    {
        private float _startTime;
        
        public ChargingTimed(JumpPhase superState) : base(superState) {}

        public override void Enter()
        {
            base.Enter();
            _startTime = Time.time;
        }

        public override void Update()
        {
            base.Update();
            
            SuperState.Charge = Mathf.Clamp01((Time.time - _startTime) / Owner.ChargeDuration);

            if(Input.GetKeyUp(KeyCode.Space) || SuperState.Charge >= 1f)
            {
                StateMachine.CurrentState = new Jumping(SuperState); 
            }
        }
    }

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

            EventManager.TriggerEvent("Jump");

            //set rotation
            //init animation
        }

        public override void Exit()
        {
            //init animation
        }

        public override void Update()
        {
            float jumpTime = Mathf.Clamp01((Time.time - _startTime) / Mathf.Lerp(Owner.MinJumpDuration, Owner.MaxJumpDuration, SuperState.Charge));

            Owner.transform.position = _startPos + Quaternion.AngleAxis(SuperState.Angle, Vector3.up) * 
                new Vector3( 0f,
                Owner.JumpShape.Evaluate(jumpTime) * Owner.ChargeMaxHeight.Evaluate(SuperState.Charge) * Owner.MaxJumpHeight,
                Owner.getJumpDistance(SuperState.Charge) * jumpTime);

            if(jumpTime >= 1f)
                Owner.StateMachine.CurrentState = new Idle(Owner.StateMachine);
        }   
    }
}