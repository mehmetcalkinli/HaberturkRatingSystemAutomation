++USE RatingSystem

go


CREATE TABLE OutputTable (

id int identity(1,1) not null,

tableDate date,

tableType nvarchar(10),
timebands nvarchar(20),

programNTV nvarchar(100),
programHABERTURK nvarchar(100),
programCNNTURK nvarchar(100),

TotalNTV decimal(15,9),
TotalHABERTURK decimal(15,9),	
TotalCNNTURK decimal(15,9),	
TotalHALKTV decimal(15,9),	
TotalKRT	 decimal(15,9),	
TotalAHABER	 decimal(15,9),	
TotalTRTHABER	 decimal(15,9),	
TotalTELE1	 decimal(15,9),	
TotalTV100	 decimal(15,9),	
TotalHABERGLOBAL decimal(15,9),	
TotalSOZCUTV decimal(15,9),	

ABSESNTV decimal(15,9),
ABSESHABERTURK decimal(15,9),	
ABSESCNNTURK decimal(15,9),	
ABSESHALKTV decimal(15,9),	
ABSESKRT	 decimal(15,9),	
ABSESAHABER	 decimal(15,9),	
ABSESTRTHABER	 decimal(15,9),	
ABSESTELE1	 decimal(15,9),	
ABSESTV100	 decimal(15,9),	
ABSESHABERGLOBAL decimal(15,9),	
ABSESSOZCUTV decimal(15,9),	



CONSTRAINT PK_RTG PRIMARY KEY (id),
INDEX ind1_OT (timebands,tableType,tableDate)
)







CREATE TABLE TotalDay
(
id int identity(1,1) not null,

tableDate date,
tableType varchar(10),


TotalNTV decimal(15,9),
TotalHABERTURK decimal(15,9),	
TotalCNNTURK decimal(15,9),	
TotalHALKTV decimal(15,9),	
TotalKRT	 decimal(15,9),	
TotalAHABER	 decimal(15,9),	
TotalTRTHABER	 decimal(15,9),	
TotalTELE1	 decimal(15,9),	
TotalTV100	 decimal(15,9),	
TotalHABERGLOBAL decimal(15,9),	
TotalSOZCUTV decimal(15,9),	

ABSESNTV decimal(15,9),
ABSESHABERTURK decimal(15,9),	
ABSESCNNTURK decimal(15,9),	
ABSESHALKTV decimal(15,9),	
ABSESKRT	 decimal(15,9),	
ABSESAHABER	 decimal(15,9),	
ABSESTRTHABER	 decimal(15,9),	
ABSESTELE1	 decimal(15,9),	
ABSESTV100	 decimal(15,9),	
ABSESHABERGLOBAL decimal(15,9),	
ABSESSOZCUTV decimal(15,9),	

RankTotalNTV int,
RankTotalHABERTURK int,	
RankTotalCNNTURK int,	
RankTotalHALKTV int,	
RankTotalKRT	 int,	
RankTotalAHABER	 int,	
RankTotalTRTHABER	 int,	
RankTotalTELE1	 int,	
RankTotalTV100	 int,	
RankTotalHABERGLOBAL int,	
RankTotalSOZCUTV int,	

RankABSESNTV int,
RankABSESHABERTURK int,	
RankABSESCNNTURK int,	
RankABSESHALKTV int,	
RankABSESKRT	 int,	
RankABSESAHABER	 int,	
RankABSESTRTHABER	 int,	
RankABSESTELE1	 int,	
RankABSESTV100	 int,	
RankABSESHABERGLOBAL int,	
RankABSESSOZCUTV int,	

CONSTRAINT PK_TD PRIMARY KEY (id),
INDEX ind1_TD (tableDate,tableType)
)



--alt + f1



SELECT * FROM  TotalDay WHERE tableType = 'SHARE' ORDER BY tableDate

TRUNCATE TABLE TotalDay



SELECT *
FROM TotalDay
WHERE tableDate >= '2024-02-01' 
  AND tableDate <= '2024-02-19'
  AND tableType = 'SHARE'
  ORDER BY tableDate







SELECT * FROM OutputTable WHERE tableType = 'RTG' ORDER BY timebands

TRUNCATE TABLE OutputTable





--NTV
--HABERTURK
--CNNTURK
--HALKTV
--KRT	
--AHABER	
--TRTHABER	
--TELE1	
--TV100	
--HABERGLOBAL	
--SOZCUTV
