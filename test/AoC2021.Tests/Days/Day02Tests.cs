using System.IO;
using System.Reflection;
using Xunit;
using AoC2021.Days;

namespace AoC2021.Tests.Days
{
    public class Day02Tests
    {
        [Fact]
        public void Day02Test1()
        {
            // arrange
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            path = Path.GetFullPath(Path.Combine(path, "..", "..", ".."));

            var inputFile = Directory.GetFiles(path + @"/TestInput", "Day02_1.txt")[0];
            var day02 = new Day02(inputFile);

            // act
            var result1 = day02.PartOne();
            var result2 = day02.PartTwo();

            // assert
            Assert.Equal("0", result1);
            Assert.Equal("0", result2);
        }
    }
}