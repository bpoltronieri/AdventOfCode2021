using System.IO;
using System.Reflection;
using Xunit;
using AoC2021.Days;

namespace AoC2021.Tests.Days
{
    public class Day17Tests
    {
        [Fact]
        public void Day17Test1()
        {
            // arrange
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            path = Path.GetFullPath(Path.Combine(path, "..", "..", ".."));

            var inputFile = Directory.GetFiles(path + @"/TestInput", "Day17_1.txt")[0];
            var day17 = new Day17(inputFile);

            // act
            var result1 = day17.PartOne();
            var result2 = day17.PartTwo();

            // assert
            Assert.Equal("45", result1);
            Assert.Equal("112", result2);
        }
    }
}