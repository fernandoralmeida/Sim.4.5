namespace Sim.Data
{
    public class Factory
    {
        public static IData Connecting(string connectingstring)
        {
            return new Data(connectingstring);
        }
    }
}
