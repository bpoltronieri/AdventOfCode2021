using System.IO;
using System.Reflection;
using Xunit;
using AoC2021.Days;

namespace AoC2021.Tests.Days
{
    public class Day23Tests
    {
        [Fact]
        public void Day23Test1()
        {
            // arrange
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            path = Path.GetFullPath(Path.Combine(path, "..", "..", ".."));

            var inputFile = Directory.GetFiles(path + @"/TestInput", "Day23_1.txt")[0];
            var day23 = new Day23(inputFile);

            // act
            var result1 = day23.PartOne();
            var result2 = day23.PartTwo();

            // assert
            Assert.Equal("12521", result1);
            Assert.Equal("44169", result2);
        }
    }
}