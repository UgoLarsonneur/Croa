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
                _stateMachine.setState(new PlayerCharge(_stateMachine));
            }
        }
    }

    public class PlayerCharge : State<Player>
    {
        public PlayerCharge(StateMachine<Player> stateMachine) : base(stateMachine) {}
        public float _startTime;

        public override void enter()
        {
            _startTime = Time.time;
        }

        public override void update()
        {
            float charge = Mathf.Clamp01((Time.time - _startTime) / Owner.JumpChargeDuration);

            if(Input.GetKeyUp(KeyCode.Space) || charge >= 1f)
            {
                _stateMachine.setState(new PlayerJumping(_stateMachine, charge)); 
            }
        }
    }

    public class PlayerJumping : State<Player>
    {
        private float _charge;
        private Vector3 _startPos;
        private float _startTime;
        private float _duration;

        public PlayerJumping(StateMachine<Player> stateMachine, float charge) : base(stateMachine)
        {
            _charge = charge;
            _duration = Mathf.Lerp(Owner.MinJumpDuration, Owner.MaxJumpDuration, _charge);
        }

        public override void enter()
        {
            _startTime = Time.time;
            _startPos = Owner.transform.position;

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

            Owner.transform.position = _startPos + new Vector3(
                0f,
                Owner.JumpShape.Evaluate(jumpTime) * Owner.ChargeMaxHeight.Evaluate(_charge) * Owner.MaxJumpHeight,
                (Owner.MinJumpDist + (Owner.MaxJumpDist - Owner.MinJumpDist) * _charge) * jumpTime);

            if(jumpTime >= 1f)
                _stateMachine.setState(new PlayerIdle(_stateMachine));
        }
        
    }

}