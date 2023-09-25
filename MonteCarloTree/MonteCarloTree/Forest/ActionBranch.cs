using System;
using System.Collections.Generic;

namespace MonteCarloTree
{
    [Serializable]
    public class ActionBranch : Node
    {
        /// <summary>
        /// Действие узла
        /// </summary>
        public int ActivAction { get; set; }
        /// <summary>
        /// Стутус использования узла
        /// noActive — не использовалось
        /// isUsed — сейчас используется
        /// alreadyUsed — уже использовалось
        /// </summary>
        public enum StatusUsed { noActive, isUsed, alreadyUsed }
        /// <summary>
        /// Стутус использования узла
        /// </summary>
        private StatusUsed statusUsed { get; set; }
        /// <summary>
        /// Количество выйгрышных использований узла
        /// </summary>
        private uint winningGame;
        /// <summary>
        /// Количесво использований узла
        /// </summary>
        private uint numberOfGames;
        /// <summary>
        /// Модификатор поколения узла 
        /// </summary>
        private double generationModifier;
        /// <summary>
        /// Начальная оценка узла
        /// </summary>
        private double startingActionBranchEvaluation;
        /// <summary>
        /// Оценка узла
        /// </summary>
        [NonSerialized] private double actionBranchEvaluation;
        /// <summary>
        /// Последний счёт использованного действия
        /// </summary>
        [NonSerialized] private double lastScoreAction;
        /// <summary>
        /// Нужно ли увеличивать количество выйгрышных использований
        /// </summary>
        [NonSerialized] public bool isWinningGameUpgrade;

