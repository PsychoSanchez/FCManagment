using System;

namespace FootballClubLib.Entites
{
    class Players
    {
        public int PlayerID { get; set; }
        public int ContractNumber { get; set; }
        public int PlayerNumber { get; set; }
        public int Position { get; set; }
        public int AltPosition { get; set; }
        public DateTime Birthday { get; set; }
        public int Age { get; set; }
        public string Nationality { get; set; }
        public bool bNationalTeam { get; set; }
        public bool bInjured { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public bool bOnTransfer { get; set; }
        public double TransferCost { get; set; }
        public string PhoneNumber { get; set; }
        public string Adress { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }

    }
}
