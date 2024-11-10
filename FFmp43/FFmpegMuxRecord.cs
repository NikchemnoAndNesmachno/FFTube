namespace FFmp43
{
    public class FFmpegMuxRecord: FFmpegBaseRecord
    {
        public string AudioInput { get; set; } = "";

        public override string OutputExtension => Path.GetExtension(VideoInput);

        public override string Output => 
            Path.Combine(InputFolder, InputFileNameNoExtension + "_muxed." + OutputExtension);
        
        public override string GetArguments()
        {
            return $"-i \"{VideoInput}\" -i \"{AudioInput}\" -c:v {FFmpegCodecs.Mp4} -c:a {FFmpegCodecs.Libmp3} \"{Output}\" " +
                   $"-progress pipe:1";
        }
    }
}