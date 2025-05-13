USE RatingSystem

go


SELECT	cp.* 
FROM	channelPeriods cp
WHERE	cp.periodDate='2024-02-19'


TRUNCATE TABLE OUTPUTTABLE

INSERT INTO OUTPUTTABLE (Timebands, tabledate ,tableType)
SELECT	DISTINCT cp.Timebands, cp.periodDate, 'RTG' tableType
FROM	channelPeriods cp
WHERE	cp.periodDate='2024-02-19'
	AND cp.Timebands != 'Total Day'
ORDER BY cp.Timebands



--SELECT * FROM OUTPUTTABLE ORDER BY timebands

SELECT	cp.*, OU.*
FROM	channelPeriods cp WITH(NOLOCK) INNER JOIN
		OUTPUTTABLE OU  WITH(NOLOCK) ON (OU.Timebands = cp.Timebands AND OU.tabledate = cp.periodDate AND OU.tabletype = 'RTG' )
WHERE	cp.periodDate='2024-02-19'
	AND cp.Timebands != 'Total Day'
	AND cp.channelid = 2
ORDER BY cp.Timebands



UPDATE	OU
SET		OU.TotalNTV = cp.RtgIndividuals5Plus
--SELECT	cp.*, OU.*
FROM	channelPeriods cp WITH(NOLOCK) INNER JOIN
		OUTPUTTABLE OU  WITH(NOLOCK) ON (OU.Timebands = cp.Timebands AND OU.tabledate = cp.periodDate AND OU.tabletype = 'RTG' )
WHERE	cp.periodDate='2024-02-19'
	AND cp.Timebands != 'Total Day'
	AND cp.channelid = 2



UPDATE	OU
SET		OU.TotalHABERTURK = cp.RtgIndividuals5Plus
--SELECT	cp.*, OU.*
FROM	channelPeriods cp WITH(NOLOCK) INNER JOIN
		OUTPUTTABLE OU  WITH(NOLOCK) ON (OU.Timebands = cp.Timebands AND OU.tabledate = cp.periodDate AND OU.tabletype = 'RTG' )
WHERE	cp.periodDate='2024-02-19'
	AND cp.Timebands != 'Total Day'
	AND cp.channelid = 3
GO







---------------------------------------------------------PROCEDURE----------------------------------------------------------------------------------------

ALTER PROCEDURE JoinChannelPeriodsOutputTable
    @date dateTime
	
