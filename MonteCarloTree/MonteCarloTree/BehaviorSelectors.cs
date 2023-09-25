using System;
using System.Collections.Generic;
using System.Text;

namespace MonteCarloTree
{
    [Serializable]
    public abstract class BehaviorSelectorEmpty : BehaviorSelector
    {
        public List<int> ScoreTrees;
        public static Arena arena = null;

        public BehaviorSelectorEmpty()
        {
            ScoreTrees = null; 
        }
        public BehaviorSelectorEmpty(int numTree) : this()
        {
            ScoreTrees = new List<int>(numTree);
            for (int i = 0; i < numTree; i++)
            {
                ScoreTrees.Add(0);
            }
        }
        public BehaviorSelectorEmpty(BehaviorSelectorEmpty behaviorSelector) : this(behaviorSelector.ScoreTrees.Count) { }
    
        public int GetIndexMaxScoreTree()
        {
            int maxScore = ScoreTrees[0], index = 0;
            for (int i = 1; i < ScoreTrees.Count; i++)
            {
                int scoreTree = ScoreTrees[i];
                if (scoreTree > maxScore)
                {
                    maxScore = scoreTree;
                    index = i;
                }
            }
            return index;
        }
    }

    [Serializable]
    public class BehaviorSelectorGoblin : BehaviorSelectorEmpty
    {

        public BehaviorSelectorGoblin() : base() { }
        public BehaviorSelectorGoblin(int numTree) : base(numTree) { }
        public BehaviorSelectorGoblin(BehaviorSelectorEmpty behaviorSelector) : base(behaviorSelector) { }
        
        /// <summary>
        /// Вернуть индекс следующее активного дерева
        /// </summary>
        /// <returns>Индекс следующее активного дерева</returns>
        public override int GetIndexNextTree()
        {
            //return 0;

            for (int i = 0; i < ScoreTrees.Count; i++) ScoreTrees[i] = 0;

            if (arena.GetHealthPercentOpponent() <= 0.1) ScoreTrees[0]++;
            if (arena.GetNumberAttackingOpponents() >= 2) ScoreTrees[0]++;
            if (arena.GetHealthPercentActor() <= 0.3) ScoreTrees[1]++;
            if (arena.GetProtectionOpponent() >= 20) ScoreTrees[1]++;


            return GetIndexMaxScoreTree();
        }

        /// <summary>
        /// Вернуть клон переключателя поведения
        /// </summary>
        /// <returns>Клон переключателя поведения</returns>
        public override BehaviorSelector GetCloneBehaviorSelector()
        {
            return new BehaviorSelectorGoblin(this);
        }
    }

    [Serializable]
    public class BehaviorSelectorOgre : BehaviorSelectorEmpty
    {

        public BehaviorSelectorOgre() : base() { }
        public BehaviorSelectorOgre(int numTree) : base(numTree) { }
        public BehaviorSelectorOgre(BehaviorSelectorEmpty behaviorSelector) : base(behaviorSelector) { }

        /// <summary>
        /// Вернуть индекс следующее активного дерева
        /// </summary>
        /// <returns>Индекс следующее активного дерева</returns>
        public override int GetIndexNextTree()
        {
            for (int i = 0; i < ScoreTrees.Count; i++) ScoreTrees[i] = 0;

            if (arena.GetHealthPercentOpponent() <= 0.2) ScoreTrees[0]++;
            if (arena.GetNumberAttackingOpponents() >= 1) ScoreTrees[0]++;
            if (arena.GetHealthPercentActor() <= 0.1) ScoreTrees[1]++;
            if (arena.GetProtectionOpponent() >= 20) ScoreTrees[1]++;


            return GetIndexMaxScoreTree();
        }

        /// <summary>
        /// Вернуть клон переключателя поведения
        /// </summary>
        /// <returns>Клон переключателя поведения</returns>
        public override BehaviorSelector GetCloneBehaviorSelector()
        {
            return new BehaviorSelectorOgre(this);
        }
    }
}
