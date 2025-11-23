namespace FactorioServer.Models
{
    public class FactorioGameTimeData
    {
        public uint Tick { get; set; }
        public uint TicksPlayed { get; set; }
        public bool Paused { get; set; }
        public float Speed { get; set; }
    }
}