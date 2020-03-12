using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CoreFlow.Shared
{
    public interface IAddonRepository
    {
        void RegisterAddOn(System.IO.Stream stream);
        Assembly Resolve(String fullname);
    }
}
