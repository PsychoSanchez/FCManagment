//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MVCApp
{
    using System;
    using System.Collections.Generic;
    
    public partial class PlayerPositions
    {
        public int PlayerPositionID { get; set; }
        public Nullable<int> PlayerID { get; set; }
        public Nullable<int> PositionID { get; set; }
    
        public virtual Players Players { get; set; }
        public virtual Positions Positions { get; set; }
    }
}
