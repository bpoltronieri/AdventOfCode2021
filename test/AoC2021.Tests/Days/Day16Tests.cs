using System.IO;
using System.Reflection;
using Xunit;
using AoC2021.Days;

namespace AoC2021.Tests.Days
{
    public class Day16Tests
    {
        [Fact]
        public void Day16Test1()
        {
            // arrange
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            path = Path.GetFullPath(Path.Combine(path, "..", "..", ".."));

            var inputFile = Directory.GetFiles(path + @"/TestInput", "Day16_1.txt")[0];
            var day16 = new Day16(inputFile);

            // act
            var result1 = day16.PartOne();

            // assert
            Assert.Equal("31", result1);
        }

        [Fact]
        public void Day16Test2()
        {
            // arrange
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            path = Path.GetFullPath(Path.Combine(path, "..", "..", ".."));

            var inputFile = Directory.GetFiles(path + @"/TestInput", "Day16_2.txt")[0];
            var day16 = new Day16(inputFile);

            // act
            var result2 = day16.PartTwo();

            // assert
            Assert.Equal("1", result2);
        }
    }
}