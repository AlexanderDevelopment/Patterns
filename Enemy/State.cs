public abstract class State
    {
        protected EnemyScript enemy;
        protected StateMachine stateMachine;

        protected State(EnemyScript enemy, StateMachine stateMachine)
        {
            this.enemy = enemy;
            this.stateMachine = stateMachine;
        }


        public virtual void Enter()
        {
 
        }

        

        public virtual void LogicUpdate()
        {

        }


        public virtual void Exit()
        {

        }
    }
