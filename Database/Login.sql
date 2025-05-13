CREATE TABLE LoginTable
(
id int identity(1,1) not null,
LoginUsername nvarchar(60),
LoginPassword nvarchar(60),

CONSTRAINT PK_LoginTable PRIMARY KEY (id),

INDEX ind1_LoginTable (LoginUsername),
INDEX ind2_LoginTable (LoginPassword)

)

DELETE FROM LoginTable WHERE LoginUsername = 'consoleApp'
INSERT INTO LoginTable (LoginUsername,LoginPassword) VALUES ('admin','admin')

INSERT INTO LoginTable (LoginUsername,LoginPassword) VALUES ('user','123')

INSERT INTO LoginTable (LoginUsername,LoginPassword) VALUES ('consoleApp','ksR27,iQ3?8OhcgFt5342/Y&mB6c')

SELECT * FROM LoginTable

SELECT LoginUsername,LoginPassword FROM LoginTable

UPDATE LOGÝNTABLE SET LoginPassword = '1' WHERE LOGÝNUSERNAME ='admin'

DELETE FROM LOGÝNTABLE WHERE id=14