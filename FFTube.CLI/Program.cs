using FFTube;
namespace FFmp43.CLI;

class Program
{

    public static void MuxSample()
    {
        var record = new FFmpegMuxRecord()
        {
            VideoInput = "/home/yesman/Desktop/ffmpegs/Swingrowers - Pump Up the Jam (Electro Swing Cover ft. The Lost Fingers) - BBC Strictly_720.webm",
            AudioInput =  "/home/yesman/Desktop/ffmpegs/ia_128.m4a",
            ToSaveAudio = true,
            ToSaveOldVideo = false
        };
        var handler = new FFmpegEventHandler();
        handler.Progress += d => Console.WriteLine($"\tProgress: {d}"); 
        FFmpegRunner.Run(record, handler);
    }
    static void Main(string[] args)
    {
        FFmpegRunner.FFmpegPath = "/home/yesman/Desktop/ffmpegs/ffmpeg_static";
        MuxSample();
    }
}