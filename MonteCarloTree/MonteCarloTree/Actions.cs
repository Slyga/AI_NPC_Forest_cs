using System;
using System.Collections.Generic;
using System.Text;

namespace MonteCarloTree
{
    [Serializable]
    public abstract class ActionEmpty : Action
    {
        public string name;

        /// <summary>
        /// Тип действия
        /// </summary>
        public TypeAction typeAction;

        public CharacterEmpty addressee;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="typeAction_">Тип действия</param>
        public ActionEmpty(string name_,TypeAction typeAction_) 
        { 
            name = name_; 
            typeAction = typeAction_; 
        }

        /// <summary>
        /// Возврат типа действия
        /// </summary>
        /// <returns>Тип действия</returns>
        public override TypeAction GetTypeAction() { return typeAction; }

        public abstract ActionEmpty GetCloneAction();
        public abstract void ChoiseAddressee(CharacterEmpty actor, CharacterEmpty opponent);
    }

    [Serializable]
    public class HandDamage : ActionEmpty
    {
        public int minDamage, maxDamage;
        public int lastDamage;

        public HandDamage(int minDamage_, int maxDamage_) : base ("Атака рукой", TypeAction.D)
        {
            lastDamage = 0;
            minDamage = minDamage_;
            maxDamage = maxDamage_;
        }
        public HandDamage(HandDamage action) : this(action.minDamage, action.maxDamage) { }

        public override ActionEmpty GetCloneAction()
        {
            return new HandDamage(this);
        }

        public override double GetLastScore(TypeAction typeActionTree)
        {
            switch (typeActionTree)
            {
                case TypeAction.D:
                    if (lastDamage == 0) return -0.1;
                    //else return (lastDamage / maxDamage * 0.1);
                    else return ((double)lastDamage / maxDamage);
                case TypeAction.P:
                    if (lastDamage == 0) return -0.1;
                    else return (lastDamage / maxDamage * 0.5);
                case TypeAction.H:
                    if (lastDamage == 0) return -0.1;
                    else return (lastDamage / maxDamage * 0.05);
                default:
                    if (lastDamage == 0) return -0.1;
                    else return (lastDamage / maxDamage * 0.05);
            }
        }

        public override void ChoiseAddressee(CharacterEmpty actor, CharacterEmpty opponent)
        {
            addressee = opponent;
        }

        public override void Run() 
        {
            lastDamage = MyRandom.rnd.Next(minDamage, maxDamage) - addressee.GetProtect();
            if (lastDamage < 0) lastDamage = 0;
            addressee.currrenHealth -= lastDamage;
            if (CharacterEmpty.isPrint)
                Console.WriteLine("Damage");
            //Console.WriteLine(name + " нанёс " + addressee.name +" "+ lastDamage + " урона"); 
        }
    }

    [Serializable]
    public class HeavyBlow : ActionEmpty
    {
        public int minDamage, maxDamage;
        public int lastDamage;

        public HeavyBlow(int minDamage_, int maxDamage_) : base("Атака рукой", TypeAction.D)
        {
            lastDamage = 0;
            minDamage = minDamage_;
            maxDamage = maxDamage_;
        }
        public HeavyBlow(HeavyBlow action) : this(action.minDamage, action.maxDamage) { }

        public override ActionEmpty GetCloneAction()
        {
            return new HeavyBlow(this);
        }

        public override double GetLastScore(TypeAction typeActionTree)
        {
            switch (typeActionTree)
            {
                case TypeAction.D:
                    if (lastDamage == 0) return -0.1;
                    else return ((double)lastDamage / maxDamage);
                case TypeAction.P:
                    if (lastDamage == 0) return -0.1;
                    else return (lastDamage / maxDamage * 0.5);
                case TypeAction.H:
                    if (lastDamage == 0) return -0.1;
                    else return (lastDamage / maxDamage * 0.05);
                default:
                    if (lastDamage == 0) return -0.1;
                    else return (lastDamage / maxDamage * 0.05);
            }
        }

