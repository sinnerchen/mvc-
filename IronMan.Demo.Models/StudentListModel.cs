using System.Collections.Generic;

namespace IronMan.Demo.Models
{
    public class StudentListModel
    {
        public string Name { get; set; }
        public int PageNum { get; set; }

        public int PageSize { get; set; }

        public int RecordCount { get; set; }

        public List<Entities.Student> List { get; set; }

    }
}
