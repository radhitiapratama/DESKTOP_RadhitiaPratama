//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DESKTOP_RadhitiaPratama
{
    using System;
    using System.Collections.Generic;
    
    public partial class ReservationDetail
    {
        public int ID { get; set; }
        public int ReservationID { get; set; }
        public int MenuID { get; set; }
        public int Qty { get; set; }
    
        public virtual Menu Menu { get; set; }
        public virtual Reservation Reservation { get; set; }
    }
}
