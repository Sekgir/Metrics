﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.DAL.Interfaces
{
    public interface IConnectionManager
    {
        IDbConnection CreateOpenedConnection();
    }
}
