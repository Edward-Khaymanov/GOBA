namespace GOBA.CORE
{
    public interface IHealth
    {
        public float GetHealth();
        public float GetMaxHealth();
        public void SetHealth(float health);
        public void SetMaxHealth(float health);
        public bool IsDead();
        public void Kill();
    }
}