using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogR.Common.Interfaces.Service.Config
{
    public interface ISampleAppConfigFileCreator
    {
        void Generate();
        string GetConfigFileLocation();
    }
}
