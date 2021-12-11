using System.IO;
using System.Reflection;
using Xunit;
using AoC2021.Days;

namespace AoC2021.Tests.Days
{
    public class Day12Tests
    {
        [Fact]
        public void Day12Test1()
        {
            // arrange
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            path = Path.GetFullPath(Path.Combine(path, "..", "..", ".."));

            var inputFile = Directory.GetFiles(path + @"/TestInput", "Day12_1.txt")[0];
            var day12 = new Day12(inputFile);

            // act
            var result1 = day12.PartOne();
            var result2 = day12.PartTwo();

            // assert
            Assert.Equal("0", result1);
            Assert.Equal("0", result2);
        }
    }
}