﻿using StoreBDD.Entities;
using StoreBDD.Infrastructure.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreBDD.Services.Categories.Contracts
{
    public interface CategoryRepository : Repository
    {
        void Add(Category category);
        bool CheckTitle(string title);
    }
}