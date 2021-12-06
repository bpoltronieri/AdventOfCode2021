using System.IO;
using System.Reflection;
using Xunit;
using AoC2021.Days;

namespace AoC2021.Tests.Days
{
    public class Day07Tests
    {
        [Fact]
        public void Day07Test1()
        {
            // arrange
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            path = Path.GetFullPath(Path.Combine(path, "..", "..", ".."));

            var inputFile = Directory.GetFiles(path + @"/TestInput", "Day07_1.txt")[0];
            var day07 = new Day07(inputFile);

            // act
            var result1 = day07.PartOne();
            var result2 = day07.PartTwo();

            // assert
            Assert.Equal("0", result1);
            Assert.Equal("0", result2);
        }
    }
}