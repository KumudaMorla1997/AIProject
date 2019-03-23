using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Artificial_Intelligence
{

    class Category //class Category
    {
        //main attributes are initialized
        public List<string> uniqueWords = new List<string>();
        List<double> frequencies = new List<double>();
        public List<double> probabilities = new List<double>();
        public double totalWords = new double();

        public void CreateCategory(List<string> trainingDoc) //this method assigns unique words, frequencies and total words.  
        {
            foreach (string s in trainingDoc)
            {
                uniqueWords.Add(s);
                for (int x = 0; x < (uniqueWords.Count - 1); x++)
                {
                    if (s == uniqueWords[x])
                    {
                        uniqueWords.Remove(s);
                    }
                }
            }

            double i = 0;
            foreach (string s in uniqueWords)
            {
                foreach (string st in trainingDoc)
                {
                    if (st == s) { i++; }
                }
                frequencies.Add(i);
                i = 0;
            }
            totalWords = trainingDoc.Count();
        }

        public void GetProbabilites(double allUniqueTotal) //this method gets the probabilities using the unique words from all the documents together.
        {

            foreach (double x in frequencies)
            {
                double i = (x + 1) / (totalWords + allUniqueTotal);
                probabilities.Add(i);
            }
        }

    }

    class Training //Training class
    {
        //initialzes the main attributes
        Category coal = new Category();
        Category con = new Category();
        Category lab = new Category();
        double uniqueTotal = new double();

        public void CreateTraining() //assigns all the attributes with the relavent information
        {
            //gets the data from the training text documents
            string driveLetter = Path.GetPathRoot(Environment.CurrentDirectory);
            String[] trainingDocOne = File.ReadAllText(driveLetter+ "TrainingFiles\\Coalition9thMay2012.txt").Split(' ', ',', '.', '\r', '\n', ':', '/', '"', '\'');
            String[] trainingDocTwo = File.ReadAllText(driveLetter+"TrainingFiles\\Conservative16thNov1994.txt").Split(' ', ',', '.', '\r', '\n', ':', '/', '"', '\'');
            String[] trainingDocThree = File.ReadAllText(driveLetter+"TrainingFiles\\Conservative27thMay2015.txt").Split(' ', ',', '.', '\r', '\n', ':', '/', '"', '\'');
            String[] trainingDocFour = File.ReadAllText(driveLetter+"TrainingFiles\\Labour6thNov2007.txt").Split(' ', ',', '.', '\r', '\n', ':', '/', '"', '\'');
            String[] trainingDocFive = File.ReadAllText(driveLetter+"TrainingFiles\\Labour26thNov2003.txt").Split(' ', ',', '.', '\r', '\n', ':', '/', '"', '\'');
            String[] trainingDocSix = File.ReadAllText(driveLetter+"TrainingFiles\\LibDemConCoalitionMay2010.txt").Split(' ', ',', '.', '\r', '\n', ':', '/', '"', '\'');
            String[] sWords = File.ReadAllText(driveLetter+"TrainingFiles\\stopwords.txt").Split('\r', '\n');

            List<string> coalition = new List<string>();
            List<string> conservative = new List<string>();
            List<string> labour = new List<string>();
            List<string> stopWords = new List<string>();

            //this places the text from the text documents into list
            void ListCreator(String[] doc, List<string> b)
            {
                foreach (string s in doc)
                {
                    string x = s.ToLower();
                    b.Add(x);
                    if (s == "") { b.Remove(x); }
                }
            }

            ListCreator(trainingDocOne, coalition);
            ListCreator(trainingDocSix, coalition);
            ListCreator(trainingDocTwo, conservative);
            ListCreator(trainingDocThree, conservative);
            ListCreator(trainingDocFour, labour);
            ListCreator(trainingDocFive, labour);
            ListCreator(sWords, stopWords);

            //this removes the stop words
            void RemoveStopwords(List<string> i)
            {
                List<string> y = new List<string>();
                foreach (string s in i)
                {
                    y.Add(s);
                    foreach (string x in stopWords)
                    {
                        if (x == s) { y.Remove(x); }
                    }
                }

                i.Clear();
                foreach (string s in y)
                {
                    i.Add(s);
                }
            }

            RemoveStopwords(coalition);
            RemoveStopwords(conservative);
            RemoveStopwords(labour);

            coal.CreateCategory(coalition);
            con.CreateCategory(conservative);
            lab.CreateCategory(labour);

            List<string> a = new List<string>();

            foreach (string s in coal.uniqueWords)
            {
                a.Add(s);
            }
            foreach (string s in con.uniqueWords)
            {
                a.Add(s);
                for (int x = 0; x < (a.Count - 1); x++)
                {
                    if (s == a[x])
                    {
                        a.Remove(s);
                    }
                }

            }
            foreach (string s in lab.uniqueWords)
            {
                a.Add(s);
                for (int x = 0; x < (a.Count - 1); x++)
                {
                    if (s == a[x]) { a.Remove(s); }
                }

            }

            uniqueTotal = a.Count();

            coal.GetProbabilites(uniqueTotal);
            con.GetProbabilites(uniqueTotal);
            lab.GetProbabilites(uniqueTotal);
        }

        public void SaveToFile() //this method saves everything to text documents so it can be used for testing
        {

            string[] coalLines = coal.uniqueWords.ToArray();
            string[] conLines = con.uniqueWords.ToArray();
            string[] labLines = lab.uniqueWords.ToArray();
            string driveLetter = Path.GetPathRoot(Environment.CurrentDirectory);
            System.IO.File.WriteAllLines(driveLetter+"TrainingFiles\\CoalitionWords.txt", coalLines);
            System.IO.File.WriteAllLines(driveLetter+"TrainingFiles\\ConservativeWords.txt", conLines);
            System.IO.File.WriteAllLines(driveLetter+"TrainingFiles\\LabourWords.txt", labLines);

            List<string> coalP = new List<string>();
            foreach (double x in coal.probabilities)
            {
                coalP.Add(x.ToString());
            }
            List<string> conP = new List<string>();
            foreach (double x in con.probabilities)
            {
                conP.Add(x.ToString());
            }
            List<string> labP = new List<string>();
            foreach (double x in lab.probabilities)
            {
                labP.Add(x.ToString());
            }

            string[] coalProb = coalP.ToArray();
            string[] conProb = conP.ToArray();
            string[] labProb = labP.ToArray();


            System.IO.File.WriteAllLines(driveLetter+"TrainingFiles\\CoalitionProbabilities.txt", coalP);
            System.IO.File.WriteAllLines(driveLetter+"TrainingFiles\\ConservativeProbabilities.txt", conP);
            System.IO.File.WriteAllLines(driveLetter+"TrainingFiles\\LabourProbabilities.txt", labP);

            string[] uTotal = new string[1];
            uTotal[0] = (uniqueTotal.ToString());

            System.IO.File.WriteAllLines(driveLetter+"TrainingFiles\\UniqueTotal.txt", uTotal);

            string[] coalTotal = new string[1];
            coalTotal[0] = coal.totalWords.ToString();
            string[] conTotal = new string[1];
            conTotal[0] = con.totalWords.ToString();
            string[] labTotal = new string[1];
            labTotal[0] = lab.totalWords.ToString();

            System.IO.File.WriteAllLines(driveLetter+"TrainingFiles\\CoalitionTotal.txt", coalTotal);
            System.IO.File.WriteAllLines(driveLetter+"TrainingFiles\\ConservativeTotal.txt", conTotal);
            System.IO.File.WriteAllLines(driveLetter + "TrainingFiles\\LabourTotal.txt", labTotal);

        }
    }

    class Test
    {
        //initializes the attributes in Test
        List<string> words = new List<string>();
        List<double> chances = new List<double>();
        public string prediction;

        public void CreateTest(string t) //assigns words
        {
            //retrieves the data from text documents
            string driveLetter = Path.GetPathRoot(Environment.CurrentDirectory);
            String[] testDocOne = File.ReadAllText(driveLetter+"TrainingFiles\\test1.txt").Split(' ', ',', '.', '\r', '\n', ':', '/', '"', '\'');
            String[] testDocTwo = File.ReadAllText(driveLetter+"TrainingFiles\\test2.txt").Split(' ', ',', '.', '\r', '\n', ':', '/', '"', '\'');
            String[] testDocThree = File.ReadAllText(driveLetter+"TrainingFiles\\test3.txt").Split(' ', ',', '.', '\r', '\n', ':', '/', '"', '\'');
            String[] sWords = File.ReadAllText(driveLetter+"TrainingFiles\\stopwords.txt").Split('\r', '\n');

            List<string> testOne = new List<string>();
            List<string> testTwo = new List<string>();
            List<string> testThree = new List<string>();
            List<string> stopWords = new List<string>();

            void ListCreator(String[] doc, List<string> b)
            {
                foreach (string s in doc)
                {
                    string x = s.ToLower();
                    b.Add(x);
                    if (s == "") { b.Remove(x); }
                }
            }

            ListCreator(testDocOne, testOne);
            ListCreator(testDocTwo, testTwo);
            ListCreator(testDocThree, testThree);
            ListCreator(sWords, stopWords);

            void RemoveStopwords(List<string> a)
            {
                List<string> y = new List<string>();
                foreach (string s in a)
                {
                    y.Add(s);
                    foreach (string x in stopWords)
                    {
                        if (x == s) { y.Remove(x); }
                    }
                }

                a.Clear();
                foreach (string s in y)
                {
                    a.Add(s);
                }
            }

            RemoveStopwords(testOne);
            RemoveStopwords(testTwo);
            RemoveStopwords(testThree);

            if (t == "one") { words = testOne; }
            if (t == "two") { words = testTwo; }
            if (t == "three") { words = testThree; }

        }

        public void GetPrediction()
        {
            double priorProb = -0.47712125472;

            //gets data from text documents that was added to during training
            string driveLetter = Path.GetPathRoot(Environment.CurrentDirectory);
            String[] coalProb = File.ReadAllText(driveLetter+"TrainingFiles\\CoalitionProbabilities.txt").Split('\r', '\n');
            String[] conProb = File.ReadAllText(driveLetter+"TrainingFiles\\ConservativeProbabilities.txt").Split('\r', '\n');
            String[] labProb = File.ReadAllText(driveLetter+"TrainingFiles\\LabourProbabilities.txt").Split('\r', '\n');
            String[] uniqueTotal = File.ReadAllText(driveLetter+"TrainingFiles\\UniqueTotal.txt").Split('\r', '\n');
            String[] coalTotal = File.ReadAllText(driveLetter+"TrainingFiles\\CoalitionTotal.txt").Split('\r', '\n');
            String[] conTotal = File.ReadAllText(driveLetter+"TrainingFiles\\ConservativeTotal.txt").Split('\r', '\n');
            String[] labTotal = File.ReadAllText(driveLetter+"TrainingFiles\\LabourTotal.txt").Split('\r', '\n');
            String[] coalitionUniqueWords = File.ReadAllText(driveLetter+"TrainingFiles\\CoalitionWords.txt").Split('\r', '\n');
            String[] conservativeUniqueWords = File.ReadAllText(driveLetter+"TrainingFiles\\ConservativeWords.txt").Split('\r', '\n');
            String[] labourUniqueWords = File.ReadAllText(driveLetter+"TrainingFiles\\LabourWords.txt").Split('\r', '\n');

            List<string> coalP = new List<string>();
            List<string> conP = new List<string>();
            List<string> labP = new List<string>();
            List<string> uniT = new List<string>();
            List<string> coalT = new List<string>();
            List<string> conT = new List<string>();
            List<string> labT = new List<string>();
            List<string> coalUniqueWords = new List<string>();
            List<string> conUniqueWords = new List<string>();
            List<string> labUniqueWords = new List<string>();

            void ListCreator(String[] doc, List<string> b)
            {
                foreach (string s in doc)
                {
                    string x = s.ToLower();
                    b.Add(x);
                    if (s == "") { b.Remove(x); }
                }
            }

            ListCreator(coalProb, coalP);
            ListCreator(conProb, conP);
            ListCreator(labProb, labP);
            ListCreator(uniqueTotal, uniT);
            ListCreator(coalTotal, coalT);
            ListCreator(conTotal, conT);
            ListCreator(labTotal, labT);
            ListCreator(coalitionUniqueWords, coalUniqueWords);
            ListCreator(conservativeUniqueWords, conUniqueWords);
            ListCreator(labourUniqueWords, labUniqueWords);

            List<double> coalProbs = new List<double>();
            List<double> conProbs = new List<double>();
            List<double> labProbs = new List<double>();

            foreach (string s in coalP) { coalProbs.Add(Convert.ToDouble(s)); }
            foreach (string s in conP) { conProbs.Add(Convert.ToDouble(s)); }
            foreach (string s in labP) { labProbs.Add(Convert.ToDouble(s)); }

            double uTotal = Convert.ToDouble(uniT[0]);
            double clTotal = Convert.ToDouble(coalT[0]);
            double coTotal = Convert.ToDouble(conT[0]);
            double lTotal = Convert.ToDouble(labT[0]);

            //gets the predictions
            int i = 0;
            int v = 0;
            double coal = 0;
            double con = 0;
            double lab = 0;
            foreach (string s in words)
            {
                foreach (string x in coalUniqueWords)
                {
                    if (x == s)
                    {
                        coal = coal + Math.Log(coalProbs[i]);
                        v = 1;
                    }

                    i++;
                }
                foreach (string y in conUniqueWords)
                {
                    if (y == s && v == 0)
                    {
                        v = 2;
                    }
                }
                foreach (string z in labUniqueWords)
                {
                    if (z == s && v == 0) { v = 2; }
                }
                if (v == 2)
                {
                    coal = coal + Math.Log(1 / (clTotal + uTotal));
                }
                i = 0;
                v = 0;
            }
            coal = coal + priorProb;



            i = 0;
            v = 0;
            foreach (string s in words)
            {
                foreach (string x in conUniqueWords)
                {
                    if (x == s)
                    {
                        con = con + Math.Log(conProbs[i]);
                        v = 1;
                    }

                    i++;
                }
                foreach (string y in coalUniqueWords)
                {
                    if (y == s && v == 0) { v = 2; }
                }
                foreach (string z in labUniqueWords)
                {
                    if (z == s && v == 0) { v = 2; }
                }
                if (v == 2)
                {
                    con = con + Math.Log(1 / (coTotal + uTotal));
                }
                i = 0;
                v = 0;
            }
            con = con + priorProb;

            i = 0;
            v = 0;
            foreach (string s in words)
            {
                foreach (string x in labUniqueWords)
                {
                    if (x == s)
                    {
                        lab = lab + Math.Log(labProbs[i]);
                        v = 1;
                    }

                    i++;
                }
                foreach (string y in coalUniqueWords)
                {
                    if (y == s && v == 0) { v = 2; }
                }
                foreach (string z in conUniqueWords)
                {
                    if (z == s && v == 0) { v = 2; }
                }
                if (v == 2)
                {
                    lab = lab + Math.Log(1 / (lTotal + uTotal));
                }
                i = 0;
                v = 0;
            }

            lab = lab + priorProb;



            chances.Add(coal);
            chances.Add(con);
            chances.Add(lab);
            double solution = chances.Max();//compares the three numbers to determine the prediction.
            if (solution == coal) { prediction = "Coalition"; }
            if (solution == con) { prediction = "Conservative"; }
            if (solution == lab) { prediction = "Labour"; }

            Console.WriteLine("This speech belongs to a " + prediction + " government."); //outputs the prediction
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            int leave = 0;
        
            void MainMenu() //starts menu and displays menu options
            {
                Console.Clear();
                Console.WriteLine("**********************************************");
                Console.WriteLine("Please Enter your Option and press enter:");
                Console.WriteLine("Train and Test Model: 1");
                Console.WriteLine("Test Model: 2");
                Console.WriteLine("Exit: X");
                Console.WriteLine("**********************************************");
                string a = Console.ReadLine();
                if (a == "1")
                {
                    // trains and tests the model
                    Training model = new Training();
                    Test test = new Test();
                    model.CreateTraining();
                    model.SaveToFile();
                    Console.WriteLine("Please enter which test you would like to test the model with (one, two, three): ");
                    test.CreateTest(Console.ReadLine());
                    test.GetPrediction();
                    Console.ReadKey();
                }
                if (a == "2")
                {
                    //just tests the model
                    Test test = new Test();
                    Console.WriteLine("Please enter which test you would like to test the model with (one, two, three): ");
                    test.CreateTest(Console.ReadLine());
                    test.GetPrediction();
                    Console.ReadKey();
                }
                if (a == "x" || a == "X")
                {
                    Console.Clear();
                    leave = 2;
                }
                else
                {
                    Console.WriteLine("--------------------------------------------------");
                    Console.WriteLine("There was an Error please press enter");
                    Console.WriteLine("--------------------------------------------------");
                    Console.ReadKey();
                }
            }

            do { MainMenu(); } while (leave == 0);

        }
    }
}

