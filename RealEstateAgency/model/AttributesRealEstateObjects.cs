//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RealEstateAgency.model
{
    using System;
    using System.Collections.Generic;
    
    public partial class AttributesRealEstateObjects
    {
        public int Id { get; set; }
        public int IdObject { get; set; }
        public int IdAttributesName { get; set; }
        public double Value { get; set; }
    
        public virtual AttributesName AttributesName { get; set; }
        public virtual RealEstateObjects RealEstateObjects { get; set; }
    }
}