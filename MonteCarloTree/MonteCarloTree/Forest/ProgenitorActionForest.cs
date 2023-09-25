using System;
using System.Collections.Generic;
using System.Text;

namespace MonteCarloTree
{
    [Serializable]
    public class ProgenitorActionForest
    {
        /// <summary>
        /// Список деревьев
        /// </summary>
        public List<ActionTree> actionTrees;
        /// <summary>
        /// Переключатель между деревьями
        /// </summary>
        protected BehaviorSelector behaviorSelector;
        /// <summary>
        /// Индекс предыдущего активного дерева
        /// </summary>
        public int LastActiveTree { protected set; get; }

        public ProgenitorActionForest()
        {
            actionTrees = null;
            behaviorSelector = null;
            LastActiveTree = -1;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="actionTrees_">Список деревьев</param>
        /// <param name="behaviorSelector_">Переключатель между деревьями</param>
        public ProgenitorActionForest(List<ActionTree> actionTrees_, BehaviorSelector behaviorSelector_) : base()
        {
            actionTrees = new List<ActionTree>(actionTrees_.Count);
            for (int i = 0; i < actionTrees_.Count; i++)
            {
                actionTrees.Add(new ActionTree(actionTrees_[i]));
            }
            behaviorSelector = behaviorSelector_.GetCloneBehaviorSelector();
        }

        /// <summary>
        /// Конструктор копирования
        /// </summary>
        /// <param name="progenitorActionForest">Элемент копирования</param>
        public ProgenitorActionForest(ProgenitorActionForest progenitorActionForest) 
            : this(progenitorActionForest.actionTrees, progenitorActionForest.behaviorSelector) {}

        /// <summary>
        /// Вернуть клон переключателя поведения
        /// </summary>
        /// <returns>Клон переключателя поведения</returns>
        public TypeAction GetTypeBehaviorLastActiveTree()
        {
            return actionTrees[LastActiveTree].typeBehavior;
        }
        /// <summary>
        /// Вернуть клон переключателя поведения
        /// </summary>
        /// <returns>Клон переключателя поведения</returns>
        public BehaviorSelector GetCloneBehaviorSelector()
        {
            return behaviorSelector.GetCloneBehaviorSelector();
        }

        public void Print(int numOutputLength)
        {
            foreach (var tree in actionTrees)
            {
                Console.WriteLine("Дерево "+tree.typeBehavior);
                tree.Print(numOutputLength);
                Console.WriteLine();
            }
            Console.WriteLine("##################################################################################");
        }
    }
}