        public override void ChoiseAddressee(CharacterEmpty actor, CharacterEmpty opponent)
        {
            addressee = opponent;
        }

        public override void Run()
        {
            if (MyRandom.rnd.Next()%3!=0)
            {
                lastDamage = MyRandom.rnd.Next(minDamage, maxDamage) - addressee.GetProtect();
                if (lastDamage < 0) lastDamage = 0;
                addressee.currrenHealth -= lastDamage;
                //WriteLine(name + " нанёс " + addressee.name + " " + lastDamage + " урона");
            }
            else
            {
                lastDamage = 0;
                //Console.WriteLine(name + "промахнулся");
            }
        }
    }

    [Serializable]
    public class BandageHealing : ActionEmpty
    {
        public int maxHealthing;
        public int lastHealing;

        public BandageHealing(int maxHealthing_) : base("Перемотать руку бинтом", TypeAction.H)
        {
            lastHealing = 0;
            maxHealthing = maxHealthing_;
        }
        public BandageHealing(BandageHealing action) : this(action.maxHealthing) { }

        public override ActionEmpty GetCloneAction()
        {
            return new BandageHealing(this);
        }

        public override double GetLastScore(TypeAction typeActionTree)
        {
            switch (typeActionTree)
            {
                case TypeAction.D:
                    if (lastHealing == 0) return -0.1;
                    else return (lastHealing / maxHealthing * 0.5);
                case TypeAction.P:
                    if (lastHealing == 0) return -0.1;
                    else return ((double)lastHealing / maxHealthing);
                case TypeAction.H:
                    if (lastHealing == 0) return -0.1;
                    else return (lastHealing / maxHealthing * 0.1);
                default:
                    if (lastHealing == 0) return -0.1;
                    else return (lastHealing / maxHealthing * 0.1);
            }
        }

        public override void ChoiseAddressee(CharacterEmpty actor, CharacterEmpty opponent)
        {
            addressee = actor;
        }

        public override void Run()
        {
            lastHealing = maxHealthing;
            if (lastHealing > addressee.maxHealth - addressee.currrenHealth) lastHealing = addressee.maxHealth - addressee.currrenHealth;
            if (CharacterEmpty.isPrint)
                Console.WriteLine("Protect");
                //Console.WriteLine(addressee.name+ " решил " + name + " и восполнил себе " + lastHealing + " здоровья");
        }
    }
    
    [Serializable]
    public class HandProtect : ActionEmpty
    {
        public int maxProtect;
        public int lastProtect;

        public HandProtect(int maxProtect_) : base("Защититься рукой", TypeAction.P)
        {
            lastProtect = 0;
            maxProtect = maxProtect_;
        }
        public HandProtect(HandProtect action) : this(action.maxProtect) { }

        public override ActionEmpty GetCloneAction()
        {
            return new HandProtect(this);
        }

        public override double GetLastScore(TypeAction typeActionTree)
        {
            switch (typeActionTree)
            {
                case TypeAction.D:
                    if (lastProtect == 0) return -0.1;
                    else return (lastProtect / maxProtect * 0.25);
                case TypeAction.P:
                    if (lastProtect == 0) return -0.1;
                    else return ((double)lastProtect / maxProtect * 0.5);
                case TypeAction.H:
                    if (lastProtect == 0) return -0.1;
                    else return (lastProtect / maxProtect * 0.1);
                default:
                    if (lastProtect == 0) return -0.1;
                    else return (lastProtect / maxProtect * 0.1);
            }
        }

        public override void ChoiseAddressee(CharacterEmpty actor, CharacterEmpty opponent)
        {
            addressee = actor;
        }

        public override void Run()
        {
            lastProtect = maxProtect;
            //if (lastProtect > addressee.maxHealth - addressee.currrenHealth) lastProtect = addressee.maxHealth - addressee.currrenHealth;
            if (CharacterEmpty.isPrint)
                Console.WriteLine("Protect");
            //Console.WriteLine(addressee.name + " решил " + name + " и установить защиту рукой на" + lastProtect);
        }
    }
}
