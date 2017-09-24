namespace LogR.Common.Interfaces.Service.Config
{
    public interface ISampleAppConfigFileCreator
    {
        void Generate();

        string GetConfigFileLocation();
    }
}
