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
            Owner.MoveAngle();
        }
    }


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


    public abstract class Charging : SubState<Player, JumpPhase>
    {

        public Charging(JumpPhase jumpPhase) : base(jumpPhase) {}

        public override void Enter()
        {
            EventManager.TriggerEvent("Charge");
        }

        public override void Update()
        {
            Owner.MoveAngle();
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
                Physics.Raycast(Owner.transform.position+Vector3.up*10f, Vector3.down, out RaycastHit hit, 10f, LayerMask.GetMask("Lily"));
                if(hit.collider != null)
                {
                    Transform lilyModel = hit.collider.gameObject.transform;
                    Owner.transform.parent = lilyModel;
                    Owner.transform.localPosition = new Vector3(Owner.transform.localPosition.x, 0f, Owner.transform.localPosition.z);
                    
                }
                else
                {
                    //Owner.transform.parent = null;
                    Debug.Log("Perdu");
                }

                Owner.StateMachine.CurrentState = new Idle(Owner.StateMachine);
            }
                
        }   
    }

/*     public class Landing : State<Player>
    {

        public Landing(StateMachine<Player> stateMachine) : base(stateMachine) {}

        public override void Enter()
        {
            
            base.Enter();
            //TODO: Clean


        }

        public override void Exit()
        {
            base.Exit();
            EventManager.TriggerEvent("Finished Landing");
        }

    } */
}