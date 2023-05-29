﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NVOS.Core
{
    public interface IDisposableService : IService
    {
        bool Dispose();
    }
}