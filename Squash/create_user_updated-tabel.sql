START TRANSACTION;
DROP TABLE IF EXISTS users_updated;


CREATE TABLE users_updated (
Id int(10) unsigned NOT NULL AUTO_INCREMENT,
UserId int(10) NOT NULL,
Firstname varchar(45) NOT NULL DEFAULT '',
Surname varchar(45) NOT NULL DEFAULT '',
Phone varchar(45) NOT NULL DEFAULT '',
EMail varchar(255),
StreetAddress varchar(45) NOT NULL DEFAULT '',
ZipCode varchar(45) NOT NULL DEFAULT '',
City varchar(45) NOT NULL DEFAULT '',
IPAddress varchar(45) NOT NULL DEFAULT '',
Password varchar(45) NOT NULL DEFAULT '',
Cellular varchar(45) DEFAULT '',
PublicAddress tinyint(3) unsigned NOT NULL DEFAULT '1',
PRIMARY KEY (Id)
)ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

INSERT INTO users_updated (UserId, Firstname, Surname, Phone, EMail, StreetAddress, ZipCode, City, IPAddress, Password, Cellular, PublicAddress)
SELECT UserId, Firstname, Surname, Phone, EMail, StreetAddress, ZipCode, City, IPAddress, Password, Cellular, PublicAddress
FROM users;

COMMIT;