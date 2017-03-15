using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;

namespace ContactManager.Model
{
    public class ContactRepository
    {
        private List<Contact> m_contactStore;
        private readonly SQLiteConnection m_db;

        public ContactRepository()
        {
            var dbpath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "ContactManager.db3");
            m_db = new SQLiteConnection("Data Source=" + dbpath);

            Deserialize();
        }

        public void Save(Contact contact)
        {
            if (contact.Id == Guid.Empty)
                contact.Id = Guid.NewGuid();

            string sqlstr;
            var address = contact.Address;
            if (!m_contactStore.Contains(contact))
            {
                m_contactStore.Add(contact);

                sqlstr =
@"INSERT INTO T_Contacts(
      Id, FirstName, LastName, ImagePath, Organization, JobTitle
    , CellPhone, HomePhone, OfficePhone, PrimaryEmail, SecondaryEmail
    , City, Country, Line1, Line2, State, Zip) 
VALUES(@Id, @FirstName, @LastName, @ImagePath, @Organization, @JobTitle
    , @CellPhone, @HomePhone, @OfficePhone, @PrimaryEmail, @SecondaryEmail
    , @City, @Country, @Line1, @Line2, @State, @Zip)";
            }
            else
            {
                sqlstr =
@"UPDATE T_Contacts SET 
  FirstName=@FirstName, LastName=@LastName, ImagePath=@ImagePath 
, Organization=@Organization , JobTitle=@JobTitle, CellPhone=@CellPhone
, HomePhone=@HomePhone, OfficePhone=@OfficePhone
, PrimaryEmail=@PrimaryEmail, SecondaryEmail=@SecondaryEmail
, City=@City, Country=@Country, Line1=@Line1
, Line2=@Line2, State=@State, Zip=@Zip
WHERE Id=@Id";
            }

            m_db.Open();
            using (var cmd = m_db.CreateCommand())
            {
                cmd.CommandText = sqlstr;

                cmd.Parameters.AddWithValue("@Id", contact.Id.ToString());
                cmd.Parameters.AddWithValue("@FirstName", contact.FirstName);
                cmd.Parameters.AddWithValue("@LastName", contact.LastName);
                cmd.Parameters.AddWithValue("@ImagePath", contact.ImagePath);
                cmd.Parameters.AddWithValue("@Organization", contact.Organization);
                cmd.Parameters.AddWithValue("@JobTitle", contact.JobTitle);
                cmd.Parameters.AddWithValue("@CellPhone", contact.CellPhone);
                cmd.Parameters.AddWithValue("@HomePhone", contact.HomePhone);
                cmd.Parameters.AddWithValue("@OfficePhone", contact.OfficePhone);
                cmd.Parameters.AddWithValue("@PrimaryEmail", contact.PrimaryEmail);
                cmd.Parameters.AddWithValue("@SecondaryEmail", contact.SecondaryEmail);
                cmd.Parameters.AddWithValue("@City", address.City);
                cmd.Parameters.AddWithValue("@Country", address.Country);
                cmd.Parameters.AddWithValue("@Line1", address.Line1);
                cmd.Parameters.AddWithValue("@Line2", address.Line2);
                cmd.Parameters.AddWithValue("@State", address.State);
                cmd.Parameters.AddWithValue("@Zip", address.Zip);

                cmd.ExecuteNonQuery();
            }
            m_db.Close();
        }

        public void Delete(Contact contact)
        {
            m_contactStore.Remove(contact);

            m_db.Open();
            using (var cmd = m_db.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM T_Contacts WHERE id=@Id";
                cmd.Parameters.AddWithValue("@Id", contact.Id.ToString());
                cmd.ExecuteNonQuery();
            }
            m_db.Close();
        }

        public List<Contact> FindByLookup(string lookupName)
        {
            var found = from c in m_contactStore
                where c.LookupName.StartsWith(
                    lookupName,
                    StringComparison.OrdinalIgnoreCase)
                select c;

            return found.ToList();
        }

        public List<Contact> FindAll()
        {
            return new List<Contact>(m_contactStore);
        }

        private void Deserialize()
        {
            var qrystr = 
@"SELECT id, FirstName, LastName, ImagePath , Organization, JobTitle
        , CellPhone, HomePhone, OfficePhone, PrimaryEmail, SecondaryEmail
        , City, Country, Line1 , Line2, State, Zip
  FROM T_Contacts";
            m_db.Open();
            using (var cmd = m_db.CreateCommand())
            {
                cmd.CommandText = qrystr;
                var reader = cmd.ExecuteReader();

                m_contactStore = new List<Contact>();
                while (reader.Read())
                {
                    m_contactStore.Add(new Contact()
                    {
                        Id = new Guid(reader["id"].ToString()),
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        ImagePath = reader["ImagePath"].ToString(),
                        Organization = reader["Organization"].ToString(),
                        JobTitle = reader["JobTitle"].ToString(),
                        CellPhone = reader["CellPhone"].ToString(),
                        HomePhone = reader["HomePhone"].ToString(),
                        OfficePhone = reader["OfficePhone"].ToString(),
                        PrimaryEmail = reader["PrimaryEmail"].ToString(),
                        SecondaryEmail = reader["SecondaryEmail"].ToString(),
                        Address = new Address()
                        {
                            City = reader["City"].ToString(),
                            Country = reader["Country"].ToString(),
                            Line1 = reader["Line1"].ToString(),
                            Line2 = reader["Line2"].ToString(),
                            State = reader["State"].ToString(),
                            Zip = reader["Zip"].ToString()
                        }
                    });
                }
            }
            m_db.Close();
        }
    }
}
