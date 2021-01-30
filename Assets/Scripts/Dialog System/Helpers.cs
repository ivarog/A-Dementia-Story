using UnityEngine;

public static class Helpers
{
    public static string InvertWords(string line)
    {
        string newLine = "";

        string[] words = line.Split(' ');
        
        for(int i = 0; i < words.Length; i++)
        {
            newLine = words[i] + " " + newLine;
        }

        return newLine;
    }
}