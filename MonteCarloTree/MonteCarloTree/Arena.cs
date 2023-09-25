using System;
using System.Collections.Generic;
using System.Text;

namespace MonteCarloTree
{
    public class Arena
    {
        public List<CharacterEmpty> blue, red;
        public CharacterEmpty Actor, Opponent;


        public double GetHealthPercentActor() { return Actor.GetHealthPercent(); }
        public double GetHealthPercentOpponent() { return Opponent.GetHealthPercent(); }
        public int GetNumberAttackingOpponents() { return 1; }
        public int GetProtectionOpponent() { return Opponent.GetProtect(); }

        public bool IsAlievTeam(List<CharacterEmpty> team)
        {
            foreach (var character in team)
            {
                if (character.IsAlive()) return true;
            }
            return false;
        }

        public void PrintOpponents()
        {
            Console.WriteLine("Команда синих");
            foreach (var character in blue)
            {
                character.Print();
                Console.WriteLine();
            }
            Console.WriteLine("Команда красных");
            foreach (var character in red)
            {
                character.Print();
                Console.WriteLine();
            }
        }
        public void DeadOpponents()
        {
            foreach (var character in blue)
            {
                if (!character.IsAlive())
                    character.Death();
            }
            foreach (var character in red)
            {
                if (!character.IsAlive())
                    character.Death();
            }
        }

        public void Start (List<CharacterEmpty> blue_, List<CharacterEmpty> red_)
        {
            int temp, round = 1;
            blue = blue_;
            red = red_;

            while (IsAlievTeam(blue) && IsAlievTeam(red))
            {
                //CharacterEmpty.isPrint = false;
                if (CharacterEmpty.isPrint)
                {
                    Console.WriteLine("-----------------------------------------------------");
                    Console.WriteLine("Раунд " + round);
                    PrintOpponents();
                }
                   
                int characterNumber = 0;
                while (characterNumber < blue.Count || characterNumber < red.Count)
                {
                    if (characterNumber < blue.Count)
                    {
                        CharacterEmpty.isPrint = false;
                        if (blue[characterNumber].IsAlive() && IsAlievTeam(red))
                        {
                            if (CharacterEmpty.isPrint)
                                Console.WriteLine("Ход синих");
                            Actor = blue[characterNumber];
                            temp = blue[characterNumber].GetChoiseEnemy(red);
                            Opponent = red[temp];
                            CharacterEmpty.isPrint = false;
                            Actor.Start(red[temp]);
                            CharacterEmpty.isPrint = false;
                            if (CharacterEmpty.isPrint)
                                Console.WriteLine();
                        }
                    }
                    if (characterNumber < red.Count)
                    {
                        CharacterEmpty.isPrint = false;
                        if (red[characterNumber].IsAlive() && IsAlievTeam(blue))
                        {
                            if (CharacterEmpty.isPrint)
                                Console.WriteLine("Ход красных");
                            Actor = red[characterNumber];
                            temp = red[characterNumber].GetChoiseEnemy(blue);
                            Opponent = blue[temp];
                            Actor.Start(blue[temp]);
                            if (CharacterEmpty.isPrint)
                                Console.WriteLine();
                        }
                    }
                    DeadOpponents();
                    characterNumber++;
                }
                round++;
            }
            CharacterEmpty.isPrint = true;
            if (CharacterEmpty.isPrint)
            {
                //Console.WriteLine("*****************************************************");
                if (IsAlievTeam(blue)) Console.WriteLine("Победила команда синих");
                else Console.WriteLine("Победила команда красных");
                //PrintOpponents();
            }
            foreach (var character in blue)
            {
                if (character.IsAlive())
                    character.Death();
            }
            foreach (var character in red)
            {
                if (character.IsAlive())
                    character.Death();
            }
        }
    }
}
