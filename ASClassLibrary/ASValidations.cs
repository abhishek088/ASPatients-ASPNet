using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace ASClassLibrary
{
    public static class ASValidations
    {
        public static bool ASPostalCodeValidation(string postalCode)
        {
            Regex postalCodeRegex = new Regex(@"^[ABCEGHJKLMNPRSTVXYabceghjklmnprstvxy]\d[ABCEGHJKLMNPRSTVWXYZabceghjklmnprstvxy] ?\d[ABCEGHJKLMNPRSTVWXYZabceghjklmnprstvxy]\d$");
            if (postalCode == null || postalCode == "" || postalCodeRegex.IsMatch(postalCode))
            {
                return true;
            }
            return false;
        }

        public static string ASExtractDigits(string word)
        {
            if (word == null)
            {
                return "";
            }
            else
            {
                string newWord = String.Empty;
                for (int i = 0; i < word.Length; i++)
                {
                    if (Char.IsDigit(word[i]))
                        newWord += word[i];
                }
                return newWord;
            }
        }

        public static string ASCapitalize(string word)
        {
            if (word == null)
            {
                return "";
            }
            else
            {
                word = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(word.ToLower());
                return word.Trim();
            }
        }

        public static bool ASZipCodeValidation(string zip)
        {
            if (zip == null)
            {
                zip = "";
                return true;
            }
            else if (zip.Length == 5)
            {
                zip = ASExtractDigits(zip);
                return true;
            }
            else if (zip.Length == 9)
            {
                zip = ASExtractDigits(zip);
                var newZip = String.Empty;
                for (int i = 0; i < zip.Length; i++)
                {
                    if (i == 5)
                        newZip += "-";

                    newZip += zip[i];
                }
                zip = newZip;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
