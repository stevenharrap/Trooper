//--------------------------------------------------------------------------------------
// <copyright file="StoreItem.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.Ui.Mvc.Cruncher
{
    using Trooper.Ui.Interface.Mvc.Cruncher;

    /// <summary>
    /// Holds a reference to a file or actual content 
    /// </summary>
    public class StoreItem : IStoreItem
    {        
        public string File { get; set; }
                
        public string Name { get; set; }
                
        public string Content { get; set; }
                
        public bool Less { get; set; }
                
        public OrderOptions Order { get; set; }
                
        public ReferenceOptions Reference { get; set; }        
    }
}
