using System.Diagnostics;


namespace FFmp43;

public static class FFmpegRunner
{
    private static string _ffmpegPath = "";

    public static string FFmpegPath
    {
        get => string.IsNullOrEmpty(_ffmpegPath) ? 
               Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), DefaultFolderName), DefaultFFmpegName) :
               _ffmpegPath;
        set => _ffmpegPath = value;
    }
        
    private static string DefaultFolderName => "ffmpeg";
    private static string DefaultFFmpegName => "ffmpeg.exe";
        
    
    public static void Run(FFmpegBaseRecord record, FFmpegEventHandler handler)
    {
        record.RefreshFile();
        var  process = new Process();
        process.StartInfo.FileName = FFmpegPath;
        process.StartInfo.Arguments = record.GetArguments();
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.OutputDataReceived += handler.OnOutput;
        process.ErrorDataReceived += handler.OnError;
        
        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        process.WaitForExit();
        process.CancelErrorRead();
        process.CancelOutputRead();
        process.OutputDataReceived -= handler.OnOutput;
        process.ErrorDataReceived -= handler.OnError;
        process.Close();
    }
}