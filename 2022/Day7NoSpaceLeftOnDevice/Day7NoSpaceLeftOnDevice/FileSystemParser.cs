namespace Day7NoSpaceLeftOnDevice;

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
                // nothing to do
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
            
    }
}