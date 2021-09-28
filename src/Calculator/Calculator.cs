using System;

namespace Math
{
    public class Calculator
    {
        public int Sum(int[] values)
        {
            int result = 0;

            foreach (var number in values)
            {
                result += number;
            }

            return result;
        }

        public int Subtract(int[] values)
        {
            int result = 0;

            foreach (var number in values)
            {
                result -= number;
            }

            return result;
        }

        public int Multiply(int[] values)
        {
            int result = 1;

            foreach (var number in values)
            {
                result *= number;
            }

            return result;
        }

        public decimal Divide(int v1, int v2)
        {
            return v1 / v2;
        }
    }
}
