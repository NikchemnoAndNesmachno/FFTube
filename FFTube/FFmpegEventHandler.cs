using System.Diagnostics;

namespace FFTube;

public class FFmpegEventHandler
{
    private bool _durationIsFound;
    private bool _isFinished;
    private ulong _totalSize;
    public string TotalTime { get; set; } = "";
    public string CurrentTime { get; set; } = "";
    public event Action<double>? Progress;
    private static ulong GetMilliseconds(string[] time)
    {
        var secs = time[2].Split(".");
        var msecs = secs[1].Remove(secs[1].Length - 1, 1);
        msecs += "000";
        var milliSeconds = ulong.Parse(msecs[..3]);
        var size = ulong.Parse(time[0]) * 3600000 +
                   ulong.Parse(time[1]) * 60000 +
                   ulong.Parse(secs[0]) * 1000 +
                   milliSeconds;
        return size;
    }
    public void OnError(object sender, DataReceivedEventArgs e)
    {
        Console.WriteLine(e.Data);
        if (_durationIsFound) return;
        var row = e.Data?.Split(Array.Empty<char>(), 
            StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (row == null) return;
        if (row[0] != "Duration:") return;
        TotalTime = row[1];            
        var time = row[1].Split(":");
        _totalSize = GetMilliseconds(time);
        _durationIsFound = true;
    }

    public void OnOutput(object sender, DataReceivedEventArgs e)
    {
        if(_isFinished) return;
        var row = e.Data?.Split("=");
        if(row is null || row.Length < 2) return;
        if (row[0] == "progress" && row[1] == "end")
        {
            Progress?.Invoke(1);
            _isFinished = true;
            return;
        }
        if (row[0] == "out_time")
        {
            CurrentTime = row[1];
            return;
        }

        if (row[0] != "out_time_ms") return;
        var currentSize = ulong.Parse(row[1])/1000;
        var progress = (double)currentSize / _totalSize;
        if (progress >= 1)
        {
            _isFinished = true;
        }
        Progress?.Invoke(progress);

    }
}