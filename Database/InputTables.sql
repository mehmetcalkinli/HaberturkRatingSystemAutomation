

USE RatingSystem

go


CREATE TABLE Channels 
(
id int identity(1,1) not null,
channelName nvarchar(60),
unique(channelName),

CONSTRAINT PK_Channels PRIMARY KEY (id),
INDEX ind1_channels (channelName)
)



go
select*from CheckInsert
CREATE TABLE CheckInsert
(
id int identity(1,1) not null,

insertedDate date,

insertValue int

)
INSERT INTO CheckInsert (insertedDate,insertValue) VALUES ('20240201',1)
--DELETE FROM CheckInsert WHERE insertedDate='20240205'
go
CREATE TABLE ChannelPeriods
(
id int identity(1,1),
channelid int,
periodDate date,
Timebands varchar(20),


RtgIndividuals5Plus decimal(15,9),	
RtgMale20Plus decimal(15,9),
RtgFemale20Plus decimal(15,9),
RtgIndividualsSESAB decimal(15,9),
RtgIndividuals20PlusABC1 decimal(15,9),
RtgIndividuals511 decimal(15,9),

ShareIndividuals5Plus decimal(15,9),	
ShareMale20Plus decimal(15,9),
ShareFemale20Plus decimal(15,9),
ShareIndividualsSESAB decimal(15,9),
ShareIndividuals20PlusABC1 decimal(15,9),
ShareIndividuals511 decimal(15,9),

Rtg0Individuals5Plus decimal(15,9),	
Rtg0Male20Plus decimal(15,9),
Rtg0Female20Plus decimal(15,9),
Rtg0IndividualsSESAB decimal(15,9),
Rtg0Individuals20PlusABC1 decimal(15,9),
Rtg0Individuals511 decimal(15,9),


CONSTRAINT PK_ChannelPeriods PRIMARY KEY (id),
INDEX ind1_ChannelPeriods (channelid, periodDate, Timebands)

)



go




SELECT * FROM Channels order by id

--TRUNCATE TABLE Channels



go


SELECT * FROM ChannelPeriods order by id

--TRUNCATE TABLE ChannelPeriods


go


