using System;


namespace MonteCarloTree
{
    /// <summary>
    /// Класс случайных чисел
    /// </summary>
    [Serializable]
    public class MyRandom
    {
        public static Random rnd = new Random();
    }

    /// <summary>
    /// Типы действий
    /// </summary>
    [Serializable]
    public enum TypeAction { D, P, H }
}
