CREATE TABLE Folders
(
  Owner varchar(30) NOT NULL,
  FolderID smallint IDENTITY,
  FolderName varchar(30) NOT NULL,
  FolderPath varchar(200) NOT NULL,
  FolderStatus varchar(20) NOT NULL,

  PRIMARY KEY(Owner,FolderID),
  FOREIGN KEY (Owner) REFERENCES Users(Username)
);