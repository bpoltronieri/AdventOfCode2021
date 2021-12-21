using System.IO;
using System.Reflection;
using Xunit;
using AoC2021.Days;

namespace AoC2021.Tests.Days
{
    public class Day21Tests
    {
        [Fact]
        public void Day21Test1()
        {
            // arrange
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            path = Path.GetFullPath(Path.Combine(path, "..", "..", ".."));

            var inputFile = Directory.GetFiles(path + @"/TestInput", "Day21_1.txt")[0];
            var day21 = new Day21(inputFile);

            // act
            var result1 = day21.PartOne();
            // part 2 currently is too slow to run as a test.
            // the input sample isn't in any way simpler than the real input anyway
            // var result2 = day21.PartTwo();

            // assert
            Assert.Equal("739785", result1);
            // Assert.Equal("444356092776315", result2);
        }
    }
}