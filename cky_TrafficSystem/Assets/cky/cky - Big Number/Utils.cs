using System.Numerics;

namespace cky.BigNumber
{
    public static class Utils
    {
        public static string Convert4(this string number)
        {
            var result = number;
            var length = number.Length;

            if (length > 4 && length <= 7)
            {
                result = number.Remove(number.Length - 3) + "K";
            }
            else if (length > 7 && length <= 10)
            {
                result = number.Remove(number.Length - 6) + "M";
            }
            else if (length > 10 && length <= 13)
            {
                result = number.Remove(number.Length - 9) + "B";
            }
            else if (length > 13 && length <= 16)
            {
                result = number.Remove(number.Length - 12) + "AA";
            }
            else if (length > 16 && length <= 19)
            {
                result = number.Remove(number.Length - 15) + "BB";
            }
            else if (length > 19 && length <= 22)
            {
                result = number.Remove(number.Length - 18) + "CC";
            }
            else if (length > 22 && length <= 25)
            {
                result = number.Remove(number.Length - 21) + "DD";
            }
            else if (length > 25 && length <= 28)
            {
                result = number.Remove(number.Length - 24) + "EE";
            }
            else if (length > 28 && length <= 31)
            {
                result = number.Remove(number.Length - 27) + "FF";
            }


            return result;
        }

        public static string AddStringNumbers(string x, string y)
        {
            return (ToBigInteger(x) + ToBigInteger(y)).ToString();
        }

        public static string SubtractStringNumbers(string x, string y)
        {
            return (ToBigInteger(x) - ToBigInteger(y)).ToString();
        }

        public static string MultiplyStringNumbers(string x, string y)
        {
            return (ToBigInteger(x) * ToBigInteger(y)).ToString();
        }

        public static bool IsBiggerOrEqual(string x, string y)
        {
            if (ToBigInteger(x) >= ToBigInteger(y))
                return true;

            return false;
        }


        public static BigInteger ToBigInteger(string value) // Faster than BigInteger.Parse(value) ***
        {
            BigInteger result = 0;

            for (int i = 0; i < value.Length; i++)
            {
                result = result * 10 + (value[i] - '0');
            }

            return result;
        }
    }
}