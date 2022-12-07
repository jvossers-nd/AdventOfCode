using System.Collections.Immutable;
using System.Reflection.Metadata.Ecma335;
using FluentAssertions;
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
        public void SolutionPart1()
        {
            var lines = File.ReadAllLines(Files.InputFile);

            var sut = new FileSystemParser();

            var fileSystem = sut.Parse(lines);

            var directories = fileSystem.AllDirectories.Where(d => d.Size <= 100000).ToList();

            var solution = directories.Sum(d => d.Size);

            Console.WriteLine(solution);
        }

        [Fact]
        public void ShouldParseFileSystem()
        {
            var lines = File.ReadAllLines(Files.InputFileTest);

            var sut = new FileSystemParser();

            var fileSystem = sut.Parse(lines);
            
            // file sizes
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

            // dir sizes
            fileSystem.Root.Directories["a"].Directories["e"].Size.Should().Be(584);
            fileSystem.Root.Directories["a"].Size.Should().Be(94853);
            fileSystem.Root.Directories["d"].Size.Should().Be(24933642);
            fileSystem.Root.Size.Should().Be(48381165);

            fileSystem.AllDirectories.Count.Should().Be(4);
            fileSystem.AllDirectories.SingleOrDefault(d => d.Name == "/").Should().NotBeNull();
            fileSystem.AllDirectories.SingleOrDefault(d => d.Name == "a").Should().NotBeNull();
            fileSystem.AllDirectories.SingleOrDefault(d => d.Name == "e").Should().NotBeNull();
            fileSystem.AllDirectories.SingleOrDefault(d => d.Name == "d").Should().NotBeNull();

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
            var fs = new ElfFileSystem()
            {
                Root = new ElfDirectory("/", null)
            };

            ElfDirectory currentDirectory = null;

            foreach (var line in lines)
            {
                if (line.StartsWith("$ cd"))
                {
                    if (line == "$ cd /")
                    {
                        currentDirectory = fs.Root;
                    }
                    else if (line == "$ cd ..")
                    {
                        currentDirectory = currentDirectory.ParentDirectory;
                    }
                    else
                    {
                        var dirName = line.Replace("$ cd ", string.Empty);
                        currentDirectory = currentDirectory.Directories[dirName];
                    }
                }
                else if (line.StartsWith("$ ls"))
                {
                }
                else if (line.StartsWith("dir"))
                {
                    string dirName = line.Replace("dir ", string.Empty);
                    if (!currentDirectory.Directories.Keys.Contains(dirName))
                    {
                        var newDir = new ElfDirectory(dirName, currentDirectory);
                        currentDirectory.ChildNodes.Add(newDir);
                    }
                }
                else
                {
                    var parts = line.Split(' ');
                    var fileName = parts[1];
                    int fileSize = int.Parse(parts[0]);

                    if (!currentDirectory.Files.Keys.Contains(fileName))
                    {
                        var newFile = new ElfFile(fileName, fileSize);
                        currentDirectory.ChildNodes.Add(newFile);
                    }
                }
            }

            return fs;

            //var fs =  new ElfFileSystem
            //{
            //    Root = new ElfDirectory("/", new List<ElfNode>()
            //    {
            //        new ElfDirectory("a", new List<ElfNode>() 
            //        {
            //            new ElfDirectory( "e",  new List<ElfNode>()
            //            {
            //                new ElfFile("i", 584)
            //            }),
            //            new ElfFile("f", 29116),
            //            new ElfFile("g", 2557),
            //            new ElfFile("h.lst", 62596)
            //        }),
            //        new ElfFile("b.txt", 14848514),
            //        new ElfFile("c.dat", 8504156),


            //        new ElfDirectory("d", new List<ElfNode>() 
            //        {
            //            new ElfFile("j", 4060174),
            //            new ElfFile("d.log", 8033020),
            //            new ElfFile("d.ext", 5626152),
            //            new ElfFile("k", 7214296)
            //        }),
            //    })
            //};

            return fs;
        }
    }

    public class ElfFileSystem
    {
        public ElfDirectory Root { get; set; }
        public List<ElfDirectory> AllDirectories => Root.DirectoriesResursive;

    }

    public abstract class ElfNode
    {
        public virtual int Size { get; set; }
        
        public string Name { get; set; }
    }

    public class ElfDirectory : ElfNode
    {
        public List<ElfNode> ChildNodes { get; set; }
        public List<ElfDirectory> DirectoriesResursive => ChildNodes.OfType<ElfDirectory>().SelectMany(d => d.DirectoriesResursive).Union(new List<ElfDirectory>() { this }).ToList();
        public IImmutableDictionary<string, ElfDirectory> Directories => ChildNodes.OfType<ElfDirectory>().ToImmutableDictionary(n => n.Name, n => n);
        public IImmutableDictionary<string, ElfFile> Files => ChildNodes.OfType<ElfFile>().ToImmutableDictionary(n => n.Name, n => n);
        public ElfDirectory? ParentDirectory { get; set; }

        public override int Size
        {
            get => ChildNodes.Sum(c => c.Size);
        }

        public ElfDirectory(string name, ElfDirectory parentDirectory, List<ElfNode>? childNodes = null)
        {
            Name = name;
            ParentDirectory = parentDirectory;
            ChildNodes = childNodes ?? new List<ElfNode>();
        }
    }

    public class ElfFile: ElfNode
    {
        public ElfFile(string name, int size)
        {
            Name = name;
            Size = size;
        }
    }
}