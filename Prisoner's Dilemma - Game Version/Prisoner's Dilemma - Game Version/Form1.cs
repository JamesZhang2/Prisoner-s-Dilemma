using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/* Created by James Ruogu Zhang on April 12, 2021
 * Unkind:
 * First Player: Grudger, 6 turns
 * Second Player: Cheater, 5 turns
 * Third Player: Detective, 7 turns
 * Fourth Player: Copycat, 6 turns
 * Fifth Player: Cooperator, 4 turns
 * Kind:
 * First Player: Copycat, 6 turns
 * Second Player: Cooperator, 4 turns
 * Third Player: Detective, 7 turns
 * Fourth Player: Grudger, 6 turns
 * Fifth Player: Cheater, 5 turns
 */

namespace Prisoner_s_Dilemma___Game_Version
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {

        }
    }

    public class ArrayList<T> : List<T>
    {

    }

    /**
     * Prisoner is the abstract class that includes many common states and behaviors
     * for the different types of Prisoners.
     */
    public abstract class Prisoner
    {

        protected ArrayList<bool> history;
        protected double probOfMistake;
        protected double score = 0;  // Score for the current generation

        public abstract bool coopOrBetray(ArrayList<bool> oppHistory);
        public abstract Prisoner clone();
        public abstract String getType();

        public void addToHistory(bool response)
        {
            history.Add(response);
        }

        public Prisoner(double probOfMistake)
        {
            history = new ArrayList<bool>();
            this.probOfMistake = probOfMistake;
        }

        public ArrayList<bool> getHistory()
        {
            return history;
        }

        public void resetScore()
        {
            score = 0;
        }

        public void clearHistory()
        {
            history = new ArrayList<bool>();
        }

        public void addScore(double scoreToAdd)
        {
            score += scoreToAdd;
        }

        public double getScore()
        {
            return score;
        }

        protected bool makeMistake(bool response)
        {
            Random random = new Random();
            return response ^ random.NextDouble() < probOfMistake;  // ^ is the XOR operator
        }

        public String toString()
        {
            return getType() + "\nTotal Score: " + score;
        }
    }

    /**
     * The RandomPrisoner randomly chooses to cooperate or betray,
     * with its probability of cooperating stored as a private instance variable.
     *
     * Special RandomPrisoners:
     * RandomPrisoner(0.0) is called a Cheater.
     * RandomPrisoner(1.0) is called a Cooperator.
     */
    public class RandomPrisoner : Prisoner
    {

        private double probOfCoop;

        public RandomPrisoner(double probOfMistake, double probOfCoop) : base(probOfMistake)
        {
            this.probOfCoop = probOfCoop;
        }


        public override bool coopOrBetray(ArrayList<bool> oppHistory)
        {
            Random random = new Random();
            return makeMistake(random.NextDouble() < probOfCoop);
        }


        public override Prisoner clone()
        {
            return new RandomPrisoner(probOfMistake, probOfCoop);
        }


        public override String getType()
        {
            if (probOfCoop == 0.0)
                return "Cheater";
            else if (probOfCoop == 1.0)
                return "Cooperator";
            else
                return "Type: Random(" + probOfCoop + ")";
        }
    }

    /**
     * The Grudger cooperates at first, but if its opponent betrays,
     * it will always betray thereafter.
     */
    public class Grudger : Prisoner
    {

        private bool opponentBetrayed;

        public Grudger(double probOfMistake) : base(probOfMistake)
        {
        }

        public override bool coopOrBetray(ArrayList<bool> oppHistory)
        {
            if (oppHistory.Count() == 0)
                opponentBetrayed = false;
            if (oppHistory.Count() != 0 && !oppHistory[oppHistory.Count() - 1])
                opponentBetrayed = true;
            return makeMistake(!opponentBetrayed);
        }

        public override Prisoner clone()
        {
            return new Grudger(probOfMistake);
        }

        public override String getType()
        {
            return "Type: Grudger";
        }
    }

    /**
     * Detective's first few responses would follow its probe.
     * If its opponent doesn't betray in any of the probe.Length rounds,
     * the Detective becomes a Cheater to exploit its opponent.
     * Otherwise, it becomes a Copycat.
     */
    public class Detective : Prisoner
    {
        private bool[] probe;
        private bool oppHasBetrayed;

        public Detective(double probOfMistake, bool[] probe) : base(probOfMistake)
        {
            this.probe = probe;
        }


        public override bool coopOrBetray(ArrayList<bool> oppHistory)
        {
            if (history.Count() < probe.Length)
            {
                if (history.Count() == 0)
                    oppHasBetrayed = false;
                else
                    oppHasBetrayed = oppHasBetrayed || !oppHistory[oppHistory.Count() - 1];
                return makeMistake(probe[history.Count()]);
            }
            else if (history.Count() == probe.Length)
            {
                oppHasBetrayed = oppHasBetrayed || !oppHistory[oppHistory.Count() - 1];
                // If opp has never betrayed, become a Cheater
                // If opp has betrayed, become a Copycat
                bool ret = oppHasBetrayed ? oppHistory[oppHistory.Count() - 1] : false;
                return makeMistake(ret);
            }
            else
            {
                bool ret = oppHasBetrayed ? oppHistory[oppHistory.Count() - 1] : false;
                return makeMistake(ret);
            }
        }


        public override Prisoner clone()
        {
            return new Detective(probOfMistake, probe);
        }


        public override String getType()
        {
            String probeStr = "";
            for (int i = 0; i < probe.Length - 1; i++)
            {
                probeStr = probeStr + probe[i] + ", ";
            }
            probeStr += probe[probe.Length - 1];
            return "Type: Detective(" + probeStr + ")";
        }
    }



    /**
     * The CopyPrisoner, in general, copies the response of their opponent's history responses
     * If its logic gate is OR, it cooperates in the first m rounds
     * and only betrays in the current round
     * if all of its opponent's responses in the previous m rounds have been to betray.
     * If its logic gate is AND, it only cooperates in the current round
     * if all of its opponent's responses in the previous m rounds have been to cooperate.
     * m is the memory of the CopyPrisoner.
     *
     * Special CopyPrisoners:
     * CopyPrisoner(OR, 1) and CopyPrisoner(AND, 1) is called a Copycat (TIT-FOR-TAT).
     * CopyPrisoner(OR, 2) is called a Copykitten (TIT-FOR-TWO-TATS).
     * CopyPrisoner(AND, 2) is called a Copylion (TWO-TITS-FOR-TAT).
     */
    public class CopyPrisoner : Prisoner
    {
        private String gate;
        private int memory;

        public CopyPrisoner(double probOfMistake, String gate, int memory) : base(probOfMistake)
        {
            this.gate = gate;
            this.memory = memory;
        }


        public override bool coopOrBetray(ArrayList<bool> oppHistory)
        {
            if (gate == "OR")
            {
                if (oppHistory.Count() < memory)
                    return makeMistake(true);
                else
                {
                    bool ret = oppHistory[oppHistory.Count() - 1];
                    for (int i = 2; i <= memory; i++)
                    {
                        ret = ret || oppHistory[oppHistory.Count() - i];
                    }
                    return makeMistake(ret);
                }
            }
            else
            {  // AND gate
                if (oppHistory.Count() == 0)
                    return makeMistake(true);
                else
                {
                    bool ret = oppHistory[oppHistory.Count() - 1];
                    for (int i = 2; i <= memory && i <= oppHistory.Count(); i++)
                    {
                        ret = ret && oppHistory[oppHistory.Count() - i];
                    }
                    return makeMistake(ret);
                }
            }
        }


        public override Prisoner clone()
        {
            return new CopyPrisoner(probOfMistake, gate, memory);
        }


        public override String getType()
        {
            return "Type: Copy(" + gate + ", " + memory + ")";
        }
    }

    /**
     * A prison is a tournament with many prisoners.
     * Every two prisoners compete with each other in a match of several rounds.
     * At the end of all the matches, the prisoners' scores are sorted.
     * The prisoners with the highest scores clone themselves and replace the prisoners
     * with the lowest scores, and a new generation begins.
     */
    public class Prison
    {

        private static Prisoner[] prisoners;
        public const double PROB_OF_MISTAKE = 0.1;
        public const int ROUNDS_PER_GAME = 10;
        public const int GENERATIONS = 50;
        // The number of prisoners to be replaced by the end of each generation
        public const int REPLACEMENT_PER_GEN = 3;
        // B: Betray, C: Cooperate
        // BC: The payoff of betraying when opponent cooperates
        public const double BC = 3.0;
        public const double CC = 2.0;
        public const double BB = 0.0;
        public const double CB = -1.0;

        public static void main(String[] args)
        {
            initializePrisoners();
            for (int i = 0; i < GENERATIONS; i++)
            {
                resetAllScores();
                startTournament();
                sortPrisoners();
                printResults(i);
                evolve();
            }
        }

        /**
         * Initialize prisoners into different types
         */
        private static void initializePrisoners()
        {
            prisoners = new Prisoner[50];
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                    prisoners[i * 5 + j] = new RandomPrisoner(PROB_OF_MISTAKE, i / 4.0);
            }
            for (int i = 25; i < 30; i++)
            {
                prisoners[i] = new Grudger(PROB_OF_MISTAKE);
            }
            for (int i = 30; i < 35; i++)
            {
                // Copycats
                prisoners[i] = new CopyPrisoner(PROB_OF_MISTAKE, "OR", 1);
            }
            for (int i = 35; i < 40; i++)
            {
                // Copykittens
                prisoners[i] = new CopyPrisoner(PROB_OF_MISTAKE, "OR", 2);
            }
            for (int i = 40; i < 45; i++)
            {
                // Copylions
                prisoners[i] = new CopyPrisoner(PROB_OF_MISTAKE, "AND", 2);
            }
            for (int i = 45; i < 50; i++)
            {
                bool[] probe = { true, false, true, true };
                prisoners[i] = new Detective(PROB_OF_MISTAKE, probe);
            }
        }

        /**
         * Debug
         * PROB_OF_MISTAKE = 0
         * ROUNDS_PER_GAME = 10
         * GENERATIONS = 1
         * Payoff: 3,2,0,-1
         * Correct Results:
         * Copycat: 57
         * Grudger: 46
         * Detective: 45
         * Cheater: 45
         * Cooperator: 29
         */
        private static void initializeTestPrisoners()
        {
            prisoners = new Prisoner[5];
            prisoners[0] = new RandomPrisoner(PROB_OF_MISTAKE, 0);
            prisoners[1] = new RandomPrisoner(PROB_OF_MISTAKE, 1);
            prisoners[2] = new Grudger(PROB_OF_MISTAKE);
            prisoners[3] = new CopyPrisoner(PROB_OF_MISTAKE, "OR", 1);
            bool[] probe = { true, false, true, true };
            prisoners[4] = new Detective(PROB_OF_MISTAKE, probe);
        }

        /**
         * Reset the scores of all the prisoners at the start of each generation
         */
        private static void resetAllScores()
        {
            foreach (Prisoner prisoner in prisoners)
            {
                prisoner.resetScore();
            }
        }

        /**
         * Every two prisoners compete with each other in a match of several rounds
         */
        private static void startTournament()
        {
            for (int i = 0; i < prisoners.Length; i++)
            {
                for (int j = i + 1; j < prisoners.Length; j++)
                {
                    prisoners[i].clearHistory();
                    prisoners[j].clearHistory();
                    startMatch(prisoners[i], prisoners[j]);
                }
            }
        }


        private static void startMatch(Prisoner p1, Prisoner p2)
        {
            for (int i = 0; i < ROUNDS_PER_GAME; i++)
            {
                // Cache choices so that the later player
                // doesn't get their opponent's choice for the current round
                bool p1Coop = p1.coopOrBetray(p2.getHistory());
                bool p2Coop = p2.coopOrBetray(p1.getHistory());
                p1.addToHistory(p1Coop);
                p2.addToHistory(p2Coop);
                // Update scores
                if (p1Coop)
                {
                    if (p2Coop)
                    {
                        p1.addScore(CC);
                        p2.addScore(CC);
                    }
                    else
                    {
                        p1.addScore(CB);
                        p2.addScore(BC);
                    }
                }
                else
                {
                    if (p2Coop)
                    {
                        p1.addScore(BC);
                        p2.addScore(CB);
                    }
                    else
                    {
                        p1.addScore(BB);
                        p2.addScore(BB);
                    }
                }
            }
            /*
            // Debug
             Console.WriteLine(p1);
             Console.WriteLine(p2);
             Console.WriteLine("-----------");
            */
        }

        /**
         * Sort the prisoners' scores in descending order using selection sort
         */
        private static void sortPrisoners()
        {
            for (int i = 0; i < prisoners.Length; i++)
            {
                int maxIndex = findMax(i);
                // Swap
                Prisoner temp = prisoners[i];
                prisoners[i] = prisoners[maxIndex];
                prisoners[maxIndex] = temp;
            }
        }

        /**
         * Find the index of the maximum score from startIndex to prisoners.Length - 1
         * @param startIndex The index to start searching from
         * @return The index of the maximum score from startIndex to prisoners.Length - 1
         */
        private static int findMax(int startIndex)
        {
            int maxID = startIndex;
            double maxScore = prisoners[startIndex].getScore();
            for (int i = startIndex; i < prisoners.Length; i++)
            {
                if (prisoners[i].getScore() > maxScore)
                {
                    maxScore = prisoners[i].getScore();
                    maxID = i;
                }
            }
            return maxID;
        }

        /**
         * Print the results for each generation
         * @param gen The generation to print results
         */
        private static void printResults(int gen)
        {
            Console.WriteLine("Generation " + (gen + 1) + ": ");
            printClasses();
             Console.WriteLine("--------------------------");
        }

        /**
         * Print how many numbers of prisoners there are for each prisoner type
         */
        private static void printClasses()
        {
            ArrayList<String> prisonerTypes = new ArrayList<String>();
            ArrayList<int> prisonerNumbers = new ArrayList<int>();
            foreach (Prisoner prisoner in prisoners)
            {
                bool isFound = false;
                for (int i = 0; i < prisonerTypes.Count(); i++)
                {
                    if (prisoner.getType().Equals(prisonerTypes[i]))
                    {
                        // Current type of prisoner is found, increment prisonerNumbers
                        prisonerNumbers[i]++;
                        isFound = true;
                        break;
                    }
                }
                // Current type of prisoner not found, add this type to the prisonerTypes list
                if (!isFound)
                {
                    prisonerTypes.Add(prisoner.getType());
                    prisonerNumbers.Add(1);
                }
            }
            // Print the results
            for (int i = 0; i < prisonerTypes.Count(); i++)
            {
                int num = prisonerNumbers[i];
                 Console.WriteLine(prisonerTypes[i] + ": " + num + (num == 1 ? " Prisoner" : " Prisoners"));
            }
        }

        /**
         * Printing the detailed results (type and score) of each prisoner
         * @param gen The generation to print results
         */
        private static void printDetailedResults(int gen)
        {
             Console.WriteLine("Generation " + (gen + 1) + ": ");
            foreach (Prisoner prisoner in prisoners)
            {
                 Console.WriteLine(prisoner);
            }
             Console.WriteLine("--------------------------");
        }

        /**
         * Evolve using the genetic algorithm
         * The prisoners with the highest scores are cloned to replace the prisoners with the lowest scores
         */
        private static void evolve()
        {
            for (int i = 0; i < REPLACEMENT_PER_GEN; i++)
            {
                prisoners[prisoners.Length - i - 1] = prisoners[i].clone();
            }
        }
    }

}
