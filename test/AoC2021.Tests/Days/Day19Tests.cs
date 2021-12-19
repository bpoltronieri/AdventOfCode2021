using System.IO;
using System.Reflection;
using Xunit;
using AoC2021.Days;

namespace AoC2021.Tests.Days
{
    public class Day19Tests
    {
        [Fact]
        public void Day19Test1()
        {
            // arrange
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            path = Path.GetFullPath(Path.Combine(path, "..", "..", ".."));

            var inputFile = Directory.GetFiles(path + @"/TestInput", "Day19_1.txt")[0];
            var day19 = new Day19(inputFile);

            // act
            var result1 = day19.PartOne();
            var result2 = day19.PartTwo();

            // assert
            Assert.Equal("79", result1);
            Assert.Equal("3621", result2);
        }
    }
}