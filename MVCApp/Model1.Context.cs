﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class FClubEntities : DbContext
    {
        public FClubEntities()
            : base("name=FClubEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Agents> Agents { get; set; }
        public virtual DbSet<Coachs> Coachs { get; set; }
        public virtual DbSet<CoachSquad> CoachSquad { get; set; }
        public virtual DbSet<Contracts> Contracts { get; set; }
        public virtual DbSet<ContractTypes> ContractTypes { get; set; }
        public virtual DbSet<Mans> Mans { get; set; }
        public virtual DbSet<Matches> Matches { get; set; }
        public virtual DbSet<Nationalities> Nationalities { get; set; }
        public virtual DbSet<PersosnalPosition> PersosnalPosition { get; set; }
        public virtual DbSet<PlayerPositions> PlayerPositions { get; set; }
        public virtual DbSet<Players> Players { get; set; }
        public virtual DbSet<PlayerSquad> PlayerSquad { get; set; }
        public virtual DbSet<Positions> Positions { get; set; }
        public virtual DbSet<Stadiums> Stadiums { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Tournaments> Tournaments { get; set; }
        public virtual DbSet<Trainings> Trainings { get; set; }
        public virtual DbSet<CoachInfo> CoachInfo { get; set; }
        public virtual DbSet<PlayerInfo> PlayerInfo { get; set; }
        public virtual DbSet<Users> Users { get; set; }
    
        public virtual ObjectResult<GetPlayerStatistics_Result> GetPlayerStatistics(Nullable<int> playerID)
        {
            var playerIDParameter = playerID.HasValue ?
                new ObjectParameter("PlayerID", playerID) :
                new ObjectParameter("PlayerID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetPlayerStatistics_Result>("GetPlayerStatistics", playerIDParameter);
        }
    
        public virtual ObjectResult<GetPlayerStat_Result1> GetPlayerStat(Nullable<int> playerID)
        {
            var playerIDParameter = playerID.HasValue ?
                new ObjectParameter("PlayerID", playerID) :
                new ObjectParameter("PlayerID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetPlayerStat_Result1>("GetPlayerStat", playerIDParameter);
        }
    }
}
