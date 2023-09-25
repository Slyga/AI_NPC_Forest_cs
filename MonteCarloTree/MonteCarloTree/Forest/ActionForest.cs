using System;
using System.Collections.Generic;
using System.Text;

namespace MonteCarloTree
{
    [Serializable]
    public class ActionForest : ProgenitorActionForest
    {
        /// <summary>
        /// Дерево родитель
        /// </summary>
        private ProgenitorActionForest progenitorActionForest;

        public ActionForest() : base() { progenitorActionForest = null; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="progenitorActionForest_">Дерево родитель</param>
        /// <param name="actionTrees_">Список деревьев</param>
        /// <param name="choiseActionTrees">Функция переключателя между деревьями</param>
        public ActionForest(ProgenitorActionForest progenitorActionForest_) 
            : base(progenitorActionForest_.actionTrees, progenitorActionForest_.GetCloneBehaviorSelector())
        {
            progenitorActionForest = new ProgenitorActionForest();
            progenitorActionForest = progenitorActionForest_;
        }

        /// <summary>
        /// Ввод счёта последнего выполненного действия
        /// </summary>
        /// <param name="scoreLastAction">Счёт последнего выполненного действия</param>
        public void SetScoreLastAction(double scoreLastAction)
        {
            if (LastActiveTree == -1) return;
            else actionTrees[LastActiveTree].SetScoreLastAction(scoreLastAction);
        }

        /// <summary>
        /// Запуск следующего действия
        /// </summary>
        /// <returns>Индекс выбранного действися</returns>
        public int Start()
        {
            LastActiveTree = behaviorSelector.GetIndexNextTree();
            return actionTrees[LastActiveTree].Start();
        }

        /// <summary>
        /// Передача инофрмации лесу родителю
        /// </summary>
        public void InformationTransfer()
        {
            for (int i = 0; i < actionTrees.Count; i++)
            {
                ActionTree actionTree = actionTrees[i];
                actionTree.InformationTransfer(progenitorActionForest.actionTrees[i]);
            }
        }
    }
}
