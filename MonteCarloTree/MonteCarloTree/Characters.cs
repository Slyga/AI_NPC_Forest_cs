using System;
using System.Collections.Generic;
using System.Text;

namespace MonteCarloTree
{
    [Serializable]
    public abstract class CharacterEmpty
    {
        public string name, species;
        public int maxHealth;
        public int protect;
        
        public int buffProtect;
        public int currrenHealth;
        public bool isDead;
        static public bool isPrint = true;

        public CharacterEmpty()
        {
            name = "empty";
            species = "empty";
            maxHealth = 100;
            currrenHealth = maxHealth;
            protect = 0;
            buffProtect = 0;

            isDead = false;
        }
        public CharacterEmpty(string species_, string name_, int maxHealth_, int protect_) : this()
        {
            name = name_;
            species = species_;
            maxHealth = maxHealth_;
            currrenHealth = maxHealth;
            protect = protect_;
        }

        public bool IsAlive()
        {
            if (currrenHealth > 0) return true;
            return false;
        }

        public void Print()
        {
            if (!isPrint) return;
            Console.WriteLine(species+" "+name);
            if (IsAlive())
            {
                Console.WriteLine("Здоровье " + currrenHealth);
                Console.WriteLine("Защита " + GetProtect());
            }
            else Console.WriteLine("Мёртв");

        }

        public abstract void Start(CharacterEmpty enemy);
        public abstract void Death();
        public abstract int GetChoiseEnemy(List<CharacterEmpty> enemys);
        public double GetHealthPercent() { return currrenHealth * 1.0 / maxHealth; }
        public int GetProtect()
        { int temp = protect + buffProtect; buffProtect = 0; return temp; }
        public void SetProtect(int value)
        { protect = value; }
        //public abstract CharacterEmpty GetCloneCharacter(CharacterEmpty enemys);
    }
    [Serializable]
    public class Player : CharacterEmpty
    {
        public List<ActionEmpty> actions;

        public Player() : base() { actions = null; }
        public Player(string species_, string name_, int maxHealth_, int protect_, List<ActionEmpty> actions_)
            : base(species_, name_, maxHealth_, protect_) { actions = new List<ActionEmpty>(actions_); }
        public Player(Player player) : this(player.species, player.name, player.maxHealth, player.GetProtect(), player.actions) {}

        public override void Start(CharacterEmpty enemy)
        {
            Console.WriteLine("Выбор действия");
            for (int i = 0; i < actions.Count; i++)
            {
                Console.WriteLine(i + 1 + ") " + actions[i].name);
            }
            int choise = int.Parse(Console.ReadLine()) - 1;
            actions[choise].ChoiseAddressee(this, enemy);
            actions[choise].Run();
        }
        public override void Death()
        {
            currrenHealth = 0;
            isDead = true;
        }
        public override int GetChoiseEnemy(List<CharacterEmpty> enemys)
        {
            Console.WriteLine("Выбор врага");
            for (int i = 0; i < enemys.Count; i++)
            {
                Console.WriteLine(i + 1 + ") " + enemys[i].name);
            }
            return int.Parse(Console.ReadLine()) - 1;
        }

    }
    [Serializable]
    public class RandomNature : CharacterEmpty
    {
        public List<ActionEmpty> actions;

        public RandomNature() : base() { actions = null; }
        public RandomNature(string species_, string name_, int maxHealth_, int protect_, List<ActionEmpty> actions_)
            : base(species_, name_, maxHealth_, protect_) { actions = new List<ActionEmpty>(actions_); }
        public RandomNature(RandomNature randomNature) : this(randomNature.species, randomNature.name, randomNature.maxHealth, randomNature.GetProtect(), randomNature.actions) { }

        public override void Start(CharacterEmpty enemy)
        {
            int choise = MyRandom.rnd.Next(0, actions.Count);
            actions[choise].ChoiseAddressee(this, enemy);
            actions[choise].Run();
        }
        public override void Death()
        {
            currrenHealth = 0;
            isDead = true;
        }
        public override int GetChoiseEnemy(List<CharacterEmpty> enemys)
        {
            int temp;
            do
            {
                temp = MyRandom.rnd.Next(0, enemys.Count);
            } while (!enemys[temp].IsAlive());
            return temp;
        }

    }
    [Serializable]
    public class Goblin : CharacterEmpty
    {
        public static ProgenitorActionForest progenitorActionForest = null;
        public static List<ActionEmpty> actions = null;
        
