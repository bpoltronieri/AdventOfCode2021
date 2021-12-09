using System.IO;
using System.Reflection;
using Xunit;
using AoC2021.Days;

namespace AoC2021.Tests.Days
{
    public class Day10Tests
    {
        [Fact]
        public void Day10Test1()
        {
            // arrange
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            path = Path.GetFullPath(Path.Combine(path, "..", "..", ".."));

            var inputFile = Directory.GetFiles(path + @"/TestInput", "Day10_1.txt")[0];
            var day10 = new Day10(inputFile);

            // act
            var result1 = day10.PartOne();
            var result2 = day10.PartTwo();

            // assert
            Assert.Equal("0", result1);
            Assert.Equal("0", result2);
        }
    }
}