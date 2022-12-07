using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Globalization;
using FluentAssertions;
using System.IO;
using System.Net.Http.Headers;
using Xunit;

namespace Day7NoSpaceLeftOnDevice
{
    public class Files
    {
        public const string InputFile = "input.txt";
        public const string InputFileTest = "input.test.txt";
    }

    public class Tests
    {
        [Theory]
        [InlineData(Files.InputFile)]
        [InlineData(Files.InputFileTest)]
        public void ShouldReadFile(string fileName)
        {
            File.ReadAllLines(fileName).Length.Should().BeGreaterThan(0);
        }

        [Fact]
        public void ShouldParseFileSystem()
        {
            var lines = File.ReadAllLines(Files.InputFileTest);

            var sut = new FileSystemParser();

            var fileSystem = sut.Parse(lines);

            fileSystem.Root.Directories["a"].Directories["e"].Files["i"].Size.Should().Be(584);
            fileSystem.Root.Directories["a"].Files["f"].Size.Should().Be(29116);
            fileSystem.Root.Directories["a"].Files["g"].Size.Should().Be(2557);
            fileSystem.Root.Directories["a"].Files["h.lst"].Size.Should().Be(62596);

            fileSystem.Root.Files["b.txt"].Size.Should().Be(14848514);

            fileSystem.Root.Files["c.dat"].Size.Should().Be(8504156);

            fileSystem.Root.Directories["d"].Files["j"].Size.Should().Be(4060174);
            fileSystem.Root.Directories["d"].Files["d.log"].Size.Should().Be(8033020);
            fileSystem.Root.Directories["d"].Files["d.ext"].Size.Should().Be(5626152);
            fileSystem.Root.Directories["d"].Files["k"].Size.Should().Be(7214296);
        }

        // https://adventofcode.com/2022/day/7
        // build filestructure hierarchy
        // calc direct folder size
        // add recursion to include child folder sizes in calculation
        // filter where dir.size < 100000
        // sum of the above list of filtered list
    }

    public class FileSystemParser
    {
        public ElfFileSystem Parse(string[] lines)
        {
            var fs =  new ElfFileSystem
            {
                Root = new ElfDirectory("/", new List<ElfNode>()
                {
                    new ElfDirectory("a", new List<ElfNode>() 
                    {
                        new ElfDirectory( "e",  new List<ElfNode>()
                        {
                            new ElfFile("i", 584)
                        }),
                        new ElfFile("f", 29116),
                        new ElfFile("g", 2557),
                        new ElfFile("h.lst", 62596)
                    }),
                    new ElfFile("b.txt", 14848514),
                    new ElfFile("c.dat", 8504156),


                    new ElfDirectory("d", new List<ElfNode>() 
                    {
                        new ElfFile("j", 4060174),
                        new ElfFile("d.log", 8033020),
                        new ElfFile("d.ext", 5626152),
                        new ElfFile("k", 7214296)
                    }),
                })
            };

            return fs;
        }
    }

    public class ElfFileSystem
    {
        public ElfDirectory Root { get; set; }
    }

    public abstract class ElfNode
    {
        public string Name { get; set; }
    }

    public class ElfDirectory : ElfNode
    {
        public List<ElfNode> ChildNodes { get; set; }
        public IImmutableDictionary<string, ElfDirectory> Directories => ChildNodes.OfType<ElfDirectory>().ToImmutableDictionary(n => n.Name, n => n);
        public IImmutableDictionary<string, ElfFile> Files => ChildNodes.OfType<ElfFile>().ToImmutableDictionary(n => n.Name, n => n);

        public ElfDirectory(string name, List<ElfNode>? childNodes = null)
        {
            Name = name;
            ChildNodes = childNodes ?? new List<ElfNode>();
        }
    }

    public class ElfFile: ElfNode
    {
        public int Size { get; set; }

        public ElfFile(string name, int size)
        {
            Name = name;
            Size = size;
        }
    }
}