        public static void SetActions(List<ActionEmpty> actions_, List<ActionTree> trees)
        {
            if (actions == null)
            {
                actions = actions_;

                BehaviorSelectorGoblin behaviorSelectorGoblin = new BehaviorSelectorGoblin(actions.Count);

                //List<Action> actionsTemp = new List<Action>(actions.Count);
                //for (int i = 0; i < actions.Count; i++)
                //{
                //    actionsTemp.Add(actions[i]);
                //}
                //ActionTree DamageTree = new ActionTree(actionsTemp, TypeAction.D);
                //ActionTree ProtectionTree = new ActionTree(actionsTemp, TypeAction.P);

                progenitorActionForest = new ProgenitorActionForest(trees, behaviorSelectorGoblin);
            }
        }
        public static List<Action> GetActions()
        {
            List<Action> actionsTemp = new List<Action>(actions.Count);
            for (int i = 0; i < actions.Count; i++)
            {
                actionsTemp.Add(actions[i]);
            }
            return actionsTemp;
        }

        public ActionForest actionForest;

        public Goblin() : base() 
        {
            actionForest = null;
        }
        public Goblin(string name_, int maxHealth_, int protect_)
            : base("Гоблин", name_, maxHealth_, protect_) 
        {
            actionForest = new ActionForest(progenitorActionForest);
        }

        public override void Start(CharacterEmpty enemy)
        {
            //if (isPrint) Console.WriteLine(name + " aтакует " + enemy.name);
            for (int i = 0; i < actions.Count; i++)
            {
                actions[i].ChoiseAddressee(this, enemy);
            }
            int choiseAction = actionForest.Start();
            actions[choiseAction].Run();
            actionForest.SetScoreLastAction(actions[choiseAction].GetLastScore(actionForest.GetTypeBehaviorLastActiveTree()));

        }
        public override void Death()
        {
            if (!isDead)
            {
                currrenHealth = 0;
                actionForest.InformationTransfer();
                isDead = true;
            } 
        }
        public override int GetChoiseEnemy(List<CharacterEmpty> enemys)
        {
            int temp;
            do
            {
                temp = MyRandom.rnd.Next(0, enemys.Count);
            } while (!enemys[temp].IsAlive());
            return temp;
        }
    }
    [Serializable]
    public class Ogre : CharacterEmpty
    {
        public static ProgenitorActionForest progenitorActionForest = null;
        public static List<ActionEmpty> actions = null;

        public static void SetActions(List<ActionEmpty> actions_)
        {
            if (actions == null)
            {
                actions = actions_;

                BehaviorSelectorGoblin behaviorSelectorGoblin = new BehaviorSelectorGoblin(actions.Count);

                List<int> actionsTemp = new List<int>(actions.Count);
                for (int i = 0; i < actions.Count; i++)
                {
                    actionsTemp.Add(i);
                }
                ActionTree DamageTree = new ActionTree(actionsTemp, TypeAction.D);
                ActionTree ProtectionTree = new ActionTree(actionsTemp, TypeAction.P);

                progenitorActionForest = new ProgenitorActionForest(new List<ActionTree> { DamageTree, ProtectionTree }, behaviorSelectorGoblin);
            }
        }

        public ActionForest actionForest;

        public Ogre() : base()
        {
            actionForest = null;
        }
        public Ogre(string name_, int maxHealth_, int protect_)
            : base("Огр", name_, maxHealth_, protect_)
        {
            actionForest = new ActionForest(progenitorActionForest);
        }

        public override void Start(CharacterEmpty enemy)
        {
            if (isPrint) Console.WriteLine(name + " aтакует " + enemy.name);
            for (int i = 0; i < actions.Count; i++)
            {
                actions[i].ChoiseAddressee(this, enemy);
            }
            int choiseAction = actionForest.Start();
            actions[choiseAction].Run();
            actionForest.SetScoreLastAction(actions[choiseAction].GetLastScore(actionForest.GetTypeBehaviorLastActiveTree()));
        }
        public override void Death()
        {
            if (!isDead)
            {
                currrenHealth = 0;
                actionForest.InformationTransfer();
                isDead = true;
            }
        }
        public override int GetChoiseEnemy(List<CharacterEmpty> enemys)
        {
            int temp;
            do
            {
                temp = MyRandom.rnd.Next(0, enemys.Count);
            } while (!enemys[temp].IsAlive());
            return temp;
        }
    }
}