AS
BEGIN
	DECLARE @sSQL VARCHAR(8000) 
	DECLARE @Field1 VARCHAR(1000)
	DECLARE @Field2 VARCHAR(1000)
	DECLARE @id int


	IF OBJECT_ID('tempdb..#tmpCHANNELS') IS NOT NULL 
	BEGIN 
		DROP TABLE #tmpCHANNELS 
	END
	SELECT * INTO #tmpCHANNELS FROM CHANNELS WHERE id IN (2,3,4,6,17,8,14,15,16,18,23) ORDER BY id

	--SELECT * FROM #tmpCHANNELS WHERE id IN (2,3)


	WHILE EXISTS(SELECT 1 FROM #tmpCHANNELS)
	BEGIN 

		SELECT @id = id FROM #tmpCHANNELS

		if(@id=2) SELECT @Field1 = 'TotalNTV', @Field2 = 'ABSESNTV'
		if(@id=3) SELECT @Field1 = 'TotalHABERTURK', @Field2 = 'ABSESHABERTURK'
		if(@id=4) SELECT @Field1 = 'TotalCNNTURK', @Field2 = 'ABSESCNNTURK'
		if(@id=6) SELECT @Field1 = 'TotalHALKTV', @Field2 = 'ABSESHALKTV'
		if(@id=17) SELECT @Field1 = 'TotalKRT	', @Field2 = 'ABSESKRT	'
		if(@id=8) SELECT @Field1 = 'TotalAHABER	', @Field2 = 'ABSESAHABER	'
		if(@id=14) SELECT @Field1 = 'TotalTRTHABER	', @Field2 = 'ABSESTRTHABER	'
		if(@id=15) SELECT @Field1 = 'TotalTELE1	', @Field2 = 'ABSESTELE1	'
		if(@id=16) SELECT @Field1 = 'TotalTV100	', @Field2 = 'ABSESTV100	'
		if(@id=18) SELECT @Field1 = 'TotalHABERGLOBAL', @Field2 = 'ABSESHABERGLOBAL'
		if(@id=23) SELECT @Field1 = 'TotalSOZCUTV', @Field2 = 'ABSESSOZCUTV'

	
		SELECT @sSQL = '
	UPDATE	OU
	SET		OU.'+@Field1+' = cp.RtgIndividuals5Plus, OU.'+@Field2+' = cp.RtgIndividualsSESAB 
	FROM	channelPeriods cp WITH(NOLOCK) INNER JOIN
			OUTPUTTABLE OU  WITH(NOLOCK) ON (OU.Timebands = cp.Timebands AND OU.tabledate = cp.periodDate AND OU.tabletype = ''RTG'' )
		WHERE	cp.periodDate= '''+CONVERT(VARCHAR(10),@date,112)+'''
		AND cp.Timebands != ''Total Day''
		AND cp.channelid = ' + CAST(@id AS VARCHAR(10))

		EXEC(@sSQL)

	
		SELECT @sSQL = '
	UPDATE	OU
	SET		OU.'+@Field1+' = cp.ShareIndividuals5Plus, OU.'+@Field2+' = cp.ShareIndividualsSESAB 
	FROM	channelPeriods cp WITH(NOLOCK) INNER JOIN
			OUTPUTTABLE OU  WITH(NOLOCK) ON (OU.Timebands = cp.Timebands AND OU.tabledate = cp.periodDate AND OU.tabletype = ''SHARE'' )
	WHERE	cp.periodDate= '''+CONVERT(VARCHAR(10),@date,112)+'''
		AND cp.Timebands != ''Total Day''
		AND cp.channelid = ' + CAST(@id AS VARCHAR(10))

		EXEC(@sSQL)
		DELETE #tmpCHANNELS WHERE id= @id
	END


	END;



---------------------------------------------------------PROCEDURE----------------------------------------------------------------------------------------





DECLARE @sSQL VARCHAR(8000) 
DECLARE @Field1 VARCHAR(1000)
DECLARE @Field2 VARCHAR(1000)
DECLARE @id int

--INSERT INTO OUTPUTTABLE (Timebands, tabledate ,tableType)
--SELECT	DISTINCT cp.Timebands, cp.periodDate, 'RTG' tableType
--FROM	channelPeriods cp
--WHERE	cp.periodDate='2024-02-19'
--	AND cp.Timebands != 'Total Day'
--ORDER BY cp.Timebands

--INSERT INTO OUTPUTTABLE (Timebands, tabledate ,tableType)
--SELECT	DISTINCT cp.Timebands, cp.periodDate, 'SHARE' tableType
--FROM	channelPeriods cp
--WHERE	cp.periodDate='2024-02-19'
--	AND cp.Timebands != 'Total Day'
--ORDER BY cp.Timebands

IF OBJECT_ID('tempdb..#tmpCHANNELS') IS NOT NULL 
BEGIN 
    DROP TABLE #tmpCHANNELS 
END
SELECT * INTO #tmpCHANNELS FROM CHANNELS WHERE id IN (2,3,4,6,17,8,14,15,16,18,23) ORDER BY id

--SELECT * FROM #tmpCHANNELS WHERE id IN (2,3)


WHILE EXISTS(SELECT 1 FROM #tmpCHANNELS)
BEGIN 

	SELECT @id = id FROM #tmpCHANNELS

	if(@id=2) SELECT @Field1 = 'TotalNTV', @Field2 = 'ABSESNTV'
	if(@id=3) SELECT @Field1 = 'TotalHABERTURK', @Field2 = 'ABSESHABERTURK'
	if(@id=4) SELECT @Field1 = 'TotalCNNTURK', @Field2 = 'ABSESCNNTURK'
	if(@id=6) SELECT @Field1 = 'TotalHALKTV', @Field2 = 'ABSESHALKTV'
	if(@id=17) SELECT @Field1 = 'TotalKRT	', @Field2 = 'ABSESKRT	'
	if(@id=8) SELECT @Field1 = 'TotalAHABER	', @Field2 = 'ABSESAHABER	'
	if(@id=14) SELECT @Field1 = 'TotalTRTHABER	', @Field2 = 'ABSESTRTHABER	'
	if(@id=15) SELECT @Field1 = 'TotalTELE1	', @Field2 = 'ABSESTELE1	'
	if(@id=16) SELECT @Field1 = 'TotalTV100	', @Field2 = 'ABSESTV100	'
	if(@id=18) SELECT @Field1 = 'TotalHABERGLOBAL', @Field2 = 'ABSESHABERGLOBAL'
	if(@id=23) SELECT @Field1 = 'TotalSOZCUTV', @Field2 = 'ABSESSOZCUTV'

	
	SELECT @sSQL = '
UPDATE	OU
SET		OU.'+@Field1+' = cp.RtgIndividuals5Plus, OU.'+@Field2+' = cp.RtgIndividualsSESAB 
FROM	channelPeriods cp WITH(NOLOCK) INNER JOIN
		OUTPUTTABLE OU  WITH(NOLOCK) ON (OU.Timebands = cp.Timebands AND OU.tabledate = cp.periodDate AND OU.tabletype = ''RTG'' )
WHERE	cp.periodDate=''2024-02-19''
	AND cp.Timebands != ''Total Day''
	AND cp.channelid = ' + CAST(@id AS VARCHAR(10))

	EXEC(@sSQL)

	
	SELECT @sSQL = '
UPDATE	OU
SET		OU.'+@Field1+' = cp.ShareIndividuals5Plus, OU.'+@Field2+' = cp.ShareIndividualsSESAB 
FROM	channelPeriods cp WITH(NOLOCK) INNER JOIN
		OUTPUTTABLE OU  WITH(NOLOCK) ON (OU.Timebands = cp.Timebands AND OU.tabledate = cp.periodDate AND OU.tabletype = ''SHARE'' )
WHERE	cp.periodDate=''2024-02-19''
	AND cp.Timebands != ''Total Day''
	AND cp.channelid = ' + CAST(@id AS VARCHAR(10))

	EXEC(@sSQL)
	DELETE #tmpCHANNELS WHERE id= @id
END




  
  

  