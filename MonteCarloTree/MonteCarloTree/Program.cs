using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MonteCarloTree
{
    class Program
    {
        public static int ChoiseMenu()
        {
            Console.WriteLine("Ваши действия?");
            Console.WriteLine("1) Запуск арены");
            Console.WriteLine("2) Просмотр древьев");
            Console.WriteLine("3) Обучение древьев");
            Console.WriteLine("4) Сохранить деревья");
            Console.WriteLine("5) Загрузить деревья");
            Console.WriteLine("0) Выход");
            return int.Parse(Console.ReadLine());
        }

        public static void Save()
        {
            FileStream stream = File.Create("test.dat");
            BinaryFormatter formatter = new BinaryFormatter();

            formatter.Serialize(stream, Goblin.progenitorActionForest);
            stream.Close();
        }

        public static void Load()
        {
            FileStream stream = File.Open("test.dat", FileMode.Open);
            BinaryFormatter formatter = new BinaryFormatter();

            Goblin.progenitorActionForest = formatter.Deserialize(stream) as ProgenitorActionForest;
            stream.Close();
        }

        public static void StartArena()
        {
            List<CharacterEmpty> blue, red;
            Console.WriteLine("Команда синих");
            blue = TeamConfiguration();
            Console.WriteLine("Команда красных");
            red = TeamConfiguration();
            Arena arena = new Arena();
            BehaviorSelectorEmpty.arena = arena;
            arena.Start(blue, red);
        }

        public static void LearningTrees()
        {
            for (int i = 0; i < 100; i++)
            {
                List<CharacterEmpty> blue, red;
                blue = new List<CharacterEmpty>();
                red = new List<CharacterEmpty>();
                blue.Add(new Goblin("Морк", 100, 5));
                red//.Add(new Goblin("Горк", 100, 10));
                .Add(new RandomNature("Бот",
                                     "Васян",
                                     100,
                                     5,
                                     new List<ActionEmpty> { new HandDamage(5, 10), 
                                                             new HeavyBlow(15, 25),
                                                             new BandageHealing(10), 
                                                             new HandProtect(10) }));
                Arena arena = new Arena();
                BehaviorSelectorEmpty.arena = arena;
                arena.Start(blue, red);
            }
        }

        public static void PrintForest()
        {
            /// -4 — Modifier short
            /// -3 — Start price short
            /// -2 — Price short
            /// -1 — Win/NumGame short
            /// 1 — Win/NumGame
            /// 2 — Price
            /// 3 — Start price
            /// 4 — Modifier
            Console.WriteLine("1) Гоблин");
            Console.WriteLine("2) Огр");
            int choise = int.Parse(Console.ReadLine());
            switch (choise)
            {
                case 1:
                    Goblin.progenitorActionForest.Print(-1);
                    Goblin.progenitorActionForest.Print(-3);
                    Goblin.progenitorActionForest.Print(-4);
                    break;
                case 2:
                    Ogre.progenitorActionForest.Print(-1);
                    Ogre.progenitorActionForest.Print(-3);
                    Ogre.progenitorActionForest.Print(-4);
                    break;
            }
        }

        public static List<CharacterEmpty> TeamConfiguration()
        {
            List<CharacterEmpty> team = new List<CharacterEmpty>();
            int choise;
            Console.WriteLine("Кого добавить в команду?");
            Console.WriteLine("1) Игрок");
            Console.WriteLine("2) Гоблин");
            Console.WriteLine("3) Огр");
            Console.WriteLine("4) Бот");
            Console.WriteLine("0) Хватит");
            do
            {
                foreach (var character in team)
                {
                    character.Print();
                    Console.WriteLine();
                }
                choise = int.Parse(Console.ReadLine());
                switch (choise)
                {
                    case 1: team.Add(new Player("Человек", 
                                                "Васян", 
                                                100, 
                                                5, 
                                                new List<ActionEmpty> { new HandDamage(5, 10),
                                                                        new HeavyBlow(15, 25),
                                                                        new BandageHealing(10),
                                                                        new HandProtect(10) }));
                        Console.WriteLine("Игрок добавлен");
                        break;
                    case 2: team.Add(new Goblin("Морк", 100, 5 )); Console.WriteLine("Гоблин добавлен"); break;
                    case 3: team.Add(new Ogre("Горк", 200, 5)); Console.WriteLine("Огр добавлен"); break;
                    case 4: team.Add(new RandomNature("Бот",
                                         "Васян",
                                         100,
                                         10,
                                         new List<ActionEmpty> { new HandDamage(10, 20),
                                                                 new BandageHealing(5) })); break;
                }
            } while (choise != 0);
            return team;
        }

        static void Main(string[] args)
        {
            List<ActionEmpty> actions = new List<ActionEmpty> { new HandDamage(5, 10), 
                                                                new HeavyBlow(15, 25), 
                                                                new BandageHealing(10), 
                                                                new HandProtect(10) };

            List<Action> actionsTemp = new List<Action>(actions.Count);
            for (int i = 0; i < actions.Count; i++)
            {
                actionsTemp.Add(actions[i]);
            }
            ActionTree DamageTree = new ActionTree(new List<int> { 0, 1 }, TypeAction.D);
            ActionTree ProtectionTree = new ActionTree(new List<int> { 2, 3 }, TypeAction.P);

            Goblin.SetActions(actions, new List<ActionTree> { DamageTree, ProtectionTree });

            Ogre.SetActions(new List<ActionEmpty> { new HeavyBlow(20, 31), new HandProtect(5) });
            int choise;
            do
            {
                choise = ChoiseMenu();
                switch (choise)
                {
                    case 1: StartArena(); break;
                    case 2: PrintForest(); break;
                    case 3: LearningTrees(); break;
                    case 4: Save(); break;
                    case 5: Load(); break;
                }
            } while (choise != 0);
        }
    }
}
