using System.IO;
using System.Reflection;
using Xunit;
using AoC2021.Days;

namespace AoC2021.Tests.Days
{
    public class Day18Tests
    {
        [Fact]
        public void Day18Test1()
        {
            // arrange
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            path = Path.GetFullPath(Path.Combine(path, "..", "..", ".."));

            var inputFile = Directory.GetFiles(path + @"/TestInput", "Day18_1.txt")[0];
            var day18 = new Day18(inputFile);

            // act
            var result1 = day18.PartOne();
            var result2 = day18.PartTwo();

            // assert
            Assert.Equal("4140", result1);
            Assert.Equal("3993", result2);
        }
    }
}