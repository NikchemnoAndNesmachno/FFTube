namespace FFmp43
{
    public abstract class FFmpegBaseRecord
    {
        public string VideoInput { get; set; } = "";
        public virtual string Output { get; set; } = "";
        protected string InputFolder => Path.GetDirectoryName(VideoInput) ?? "";
        protected string InputFileNameNoExtension => Path.GetFileNameWithoutExtension(VideoInput);
        public virtual string OutputExtension { get; set; } = "";
        public void RefreshFile()
        {
            var outputPath = Output;
            if (!File.Exists(outputPath)) return;
            File.Delete(outputPath);
        }

        public abstract string GetArguments();
    }
}