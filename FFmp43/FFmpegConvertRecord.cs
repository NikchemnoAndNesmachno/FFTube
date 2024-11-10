namespace FFmp43
{
    public class FFmpegConvertRecord: FFmpegBaseRecord
    {
        public override string Output => Path.Combine(InputFolder, InputFileNameNoExtension + "." + OutputExtension);

        public override string GetArguments()
        {
            return $"-i \"{VideoInput}\" -acodec {FFmpegCodecs.Libmp3} -vn \"{Output}\" " +
                   $"-progress pipe:1";
        }
    }
}