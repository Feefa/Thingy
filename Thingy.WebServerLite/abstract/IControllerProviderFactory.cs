﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thingy.WebServerLite.Api;

namespace Thingy.WebServerLite
{
    public interface IControllerProviderFactory
    {
        IControllerProvider Create(IController[] controllers);
    }
}