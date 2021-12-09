using System.IO;
using System.Reflection;
using Xunit;
using AoC2021.Days;

namespace AoC2021.Tests.Days
{
    public class Day09Tests
    {
        [Fact]
        public void Day09Test1()
        {
            // arrange
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            path = Path.GetFullPath(Path.Combine(path, "..", "..", ".."));

            var inputFile = Directory.GetFiles(path + @"/TestInput", "Day09_1.txt")[0];
            var day09 = new Day09(inputFile);

            // act
            var result1 = day09.PartOne();
            var result2 = day09.PartTwo();

            // assert
            Assert.Equal("15", result1);
            Assert.Equal("1134", result2);
        }
    }
}