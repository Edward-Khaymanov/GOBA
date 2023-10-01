namespace GOBA
{
    public static class HeroAnimatorController
    {
        public static class Params
        {
            public const string IdleTrigger = nameof(IdleTrigger);
            public const string RunTrigger = nameof(RunTrigger);
            public const string AbilityActivationTrigger = nameof(AbilityActivationTrigger);
            public const string AbilityUseTrigger = nameof(AbilityUseTrigger);
        }

        public static class States
        {
            public const string Idle = nameof(Idle);
            public const string Running = nameof(Running);
            public const string AbilityActivation = nameof(AbilityActivation);
            public const string AbilityUse = nameof(AbilityUse);
        }

        public static class OverrideClips
        {
            public const string AbilityActivation = nameof(AbilityActivation);
            public const string AbilityUse = nameof(AbilityUse);
        }
    }
}