﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tutorial.Infrastructure.Services.Interfaces
{
    public interface ITransientService
    {
        Guid GetID();
    }
}