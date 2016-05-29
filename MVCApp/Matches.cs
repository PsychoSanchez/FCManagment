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
    
    public partial class Matches
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Matches()
        {
            this.CoachSquad = new HashSet<CoachSquad>();
            this.PlayerSquad = new HashSet<PlayerSquad>();
        }
    
        public int MatchID { get; set; }
        public Nullable<int> TournamentID { get; set; }
        public Nullable<int> StadiumID { get; set; }
        public Nullable<bool> Home { get; set; }
        public string Result { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CoachSquad> CoachSquad { get; set; }
        public virtual Stadiums Stadiums { get; set; }
        public virtual Tournaments Tournaments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlayerSquad> PlayerSquad { get; set; }
    }
}
