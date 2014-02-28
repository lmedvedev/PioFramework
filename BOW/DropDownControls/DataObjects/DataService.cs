using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeaHousePeople.Samples.DataObjects
{
    public class DataService
    {
        public List<Author> GetAuthors()
        {
            List<Author> authors = new List<Author>();
            string[] names = new string[] { "John Doe", "Dohn Joe", "Carl Hiaasen", "Agatha Christie", "Elmore Leonard" };

            for (int i = 0; i < names.Length; i++)
            {
                authors.Add(new Author(i, names[i]));
            }
            return authors;
        }
    }
}
