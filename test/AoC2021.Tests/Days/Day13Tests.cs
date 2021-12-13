using System.IO;
using System.Reflection;
using Xunit;
using AoC2021.Days;

namespace AoC2021.Tests.Days
{
    public class Day13Tests
    {
        [Fact]
        public void Day13Test1()
        {
            // arrange
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            path = Path.GetFullPath(Path.Combine(path, "..", "..", ".."));

            var inputFile = Directory.GetFiles(path + @"/TestInput", "Day13_1.txt")[0];
            var day13 = new Day13(inputFile);

            // act
            var result1 = day13.PartOne();
            var result2 = day13.PartTwo();

            // assert
            Assert.Equal("17", result1);
            Assert.Equal("16", result2);
        }
    }
}