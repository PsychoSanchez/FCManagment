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
    
    public partial class PlayerSquad
    {
        public int PlayerSquadID { get; set; }
        public Nullable<int> PlayerID { get; set; }
        public Nullable<int> MatchID { get; set; }
        public Nullable<int> TrainingID { get; set; }
        public Nullable<int> Goals { get; set; }
        public Nullable<int> Assists { get; set; }
        public Nullable<int> Fouls { get; set; }
        public Nullable<int> YellowCards { get; set; }
        public Nullable<bool> RedCards { get; set; }
        public Nullable<int> TimeOnField { get; set; }
        public Nullable<int> StartMinute { get; set; }
        public Nullable<decimal> Distance { get; set; }
        public Nullable<bool> MVP { get; set; }
        public string CoachNote { get; set; }
    
        public virtual Matches Matches { get; set; }
        public virtual Players Players { get; set; }
        public virtual Trainings Trainings { get; set; }
    }
}
