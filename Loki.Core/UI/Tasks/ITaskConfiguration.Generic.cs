﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loki.UI.Tasks
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ITaskConfiguration<TArg> : ITaskConfiguration
    {
        void Start(TArg args);
    }
}