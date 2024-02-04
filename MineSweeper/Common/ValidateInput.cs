
namespace MineSweeper.Common
{
    public static class ValidateInput
    {
        public static int? Parseinput(string input)
        {
            int output;

            if (int.TryParse(input, out output))
            {
                return output;
            }

            return null;

        }

        public static bool IsValidMinValue(int input, int minValue)
        {
            return input > minValue;
        }

        public static bool IsValidMaxValue(int input, int maxValue)
        {
            return input < maxValue;
        }
    }
}
