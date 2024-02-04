using MineSweeper.Common;
using MineSweeper.Constant;

namespace MineSweeper.Test
{
    public class GameTest
    {
        [Fact]
        public void ValidateInputeWithInvalidValue()
        {
            var result = ValidateInput.Parseinput("A");

            Assert.Null(result);
        }

        [Fact]
        public void ValidateInputeWithvalidValue()
        {
            var result = ValidateInput.Parseinput("4");

            Assert.Equal(4, result);
        }

        [Fact]
        public void ValidateInValidMinGridSize()
        {
            var result = ValidateInput.IsValidMinValue(1, GameConstatnt.MinGridSize);

            Assert.False(result);
        }

        [Fact]
        public void ValidateValidMinGridSize()
        {
            var result = ValidateInput.IsValidMinValue(4, GameConstatnt.MinGridSize);

            Assert.True(result);
        }

        [Fact]
        public void ValidateInValidMaxGridSize()
        {
            var result = ValidateInput.IsValidMaxValue(11, GameConstatnt.MaxGridSize);

            Assert.False(result);
        }

        [Fact]
        public void ValidateValidMaxGridSize()
        {
            var result = ValidateInput.IsValidMaxValue(8, GameConstatnt.MaxGridSize);

            Assert.True(result);
        }

        [Fact]
        public void ValidateInValidMinMines()
        {
            var result = ValidateInput.IsValidMinValue(0, GameConstatnt.MinMines);

            Assert.False(result);
        }

        [Fact]
        public void ValidateInValidMaxMines()
        {
            int gridSize = 4;

            var result = ValidateInput.IsValidMaxValue(5, gridSize);

            Assert.False(result);
        }
    }
}