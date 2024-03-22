//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RealEstateProject.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class LandDemands
    {
        public int Id { get; set; }
        public Nullable<int> CityId { get; set; }
        public Nullable<int> StreetId { get; set; }
        public Nullable<int> AddressHouse { get; set; }
        public Nullable<int> AddressNumber { get; set; }
        public Nullable<int> MinPrice { get; set; }
        public Nullable<int> MaxPrice { get; set; }
        public Nullable<int> AgentID { get; set; }
        public Nullable<int> ClientId { get; set; }
        public Nullable<int> MinArea { get; set; }
        public Nullable<int> MaxArea { get; set; }
    
        public virtual Agents Agents { get; set; }
        public virtual Cities Cities { get; set; }
        public virtual Clients Clients { get; set; }
        public virtual Streets Streets { get; set; }
    }
}
