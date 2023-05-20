using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }
        [StringLength(50)]
        public string CategoryName { get; set; }
        [StringLength(200)]
        public string CategoryDescription { get; set; }
        //Bool olduğu için extra bir kısıtlamaya gerek yok
        public bool CategoryStatus { get; set; } //True or false

        public ICollection<Heading> Headings { get; set; } //1'e çok ilişki
    }
}
