using System.IO;
using System.Reflection;
using Xunit;
using AoC2021.Days;

namespace AoC2022.Tests.Days
{
    public class Day22Tests
    {
        [Fact]
        public void Day22Test1()
        {
            // arrange
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            path = Path.GetFullPath(Path.Combine(path, "..", "..", ".."));

            var inputFile = Directory.GetFiles(path + @"/TestInput", "Day22_1.txt")[0];
            var day22 = new Day22(inputFile);

            // act
            var result1 = day22.PartOne();
            // var result2 = day22.PartTwo();

            // assert
            Assert.Equal("590784", result1);
            // Assert.Equal("0", result2);
        }

        [Fact]
        public void Day22Test2()
        {
            // arrange
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            path = Path.GetFullPath(Path.Combine(path, "..", "..", ".."));

            var inputFile = Directory.GetFiles(path + @"/TestInput", "Day22_2.txt")[0];
            var day22 = new Day22(inputFile);

            // act
            // var result1 = day22.PartOne();
            var result2 = day22.PartTwo();

            // assert
            // Assert.Equal("0", result1);
            Assert.Equal("2758514936282235", result2);
        }
    }
}