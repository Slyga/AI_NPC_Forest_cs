using System;


namespace MonteCarloTree
{
    [Serializable]
    public abstract class BehaviorSelector
    {
        /// <summary>
        /// Вернуть индекс следующее активного дерева
        /// </summary>
        /// <returns>Индекс следующее активного дерева</returns>
        public abstract int GetIndexNextTree();

        /// <summary>
        /// Вернуть клон переключателя поведения
        /// </summary>
        /// <returns>Клон переключателя поведения</returns>
        public abstract BehaviorSelector GetCloneBehaviorSelector();
    }
}
