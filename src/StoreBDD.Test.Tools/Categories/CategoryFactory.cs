﻿using StoreBDD.Entities;
using StoreBDD.Services.Categories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreBDD.Test.Tools.Categories
{
    public class CategoryFactory
    {
        public static AddCategoryDto GenerateAddCategoryDto(string title)
        {
            return new AddCategoryDto
            {
                Title = title
            };
        }
        public static Category GenerateCategory(string title)
        {
            return new Category
            {
                Title = title
            };
        }

    }
}
