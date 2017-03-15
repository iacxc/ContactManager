drop table if exists T_Contacts;

create table T_Contacts (
        id Text Primary Key,
        FirstName Text,
        LastName Text,
        ImagePath Text,
        Organization Text,
        JobTitle Text,
        CellPhone Text,
        HomePhone Text,
        OfficePhone Text,
        PrimaryEmail Text,
        SecondaryEmail Text, 
	    City Text,
        Country Text,
        Line1 Text,
        Line2 Text,
        State Text,
        Zip Text
);


