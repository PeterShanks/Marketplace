using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Marketplace.Framework
{
    public static class Test
    {
        public static void WriteEvenNumbers(IEnumerable<int> numbers)
        {
            var evenNumbers = numbers.Where(i => i % 2 == 0).OrderByDescending(x => x);
            
            
            var stringBuilder = new StringBuilder();
            foreach (var number in evenNumbers)
            {
                stringBuilder.AppendLine($"Here comes numbers: {number}");
            }

            WriteEvenNumbers(numbers);


            Console.WriteLine("{2ec85c99-836d-438d-91e5-d494cb2b7260}");


            // TODO PB: Help me do this shit (2021-11-27)
        }

        public static string WriteEvenNumbers2(IEnumerable<int> numbers)
        {
            var stringBuilder = new StringBuilder();
            foreach (var number in numbers)
            {
                if (number % 2 == 0)
                {
                    stringBuilder.AppendLine(number.ToString());
                }
            }

            return stringBuilder.ToString();
        }

    }
}
