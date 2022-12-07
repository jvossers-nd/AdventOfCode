namespace Day7NoSpaceLeftOnDevice;

public class ElfFileSystem
{
    public ElfDirectory Root { get; set; }
    public List<ElfDirectory> AllDirectories => Root.DirectoriesResursive.ToList();
}