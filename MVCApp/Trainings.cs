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
    
    public partial class Trainings
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Trainings()
        {
            this.CoachSquad = new HashSet<CoachSquad>();
            this.PlayerSquad = new HashSet<PlayerSquad>();
        }
    
        public int TrainingID { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<int> Duration { get; set; }
        public string TrainInfo { get; set; }
        public string Address { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CoachSquad> CoachSquad { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlayerSquad> PlayerSquad { get; set; }
    }
}