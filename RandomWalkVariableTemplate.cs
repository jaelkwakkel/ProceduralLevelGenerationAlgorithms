namespace ProceduralGenerationConsole
{
    internal class RandomWalkVariableTemplate
    {
        public readonly int walkerCount;
        public readonly float turnChance;
        public readonly float fillPercentage;
        public readonly int maxSteps;
        public readonly float roomChance;
        public readonly float walkerSplitChance;
        public readonly float walkerDestroyChance;

        public RandomWalkVariableTemplate(int walkerCount, float turnChance, float fillPercentage, int maxSteps, float roomChance, float walkerSplitChance, float walkerDestroyChance)
        {
            this.walkerCount = walkerCount;
            this.turnChance = turnChance;
            this.fillPercentage = fillPercentage;
            this.maxSteps = maxSteps;
            this.roomChance = roomChance;
            this.walkerSplitChance = walkerSplitChance;
            this.walkerDestroyChance = walkerDestroyChance;
        }
    }
}
