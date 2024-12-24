namespace GOBA.CORE
{
    public interface IHealth
    {
        public float GetHealth();
        public float GetMaxHealth();
        public void SetHealth(float amount);
        public void SetMaxHealth(float amount);
        public bool IsDead();
        public void Kill();
    }
}