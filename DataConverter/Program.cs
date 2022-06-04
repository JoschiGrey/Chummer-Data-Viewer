using System.IO.Compression;

var rootDirectory = Environment.CurrentDirectory;

//Yeah yeah a hacky way to not throw everything into the root folder
#if DEBUG
    rootDirectory = rootDirectory.Replace(@"\bin\Debug\net6.0", "");
#endif
#if RELEASE
    rootDirectory = rootDirectory.Replace(@"\bin\Release\net6.0", "");
#endif

Console.WriteLine(rootDirectory);

string inputBase = Path.Combine(rootDirectory, "Input");
if (!Directory.Exists(inputBase))
    throw new DirectoryNotFoundException(inputBase);

string outputBase = Path.Combine(rootDirectory, "Output");
if (!Directory.Exists(outputBase))
{
    Console.WriteLine("Needed to create: " + outputBase);
    Directory.CreateDirectory(outputBase);
}


var files=  Directory.GetFiles(inputBase);


foreach (var file in files)
{
    
    if (!File.Exists(file))
        throw new FileNotFoundException(file);
    using var input = File.OpenRead(file);
    

    var outputPath = Path.Join(outputBase, Path.GetFileName(file));
    outputPath = Path.ChangeExtension(outputPath, ".zz");
    using var output = File.Create(outputPath);

    using var compressor = new DeflateStream(output, CompressionMode.Compress);
    input.CopyTo(compressor);

    compressor.Close();
    output.Close();
}