        public ActionBranch() : base()
        {
            ActivAction = -1;
            winningGame = 0;
            numberOfGames = 0;
            generationModifier = 0;
            startingActionBranchEvaluation = 0;
            actionBranchEvaluation = 0;
            isWinningGameUpgrade = false;
            statusUsed = StatusUsed.noActive;
            lastScoreAction = 0;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="ActivAction_">Действие узла</param>
        /// <param name="followingActions_">Список действий для последующих узлов</param>
        /// <param name="typeBehavior_">Тип поведения узла</param>
        /// <param name="nodeDepth_">Глубина узла</param>
        public ActionBranch(int ActivAction_, List<int> followingActions_, TypeAction typeBehavior_,  int nodeDepth_) : this()
        {
            ActivAction = ActivAction_;
            followingActions = new List<int>(followingActions_);
            sheets = new List<ActionBranch>(followingActions.Count);
            for (int i = 0; i < followingActions.Count; i++)
            {
                sheets.Add(null);
            }
            typeBehavior = typeBehavior_;
            nodeDepth = nodeDepth_;
        }

        /// <summary>
        /// Конструктор копирования
        /// </summary>
        /// <param name="actionBranch">Копируемый объект</param>
        public ActionBranch(ActionBranch actionBranch) : this(actionBranch.ActivAction, actionBranch.followingActions, actionBranch.typeBehavior, actionBranch.nodeDepth) 
        {
            winningGame = actionBranch.winningGame;
            numberOfGames = actionBranch.numberOfGames;
            generationModifier = actionBranch.generationModifier + actionBranch.generationModifier * ((MyRandom.rnd.Next(0, 21) - 10) / 100.0); 
            startingActionBranchEvaluation = actionBranch.startingActionBranchEvaluation;
            for (int i = 0; i < actionBranch.sheets.Count; i++)
            {
                ActionBranch sheet = actionBranch.sheets[i];
                if (sheet != null) sheets[i] = new ActionBranch(sheet);
                else sheets[i] = null;
            }
        }


        /// <summary>
        /// Возврат уравновешивающего слагаемого
        /// </summary>
        /// <param name="amountOfAction">Общее количество выполненных действий в дереве</param>
        /// <param name="numberOfGames">Количесво использований узла</param>
        /// <returns>Уравновешивающее слагаемое</returns>
        private double GetBalancingTerm(uint amountOfAction, uint numberOfGames)
        {
            if (amountOfAction == 0) amountOfAction = 1;
            if (numberOfGames == 0) numberOfGames = 1;
            return MathF.Sqrt(2 * MathF.Log(amountOfAction) / numberOfGames);
        }

        /// <summary>
        /// Вернуть максимальную глубину узла
        /// </summary>
        /// <returns>Максимальную глубина узла</returns>
        public override int GetMaxDepth()
        {
            if (IsAnySheets())
            {
                int max = 0;
                foreach (ActionBranch sheet in sheets)
                {
                    if (sheet != null)
                    {
                        int current = sheet.GetMaxDepth();
                        if (max < current)
                        {
                            max = current;
                        }
                    }
                }
                return max;
            }
            else return nodeDepth;
        }

        /// <summary>
        /// Возвращает общее количество выполненных действий у узла и у последющих узлов
        /// </summary>
        /// <returns>Общее количество выполненных действий у узла и у последющих узлов</returns>
        public override uint GetAmountOfAction()
        {
            uint sumNumberOfGames = numberOfGames;
            foreach (ActionBranch sheet in sheets)
            {
                if (sheet != null)
                {
                    sumNumberOfGames += sheet.GetAmountOfAction();
                }
            }
            return sumNumberOfGames;
        }

        /// <summary>
        /// Возврат оценки узла
        /// </summary>
        /// <param name="amountOfAction">Общее количество выполненных действий в дереве</param>
        /// <returns>Оценка узла</returns>
        public double GetActionBranchEvaluation(uint amountOfAction)
        {
            uint numberOfGames_ = numberOfGames;
            if (amountOfAction == 0) amountOfAction = 1;
            if (numberOfGames_ == 0) numberOfGames_ = 1;
            double n1 = winningGame / (double)numberOfGames_;
            double n2 = GetBalancingTerm(amountOfAction, numberOfGames);
            actionBranchEvaluation = n1 + n2 + generationModifier;
            if (startingActionBranchEvaluation == 0) startingActionBranchEvaluation = actionBranchEvaluation;
            return actionBranchEvaluation;
        }

        /// <summary>
        /// Возвращает общий счёт узла и следющих использованных узлов
        /// </summary>
        /// <returns>Общее счёт узла и последющих узлов</returns>
        public override double GetGeneralScore()
        {
            double sumScore = lastScoreAction;
            if (indexNextNode != -1)
            {
                sumScore += sheets[indexNextNode].GetGeneralScore();
            }
            return sumScore;
        }

        /// <summary>
        /// Повысить количество выйгрышных использований у узла и следующих использованных узлов
        /// </summary>
        public override void WinningGameUpgrade()
        {
            double probabilityWinning = MyRandom.rnd.Next(0, 100) / 100.0;
            if (probabilityWinning <= lastScoreAction) 
				isWinningGameUpgrade = true;
            if (indexNextNode != -1)
            {
                sheets[indexNextNode].WinningGameUpgrade();
            }
        }

        /// <summary>
        /// Возврат изменения модификатора поколения
        /// </summary>
        /// <param name="GeneralScore">Общий рейтинг дерева</param>
        /// <return>Изменение модификатора поколения</return>
        public double GetGenerationModifierChanging(double generalScore)
        {
             return Math.Abs(actionBranchEvaluation - startingActionBranchEvaluation) * lastScoreAction;
        }

        /// <summary>
        /// Передача инофрмации узлам в дерево-родитель
        /// </summary>
        /// <param name="actionBranch">Узел дерева-родителя</param>
        /// <param name="generalScore">Общий рейтинг дерева</param>
        /// <param name="increaseNumberUses">Увеличивать количество использований</param>
        public void InformationTransfer(ActionBranch actionBranch, double generalScore, bool increaseNumberUses = true)
        {
            actionBranch.startingActionBranchEvaluation = actionBranchEvaluation;
            if (increaseNumberUses) actionBranch.numberOfGames++;
            if (isWinningGameUpgrade) actionBranch.winningGame++;
            actionBranch.generationModifier += GetGenerationModifierChanging(generalScore);
            if (indexNextNode == -1)
            {
                return;
            }
            else if (actionBranch.sheets[indexNextNode] != null)
            {
                sheets[indexNextNode].InformationTransfer(actionBranch.sheets[indexNextNode], generalScore, increaseNumberUses);
            }
            else
            {
                actionBranch.sheets[indexNextNode] = new ActionBranch(sheets[indexNextNode]);
                actionBranch.sheets[indexNextNode].ClearIndexNextNode();
                actionBranch.followingActions[indexNextNode] = -1;
                sheets[indexNextNode].InformationTransfer(actionBranch.sheets[indexNextNode], generalScore, false);
            }
        }

        /// <summary>
        /// Ввод счёта последнего выполненного действия
        /// </summary>
        /// <param name="scoreLastAction">Счёт последнего выполненного действия</param>
        /// <param name="amountOfAction">Общее количество выполненных действий в дереве</param>
        public void SetScoreLastAction(double scoreLastAction, ref uint amountOfAction)
        {
            if (indexNextNode == -1 && statusUsed == StatusUsed.isUsed)
            {
                lastScoreAction = scoreLastAction;
                statusUsed = StatusUsed.alreadyUsed;
                numberOfGames++;
                amountOfAction++;
            }
            else sheets[indexNextNode].SetScoreLastAction(scoreLastAction, ref amountOfAction);
        }

        /// <summary>
        /// Выбор следующего действия
        /// </summary>
        /// <param name="amountOfAction">Общее количество выполненных действий в дереве</param>
        /// <param name="treeDepth">Глубина дерева</param>
        /// <return>Индекс выбранного действия</return>
        public int Start(ref uint amountOfAction, ref int treeDepth)
        {
            switch (statusUsed)
            {
                case StatusUsed.noActive:
                    statusUsed = StatusUsed.isUsed;
                    return ActivAction;

                case StatusUsed.alreadyUsed:
                    if (indexNextNode == -1)
                    {
                        indexNextNode = ChoiseSheet(amountOfAction, ref treeDepth);
                        return sheets[indexNextNode].Start(ref amountOfAction, ref treeDepth);
                    }
                    else return sheets[indexNextNode].Start(ref amountOfAction, ref treeDepth);
            }
            return -1;
        }

        public void Print(int depth, int treeDepth, string outputType, int outputLength)
        {
            if (nodeDepth == depth)
            {
                for (int k = 1; k <= (Math.Pow(followingActions.Count, treeDepth - depth - 1) - 0.5) * outputLength; k++)
                    Console.Write(" ");
            
                Console.Write(ActivAction + ":");
            
                if (outputType == "numberGame") 
                    Console.Write(winningGame+"/"+numberOfGames+" ");
                if (outputType == "price")
                    Console.Write("{0:F2} ", actionBranchEvaluation);
                if (outputType == "startPrice")
                    Console.Write("{0:F2} ", startingActionBranchEvaluation);
                if (outputType == "modifier")
                    Console.Write("{0:F3} ", generationModifier);
            
                for (int k = 1; k <= (Math.Pow(followingActions.Count, treeDepth - depth - 1) - 0.5) * outputLength; k++)
                    Console.Write(" ");
            }
            else if (nodeDepth < depth)
            {
                foreach (var sheet in sheets)
                {
                    if (sheet != null)
                    {
                        sheet.Print(depth, treeDepth, outputType, outputLength);
                    }
                    else
                    {
                        if (outputLength > 0)
                            for (int j = 0; j < Math.Pow(followingActions.Count, depth - nodeDepth-1); j++)
                            {
                                for (int k = 1; k <= (Math.Pow(followingActions.Count, treeDepth - depth - 1) - 0.5) * outputLength; k++)
                                    Console.Write(" ");
                                for (int i = 0; i < outputLength - 1; i++)
                                    Console.Write("_");
                                Console.Write(" ");
                                for (int k = 1; k <= (Math.Pow(followingActions.Count, treeDepth - depth - 1) - 0.5) * outputLength; k++)
                                    Console.Write(" ");
                            }
                    }
                }
            }
        }
    }
}
