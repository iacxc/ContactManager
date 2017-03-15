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
            var command = new SQLiteCommand(sqlstr, m_db);

            command.Parameters.Add(new SQLiteParameter("@Id", contact.Id.ToString()));
            command.Parameters.Add(new SQLiteParameter("@FirstName", contact.FirstName));
            command.Parameters.Add(new SQLiteParameter("@LastName", contact.LastName));
            command.Parameters.Add(new SQLiteParameter("@ImagePath", contact.ImagePath));
            command.Parameters.Add(new SQLiteParameter("@Organization", contact.Organization));
            command.Parameters.Add(new SQLiteParameter("@JobTitle", contact.JobTitle));
            command.Parameters.Add(new SQLiteParameter("@CellPhone", contact.CellPhone));
            command.Parameters.Add(new SQLiteParameter("@HomePhone", contact.HomePhone));
            command.Parameters.Add(new SQLiteParameter("@OfficePhone", contact.OfficePhone));
            command.Parameters.Add(new SQLiteParameter("@PrimaryEmail", contact.PrimaryEmail));
            command.Parameters.Add(new SQLiteParameter("@SecondaryEmail", contact.SecondaryEmail));
            command.Parameters.Add(new SQLiteParameter("@City", address.City));
            command.Parameters.Add(new SQLiteParameter("@Country", address.Country));
            command.Parameters.Add(new SQLiteParameter("@Line1", address.Line1));
            command.Parameters.Add(new SQLiteParameter("@Line2", address.Line2));
            command.Parameters.Add(new SQLiteParameter("@State", address.State));
            command.Parameters.Add(new SQLiteParameter("@Zip", address.Zip));

            command.ExecuteNonQuery();
            m_db.Close();
        }

        public void Delete(Contact contact)
        {
            m_contactStore.Remove(contact);

            var sqlstr = string.Format("DELETE FROM T_Contacts WHERE id=@Id");
            m_db.Open();
            var command = new SQLiteCommand(sqlstr, m_db);
            command.Parameters.Add(new SQLiteParameter("@Id", contact.Id.ToString()));
            command.ExecuteNonQuery();
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
            var command = new SQLiteCommand(qrystr, m_db);
            var reader = command.ExecuteReader();

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
            m_db.Close();
        }
    }
}
