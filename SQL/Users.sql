create table Users
(
  Username varchar(30) NOT NULL PRIMARY KEY,
  Firstname varchar(30) NOT NULL,
  Lastname varchar(30),
  Email varchar(50) NOT NULL,
  Cellphone varchar(10),
  userPassword varchar(30) NOT NULL 
);

create table UsersPictures
(
  Username varchar(30) NOT NULL PRIMARY KEY,
  ProfilePicture Image
)
