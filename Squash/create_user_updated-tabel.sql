START TRANSACTION;
DROP TABLE IF EXISTS users_updated;


CREATE TABLE users_updated (
Id int(10) unsigned NOT NULL AUTO_INCREMENT,
UserId int(10) unsigned NOT NULL,
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
PRIMARY KEY (Id),
INDEX `FK_to_users_idx` (`UserId` ASC),
CONSTRAINT `FK_to_users`
FOREIGN KEY (`UserId`)
  REFERENCES `users` (`UserId`)
  ON DELETE NO ACTION
  ON UPDATE NO ACTION
)ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

INSERT INTO users_updated (UserId, Firstname, Surname, Phone, EMail, StreetAddress, ZipCode, City, IPAddress, Password, Cellular, PublicAddress)
SELECT UserId, Firstname, Surname, Phone, EMail, StreetAddress, ZipCode, City, IPAddress, Password, Cellular, PublicAddress
FROM users;

DROP TABLE IF EXISTS news;
CREATE TABLE news (
Id int(10) unsigned NOT NULL AUTO_INCREMENT,
Headline varchar(250) NOT NULL DEFAULT '',
Newstext varchar(65000) NOT NULL DEFAULT '',
Imagepath varchar(250),
Imagebin mediumblob,
PRIMARY KEY (Id)

)ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

INSERT INTO news SET Headline ='Ny webbsida under konstruktion!', Newstext ='Den nya webbsidan utvecklas av två studenter på MIUN som B-Uppsats och är ett avslutningsprojekt för termin 4.', Imagepath='/Images/squashbild.png';
INSERT INTO news SET Headline ='Utökade funktioner, tillgänglig på alla enheter', Newstext ='Nya funktioner så som att kunna boka två banor samtidigt, avboka flera bokningar på samma gång m.m. är numera möjligt med den nya webbsidan. Den är dessutom responsivt utvecklad vilket betyder att webbsidan beter sig och ser olika ut beroende på vilken enhet som används men med samma funktionalitet på alla. Webbsidan möjliggör smidiga bokningar och enkel navigering på mobil.', Imagepath ='/Images/tennis-309617_960_720.png';




ALTER TABLE messages
ADD COLUMN Headline varchar(250),
MODIFY Messages varchar(1000);

ALTER TABLE reservationtypes
ADD COLUMN Cost int(10);

UPDATE reservationtypes SET Cost = 100 WHERE ReservationTypeId = 1;
UPDATE reservationtypes SET Cost = 0 WHERE ReservationTypeId = 2;
UPDATE reservationtypes SET Cost = 0 WHERE ReservationTypeId = 3;
UPDATE reservationtypes SET Cost = 50 WHERE ReservationTypeId = 4;

COMMIT;