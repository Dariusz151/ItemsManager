using ItemsManager.Common.Types;
using ItemsManager.Recipes.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ItemsManager.Recipes.Commands
{
    public class Test : ICommand
    {
        //immutable
        public string Name { get; }

        [JsonConstructor]
        public Test(string name)
        {
            Name = name;
           
        }
    }
}
