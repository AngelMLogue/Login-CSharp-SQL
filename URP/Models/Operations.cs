//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace URP.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Operations
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Operations()
        {
            this.RolOperations = new HashSet<RolOperations>();
            this.RolOperations1 = new HashSet<RolOperations>();
        }
    
        public int OID { get; set; }
        public string OName { get; set; }
        public Nullable<int> MID { get; set; }
    
        public virtual Modules Modules { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RolOperations> RolOperations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RolOperations> RolOperations1 { get; set; }
    }
}