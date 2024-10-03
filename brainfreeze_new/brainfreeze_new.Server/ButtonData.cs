namespace brainfreeze_new.Server
{
    public class ButtonData 
    {
        public int ButtonId { get; set; }
        public string? Timestamp { get; set; }
        public List<int> number { get; set; } = new List<int>();
        public int level { get; set; } = 4;
    }

}
