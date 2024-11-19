namespace FFTube;

public class FFmpegMuxRecord
{
    private string _outputPath = "";
    private readonly string _fileSuffix;
    public FFmpegMuxRecord()
    {
        _fileSuffix = "_mxd" + DateTime.Now.ToString("HH_mm_ss");
    }
    
    private static void SplitAndSet(string path, out string name, out string extension, char separator)
    {
        var index = path.LastIndexOf(separator);
        name = path[..index];
        extension = path[index..];
    }

    public bool ToSaveAudio { get; set; } = false;
    public bool ToSaveOldVideo { get; set; } = false;
    public string VideoInput { get; set; } = "";
    public string AudioInput { get; set; } = "";

    public string CustomVideoName { get; set; } = "";

    public void Clean()
    {
        if (!ToSaveAudio)
        {
            File.Delete(AudioInput);
        }

        if (!ToSaveOldVideo)
        {
            File.Delete(VideoInput);
            File.Move(_outputPath, VideoInput);
        }
        
    }
    public string Build()
    {
        var audioCodecCommand = "copy";
        SplitAndSet(VideoInput, out var videoPath, out var videoExtension, '.');
        SplitAndSet(AudioInput, out _, out var audioExtension, '.');
        if (videoExtension == FFmpegExtensions.Webm && audioExtension == FFmpegExtensions.M4a)
        {
            audioCodecCommand = "libopus";
        }

        if (!string.IsNullOrEmpty(CustomVideoName))
        {
            SplitAndSet(videoPath, out var directory, out _, Path.DirectorySeparatorChar);
            _outputPath = Path.Combine(directory, CustomVideoName) + _fileSuffix + videoExtension;
        }
        else
        {
            _outputPath = videoPath + _fileSuffix + videoExtension;
        }
        return $"-i \"{VideoInput}\" -i \"{AudioInput}\" -c:v copy -c:a {audioCodecCommand} \"{_outputPath}\" " +
               $"-progress pipe:1 -y";
    }
}