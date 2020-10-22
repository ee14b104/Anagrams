//using System.Media;
//using System.Runtime.Hosting;
using UnityEngine;
using Figgle;
using System.Timers;
using System;
using System.Diagnostics;
using System.Collections;

public class Hacker : MonoBehaviour
{
    enum  Screen { MainMenu, Play, Win};
    Screen State = Screen.MainMenu;
    string currentWord = "";
    System.Random random = new System.Random();
    int word_score = 0;
    double time_score = 0;
    int total = 0;
    int totalWords = 0;
    string shuffledWord = "";
    
    Stopwatch stopWatch = new Stopwatch();
    Coroutine turnOverRoutine = null;
    Coroutine hint1Routine = null;
    

    //StopCoroutine(lastRoutine);
    IEnumerator timeUpCoroutine()
    {
        yield return new WaitForSeconds(20f);// Wait for one second
        Terminal.ClearScreen();
        Terminal.WriteLine("You ran out of time :(");
        Terminal.WriteLine("The correct word was " + currentWord);
        GenerateWord();
    }
    IEnumerator hint1Coroutine()
    {
        yield return new WaitForSeconds(10f);// Wait for one second

        int currentWord_len = currentWord.Length;
        System.Random rnd = new System.Random();
        int ind1 = rnd.Next(0, currentWord_len);  // creates a number between 1 and 12
        int ind2 = rnd.Next(0, currentWord_len);   // creates a number between 1 and 6
        string hint = string.Concat(System.Linq.Enumerable.Repeat("_", currentWord_len));
        System.Text.StringBuilder hint_builder = new System.Text.StringBuilder(hint);
        hint_builder[ind1] = currentWord.ToUpper()[ind1];
        hint_builder[ind2] = currentWord.ToUpper()[ind2];
        hint = hint_builder.ToString();
        hint = hint.Replace("_", "_\u202F");
        Terminal.WriteLine("Here is a hint to guess " + shuffledWord.ToUpper() + ": " + hint);
    }

    void ShowMainMenu(bool printScore)
    {
        Terminal.ClearScreen();
        if (printScore)
        {
            Terminal.WriteLine("You got a total score of " + time_score);
            Terminal.WriteLine("You got " + word_score + " out of " + totalWords + " words correct");
            

        }
        Terminal.WriteLine("Let's play Anagrams!!");
        Terminal.WriteLine("Type 'start' to get started.....");
        //Reset Score and total words
        word_score = 0;
        totalWords = 0;
        
    }
    string[] lines;
    // Start is called before the first frame update
    void Start()
    {
        ShowMainMenu(false);
        TextAsset asset = Resources.Load<TextAsset>("google-10000-english-usa-no-swears-medium");

        string s = asset.ToString();
        lines = s.Split('\n');
        
    }
    
    void OnUserInput(string input)
    {
        if (input.ToLower().Equals("menu") | input == "0")
        {
            bool printScore = false;
            if(totalWords > 0)
            {
                printScore = true;
            }
            StopCoroutine(hint1Routine);
            StopCoroutine(turnOverRoutine);
            ShowMainMenu(printScore);
            
        }
        else if(State == Screen.MainMenu)
        {
            MainMenu(input);
        }
        else if(State == Screen.Play)
        {
            Play(input);
        }
        
    }
    void GenerateWord()
    {
        totalWords = totalWords + 1;
        currentWord = lines[random.Next(0, lines.Length)];
        shuffledWord = (currentWord.Anagram());
        Terminal.WriteLine("Please guess " + shuffledWord.ToUpper() + " (0 to exit, 1 to skip)");
        stopWatch.Stop();
        stopWatch.Reset();
        stopWatch.Start();
        hint1Routine = StartCoroutine(hint1Coroutine());
        turnOverRoutine = StartCoroutine(timeUpCoroutine());


    }
    void MainMenu(string input)
    {
        if (input.ToLower() == "start")
        {
            State = Screen.Play;
            GenerateWord();
        }
        else
        {
            Terminal.WriteLine("Please enter a valid option");
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="input"></param>
    void Play(string input)
    {
        if(input == "1")
        {
            StopCoroutine(hint1Routine);
            StopCoroutine(turnOverRoutine);
            Terminal.ClearScreen();
            Terminal.WriteLine("The correct answer was " + currentWord + ". Better luck for the next one...");
            GenerateWord();
            


        }
        else if (input.ToLower().Equals(currentWord.ToLower()))
        {
            
            word_score = word_score + 1;
            stopWatch.Stop();
            StopCoroutine(hint1Routine);
            StopCoroutine(turnOverRoutine);
            time_score = time_score + 20 - stopWatch.ElapsedMilliseconds / 1000;
            stopWatch.Reset();
            Terminal.ClearScreen();
            Terminal.WriteLine("You got that correct !!");

            /*if (word_score%10 == 0)
            {
                Terminal.WriteLine(FiggleFonts.Standard.Render(word_score.ToString()));
            }
            if (word_score % 100 == 0)
            {
                Terminal.WriteLine(FiggleFonts.Standard.Render("NICE!"));
            }*/
            Terminal.WriteLine("Your score is "+ Math.Round(time_score,2)+ "!");
            
            GenerateWord();
        }
        else
        {
            
            Terminal.WriteLine("That is not correct.");
            Terminal.WriteLine("Please guess "+ shuffledWord.ToUpper() + " again " + "(0 to exit, 1 to skip)" );
        }
    }


    
    void generate_hint(object sender, ElapsedEventArgs elapsedEventArg)
    {
        int currentWord_len = currentWord.Length;
        System.Random rnd = new System.Random();
        int ind1 = rnd.Next(0, currentWord_len);  // creates a number between 1 and 12
        int ind2 = rnd.Next(0, currentWord_len);   // creates a number between 1 and 6
        string hint = string.Concat(System.Linq.Enumerable.Repeat("_", currentWord_len)); 
        System.Text.StringBuilder hint_builder = new System.Text.StringBuilder(hint);
        hint_builder[ind1] = currentWord.ToUpper()[ind1];
        hint_builder[ind2] = currentWord.ToUpper()[ind2];
        hint = hint_builder.ToString();
        hint = hint.Replace("_", "_\u202F");
        Terminal.WriteLine("Here is a hint to guess "+ shuffledWord.ToUpper() + ": "+ hint);
        
    }

}
