using System;
using System.Collections.Generic;
using System.Linq;

namespace Calculator.Application
{
    public interface ICalculator
    {
        int Add(string input);
    }

    public class Calculator : ICalculator
    {
        List<int> Numbers = new List<int>();
        List<string> delimiters = new List<string> { ",", "\n" };

        public int Add(string input)
        {
            var negativeNumbers = new List<int>();
            var splitStringdels = new List<string> { "[", "][", "]" };
            if (string.IsNullOrEmpty(input))
            {
                return 0;
            }
            if (!input.Contains(",") && !input.Contains("\n"))
            {
                return Convert.ToInt32(input);
            }
            if (input.StartsWith("//["))
            {
                if (input.Contains("]["))
                {
                    var dells = input.Substring(2, input.IndexOf("]\n", StringComparison.Ordinal) - 1);//                    
                    string[] multiDels = dells.Split(splitStringdels.ToArray(), StringSplitOptions.None);
                    foreach (var handleDels in multiDels)
                    {
                        delimiters.Add(handleDels);
                    }
                }
                var delimeter = input.Substring(3, input.IndexOf("]\n", StringComparison.Ordinal) - 3);
                delimiters.Add(delimeter);
                input = input.Substring(input.IndexOf("]\n", StringComparison.Ordinal) + 2);
            }
            else if (input.StartsWith("//"))
            {
                string delimeter = input.Substring(2, input.IndexOf("\n", StringComparison.Ordinal) - 2);
                delimiters.Add(delimeter);
                input = input.Substring(input.IndexOf('\n')+1);
            }
            string[] numersArray = input.Split(delimiters.ToArray(), StringSplitOptions.None);
            foreach (var init in numersArray.Select(int.Parse))
            {
                if (init<0)
                {
                    negativeNumbers.Add(init);
                }
                else if (init<=1000)
                {
                    Numbers.Add(init);
                }
                else
                {
                    Numbers.Add(0);
                }
            }
            if (negativeNumbers.Any())
            {
                const string errormsg = "Negative numbers not allowed: {0}";
                throw new NegativeNumberException(string.Format(errormsg,string.Join(",",negativeNumbers)));
            }
            return Numbers.Sum();
        }
    }

    public class NegativeNumberException : Exception
    {
        public NegativeNumberException(string error )
        :base(error){
        }
    }
}