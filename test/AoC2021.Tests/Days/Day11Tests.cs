using System.IO;
using System.Reflection;
using Xunit;
using AoC2021.Days;

namespace AoC2021.Tests.Days
{
    public class Day11Tests
    {
        [Fact]
        public void Day11Test1()
        {
            // arrange
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            path = Path.GetFullPath(Path.Combine(path, "..", "..", ".."));

            var inputFile = Directory.GetFiles(path + @"/TestInput", "Day11_1.txt")[0];
            var day11 = new Day11(inputFile);

            // act
            var result1 = day11.PartOne();
            var result2 = day11.PartTwo();

            // assert
            Assert.Equal("1656", result1);
            Assert.Equal("195", result2);
        }
    }
}