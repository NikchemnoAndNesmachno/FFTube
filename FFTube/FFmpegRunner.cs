using System.Diagnostics;


namespace FFTube;

public static class FFmpegRunner
{
    public static string FFmpegPath { get; set; } = "";

    public static bool Exists() => File.Exists(FFmpegPath);

    public static void Run(FFmpegMuxRecord record, FFmpegEventHandler handler)
    {
        if (!Exists())
            throw new MissingMemberException("No ffmpeg found");
        var  process = new Process();
        process.StartInfo.FileName = FFmpegPath;
        process.StartInfo.Arguments = record.Build();
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
        record.Clean();
    }
}