//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace E_shop.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PaymentSystem
    {
        public int ID { get; set; }
        public Nullable<int> Nominal { get; set; }
        public string KeyCode { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string UserName { get; set; }
    }
}
