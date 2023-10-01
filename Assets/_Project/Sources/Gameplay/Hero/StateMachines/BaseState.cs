namespace GOBA
{
    public abstract class BaseState
    {
        public abstract void Enter();
        public abstract void Exist();
        public abstract void Update();
    }
}