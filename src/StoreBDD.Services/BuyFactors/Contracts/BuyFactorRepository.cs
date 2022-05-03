﻿using StoreBDD.Entities;
using StoreBDD.Infrastructure.Application;
using StoreBDD.Services.SellFactors.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreBDD.Services.BuyFactors.Contracts
{
    public interface BuyFactorRepository : Repository
    {
        void Add(BuyFactor buyFactor);
        List<GetBuyFactorDto> GetAll();
    }
}
