using System;
using System.Collections.Generic;

namespace MonteCarloTree
{
    [Serializable]
    public abstract class Node
    {
        /// <summary>
        /// Тип поведения узла
        /// </summary>
        public TypeAction typeBehavior;
        /// <summary>
        /// Список последующих узлов
        /// </summary>
        protected List<ActionBranch> sheets;
        /// <summary>
        /// Список действий для последующих узлов
        /// </summary>
        protected List<int> followingActions;
        /// <summary>
        /// Глубина узла
        /// </summary>
        protected int nodeDepth;
        /// <summary>
        /// Следующий узел, использовавшийся прошлый раз
        /// </summary>
        [NonSerialized] protected int indexNextNode;

        public Node()
        {
            followingActions = null;
            sheets = null;
            nodeDepth = -1;
            indexNextNode = -1;
        }

        public abstract uint GetAmountOfAction();
        public abstract double GetGeneralScore();
        public abstract void WinningGameUpgrade();
        public abstract int GetMaxDepth();

        /// <summary>
        /// Выбор следующего действия
        /// </summary>
        /// <param name="amountOfAction">Общее количество выполненных действий в дереве</param>
        /// <param name="treeDepth">Глубина дерева</param>
        /// <returns>Индекс выбранного действия</returns>
        protected int ChoiseSheet(uint amountOfAction, ref int treeDepth)
        {
            double EvaluationNewActionBranch = 0;
            if (amountOfAction != 0)
            {
                EvaluationNewActionBranch = MathF.Sqrt(2 * MathF.Log(amountOfAction));
            }
            double maxEvaluation = double.MinValue;
            int item = 0;
            for (int i = 0; i < sheets.Count; i++)
            {
                if (sheets[i] != null)
                {
                    ActionBranch sheet = sheets[i];
                    if (sheet.GetActionBranchEvaluation(amountOfAction) > maxEvaluation)
                    {
                        maxEvaluation = sheet.GetActionBranchEvaluation(amountOfAction);
                        item = i;
                    }
                }
            }
            if (maxEvaluation <= EvaluationNewActionBranch && IsAnyActions())
            {
                item = SetSheets(ref treeDepth);
                sheets[item].GetActionBranchEvaluation(amountOfAction);
            }
            return item;
        }

        /// <summary>
        /// Возврат списока действий для последующих узлов
        /// </summary>
        /// <returns>Список действий для последующих узлов</returns>
        protected List<int> GetFollowingActions()
        {
            List<int> followingActions_ = new List<int>(followingActions);
            for (int i = 0; i < sheets.Count; i++)
            {
                if (sheets[i] != null)
                {
                    followingActions_[i] = sheets[i].ActivAction;
                }
            }
            return followingActions_;
        }

        /// <summary>
        /// Проверка есть ли ещё не созданные узлы
        /// </summary>
        /// <returns>true - есть, false - нет</returns>
        protected bool IsAnyActions()
        {
            foreach (int action in followingActions)
            {
                if (action != -1)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Проверка есть ли созданные узлы
        /// </summary>
        /// <returns>true - есть, false - нет</returns>
        protected bool IsAnySheets()
        {
            foreach (ActionBranch sheet in sheets)
            {
                if (sheet != null)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Проверка есть ли ещё не созданные узлы
        /// </summary>
        /// <returns>true - есть, false - нет</returns>
        public void ClearIndexNextNode()
        {
            if (indexNextNode != -1)
            {
                sheets[indexNextNode].ClearIndexNextNode();
                indexNextNode = -1;
            }
        }

        /// <summary>
        /// Добавить случайное действие из оставшихся, как последующий узел
        /// </summary>
        /// <param name="treeDepth">Глубина дерева</param>
        /// <returns>Индекс добавленного узла</returns>
        protected int SetSheets(ref int treeDepth)
        {
            int indexAction;
            do
            {
                indexAction = MyRandom.rnd.Next(0, followingActions.Count);
            } while (followingActions[indexAction] == -1);

            sheets[indexAction] = new ActionBranch(followingActions[indexAction], GetFollowingActions(), typeBehavior, nodeDepth + 1);
            followingActions[indexAction] = -1;
            if (treeDepth == nodeDepth)
            {
                treeDepth++;
            }
            return indexAction;
        }
    }
}