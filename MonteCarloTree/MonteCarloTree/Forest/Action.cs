using System;
using System.Collections.Generic;
using System.Text;

namespace MonteCarloTree
{
    [Serializable]
    public abstract class Action
    {
        /// <summary>
        /// Возврат типа действия
        /// </summary>
        /// <returns>Тип действия</returns>
        public abstract TypeAction GetTypeAction();

        /// <summary>
        /// Последний счёт действия
        /// </summary>
        /// <param name="typeActionTree">Тип дерева</param>
        /// <returns></returns>
        public abstract double GetLastScore(TypeAction typeActionTree);

        /// <summary>
        /// Запуск действия
        /// </summary>
        /// <returns>Счёт действия</returns>
        public abstract void Run();
    }
}
