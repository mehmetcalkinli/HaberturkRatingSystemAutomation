

SELECT * FROM Channels order by id



go


SELECT * FROM ChannelPeriods order by id

TRUNCATE TABLE ChannelPeriods


go

SELECT * FROM CheckInsert order by id
TRUNCATE TABLE CheckInsert

INSERT INTO CheckInsert (insertedDate,insertValue) VALUES ('20240212',1)
go

DELETE  FROM  TotalDay WHERE tableDate = '20240212' 



SELECT * FROM  TotalDay WHERE tableType = 'SHARE' ORDER BY tableDate

TRUNCATE TABLE TotalDay



SELECT *
FROM TotalDay
WHERE tableDate >= '2024-02-01' 
  AND tableDate <= '2024-02-19'
  AND tableType = 'SHARE'
  ORDER BY tableDate

  select * from totalday where tableDate ='20240523' and tableType ='RTG'
  SELECT insertedDate FROM CheckInsert WHERE insertedDate >= '20240201' AND insertedDate <= '20240229' AND insertValue=1




SELECT * FROM OutputTable WHERE tableType = 'RTG' AND tabledate='20240523' ORDER BY timebands

TRUNCATE TABLE OutputTable




Exec GetDataListRTG '20240523'
Exec GetDataListShare '20240212'

go
select * from checkInsert
DELETE FROM ChannelPeriods WHERE periodDate='20240707'

DELETE FROM CheckInsert WHERE insertedDate='20240707'

DELETE FROM OutputTable WHERE tableDate='20240707'

DELETE FROM totalday WHERE tableDate='20240707'

select * from ChannelPeriods WHERE periodDate='20240707'