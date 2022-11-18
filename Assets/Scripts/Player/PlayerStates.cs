using UnityEngine;

namespace PlayerStates
{
    public class PlayerIdle : State<Player>
    {
        public PlayerIdle(StateMachine<Player> stateMachine) : base(stateMachine) {}

        public override void update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                _stateMachine.setState(new PlayerChargeTimed(_stateMachine));
            }
        }
    }

    /// <summary>
    /// Créée au cas où je veux d'autre moyen de charger le saut
    /// </summary>
    public abstract class PlayerCharge : State<Player>
    {
        protected float _charge = 0f;
        public float Charge => _charge;

        protected float _angle;
        public float Angle => _angle;
        
        public PlayerCharge(StateMachine<Player> stateMachine, float angle = 0f) : base(stateMachine)
        {
            _angle = angle;
        }

        public override void enter()
        {
            EventManager.TriggerEvent("Charge");
        }

        public override void update()
        {
            float angleDelta = 0f;
            if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.LeftArrow))
            {
                angleDelta += 1f;
            }
            if(Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.R))
            {
                angleDelta -= 1f;
            }

            _angle = Mathf.Clamp(_angle + angleDelta * Time.deltaTime * Owner.TurnSpeed, -Owner.MaxTurnAngle, Owner.MaxTurnAngle);
        }
    }

    public class PlayerChargeTimed : PlayerCharge
    {
        private float _startTime;
        
        public PlayerChargeTimed(StateMachine<Player> stateMachine, float angle = 0f) : base(stateMachine, angle) {}

        public override void enter()
        {
            base.enter();
            _startTime = Time.time;
        }

        public override void update()
        {
            base.update();
            
            _charge = Mathf.Clamp01((Time.time - _startTime) / Owner.ChargeDuration);

            if(Input.GetKeyUp(KeyCode.Space) || _charge >= 1f)
            {
                _stateMachine.setState(new PlayerJumping(_stateMachine, _charge, _angle)); 
            }
        }
    }

    public class PlayerJumping : State<Player>
    {
        private float _charge;
        private Vector3 _startPos;
        private float _startTime;
        private float _duration;
        private float _angle;
        

        public PlayerJumping(StateMachine<Player> stateMachine, float charge, float angle) : base(stateMachine)
        {
            _angle = angle;
            _charge = charge;
            _duration = Mathf.Lerp(Owner.MinJumpDuration, Owner.MaxJumpDuration, _charge);
        }

        public override void enter()
        {
            _startTime = Time.time;
            _startPos = Owner.transform.position;

            EventManager.TriggerEvent("Jump");

            //set rotation
            //init animation
        }

        public override void exit()
        {
            //init animation
        }

        public override void update()
        {
            float jumpTime = Mathf.Clamp01((Time.time - _startTime) / Mathf.Lerp(Owner.MinJumpDuration, Owner.MaxJumpDuration, _charge));

            Owner.transform.position = _startPos + Quaternion.AngleAxis(_angle, Vector3.up) * 
                new Vector3( 0f,
                Owner.JumpShape.Evaluate(jumpTime) * Owner.ChargeMaxHeight.Evaluate(_charge) * Owner.MaxJumpHeight,
                Owner.getJumpDistance(_charge) * jumpTime);

            if(jumpTime >= 1f)
                _stateMachine.setState(new PlayerIdle(_stateMachine));
        }
        
    }

}