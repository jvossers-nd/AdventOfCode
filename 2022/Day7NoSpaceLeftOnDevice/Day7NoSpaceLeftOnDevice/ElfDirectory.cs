using System.Collections.Immutable;

namespace Day7NoSpaceLeftOnDevice;

public class ElfDirectory : ElfNode
{
    public string Path { get; set; }
    public List<ElfNode> ChildNodes { get; set; }
    public List<ElfDirectory> DirectoriesResursive => ChildNodes.OfType<ElfDirectory>().SelectMany(d => d.DirectoriesResursive).Union(new List<ElfDirectory>() { this }).ToList();

    public IImmutableDictionary<string, ElfDirectory> Directories => ChildNodes.OfType<ElfDirectory>().ToImmutableDictionary(n => n.Name, n => n);
    public IImmutableDictionary<string, ElfFile> Files => ChildNodes.OfType<ElfFile>().ToImmutableDictionary(n => n.Name, n => n);
    public ElfDirectory? ParentDirectory { get; set; }

    public override int Size => ChildNodes.Sum(c => c.Size);

    public ElfDirectory(string name, ElfDirectory parentDirectory, List<ElfNode>? childNodes = null)
    {
        Name = name;
        ParentDirectory = parentDirectory;
        ChildNodes = childNodes ?? new List<ElfNode>();
        Path = parentDirectory == null
            ? Path = name
            : (parentDirectory?.Path + "/" + name).Replace(@"//", "/");
    }
}