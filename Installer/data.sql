USE master;
GO
CREATE DATABASE QuanLyQuanCafe
GO
USE QuanLyQuanCafe
GO
-- Food
-- Table
-- FoodCategory
-- Account
-- Bill
-- BilInfo
CREATE TABLE TableFood
(
    id INT IDENTITY(1,1) PRIMARY KEY,
    [name] NVARCHAR(100) NOT NULL DEFAULT N'Chưa đặt tên',
    [status] NVARCHAR(100) NOT NULL DEFAULT N'Trống', -- Trong || co nguoi
)
GO
CREATE TABLE Account
(
    UserName NVARCHAR(100) PRIMARY KEY,
    DisplayName NVARCHAR(100) NOT NULL DEFAULT N'Kter',
    [PassWord] NVARCHAR(100) NOT NULL DEFAULT 0,
    [Type] INT NOT NULL DEFAULT 0 -- 1: admin && 0: staff
)
GO
CREATE TABLE FoodCategory(
    id INT IDENTITY(1,1) PRIMARY KEY,
    [name] NVARCHAR(100) NOT NULL DEFAULT N'Chưa đặt tên',
)
GO
CREATE TABLE Food(
    id INT IDENTITY(1,1) PRIMARY KEY,
    [name] NVARCHAR(100) NOT NULL DEFAULT N'Chưa đặt tên',
    idCategory INT NOT NULL,
    price FLOAT NOT NULL DEFAULT 0,
    FOREIGN KEY(idCategory) REFERENCES FoodCategory(id)
)
GO
CREATE TABLE Bill(
    id INT IDENTITY(1,1) PRIMARY KEY,
    DateCheckIn DATE NOT NULL DEFAULT GETDATE(),
    DateCheckOut DATE,
    idTable INT NOT NULL,
    [status] INT NOT NULL DEFAULT 0,-- 1: đã thanh toán && 0: Chưa thanh toán
	discount int default 0,
	totalPrice float default 0,
    FOREIGN KEY(idTable) REFERENCES TableFood(id)
)
GO
CREATE TABLE BillInfo(
    id INT IDENTITY(1,1) PRIMARY KEY,
    idBill INT NOT NULL,
    idFood INT NOT NULL,
    [count] INT NOT NULL,
    FOREIGN KEY(idBill) REFERENCES Bill(id),
    FOREIGN KEY(idFood) REFERENCES Food(id)
)
GO
-- Chuỗi mã hóa mật khẩu = 1
-- 1962026656160185351301320480154111117132155
-- Đảo ngược lại:
-- 5512317111114510840231031535810616566202691
INSERT [dbo].[Account] ([UserName], [DisplayName], [PassWord], [Type]) VALUES (N'K9', N'BinhBoong', N'5512317111114510840231031535810616566202691', 1)
INSERT [dbo].[Account] ([UserName], [DisplayName], [PassWord], [Type]) VALUES (N'staff', N'staff', N'5512317111114510840231031535810616566202691', 0)
INSERT [dbo].[Account] ([UserName], [DisplayName], [PassWord], [Type]) VALUES (N'test', N'test', N'5512317111114510840231031535810616566202691', 1)
GO
CREATE or alter PROC USP_GetAccountByUserName
@userName NVARCHAR(100)
AS
BEGIN
    SELECT * FROM Account WHERE UserName=@userName;
END
GO
CREATE or alter PROC USP_Login
@userName NVARCHAR(100),@PassWord NVARCHAR(100)
AS
BEGIN
    SELECT * FROM Account WHERE UserName=@userName AND PassWord=@PassWord;
END
GO
-- Thêm bàn
DECLARE @i INT = 11
WHILE @i <= 30
BEGIN
    INSERT INTO TableFood(name)VALUES(N'Bàn '+ CAST(@i as NVARCHAR(100)));
    SET @i=@i+1;
END;
GO
CREATE or alter PROC USP_GetTableList
AS begin
	SELECT * FROM TableFood
end;
GO
-- Thêm thể loại (category)
INSERT [dbo].[FoodCategory] ([id], [name]) VALUES (1, N'Hải sản')
INSERT [dbo].[FoodCategory] ([id], [name]) VALUES (2, N'Nông sản')
INSERT [dbo].[FoodCategory] ([id], [name]) VALUES (3, N'Lâm sản')
INSERT [dbo].[FoodCategory] ([id], [name]) VALUES (4, N'Sản sản')
INSERT [dbo].[FoodCategory] ([id], [name]) VALUES (5, N'Nước')
INSERT [dbo].[FoodCategory] ([id], [name]) VALUES (6, N'Phở')
-- Thêm món ăn
go
INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (1, N'Mực một nắng nước sa tế', 1, 120000)
INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (2, N'Nghêu hấp xả', 1, 50000)
INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (3, N'Dú dê sữa nướng', 2, 50000)
INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (4, N'Heo rừng nướng muối ướt', 3, 75000)
INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (5, N'Cơm chiên mushi', 4, 99999)
INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (6, N'7Up', 5, 15000)
INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (7, N'Cafe', 5, 12000)
INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (18, N'Phỏ bò Hà Nội', 6, 30000)
INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (19, N'Phở bò Huế', 6, 35000)
GO
-- Thêm hóa đơn (bill)
INSERT [dbo].[Bill] ([id], [DateCheckIn], [DateCheckOut], [idTable], [status], [discount], [totalPrice]) VALUES (52, CAST(N'2023-05-01' AS Date), CAST(N'2023-05-01' AS Date), 1, 1, 20, 192000)
INSERT [dbo].[Bill] ([id], [DateCheckIn], [DateCheckOut], [idTable], [status], [discount], [totalPrice]) VALUES (53, CAST(N'2023-05-01' AS Date), NULL, 6, 0, 0, 0)
INSERT [dbo].[Bill] ([id], [DateCheckIn], [DateCheckOut], [idTable], [status], [discount], [totalPrice]) VALUES (54, CAST(N'2023-05-01' AS Date), CAST(N'2023-05-01' AS Date), 11, 1, 20, 719996.8)
INSERT [dbo].[Bill] ([id], [DateCheckIn], [DateCheckOut], [idTable], [status], [discount], [totalPrice]) VALUES (55, CAST(N'2023-05-01' AS Date), CAST(N'2023-05-01' AS Date), 7, 1, 20, 319996.8)
INSERT [dbo].[Bill] ([id], [DateCheckIn], [DateCheckOut], [idTable], [status], [discount], [totalPrice]) VALUES (56, CAST(N'2023-05-01' AS Date), CAST(N'2023-05-01' AS Date), 4, 1, 20, 272000)
INSERT [dbo].[Bill] ([id], [DateCheckIn], [DateCheckOut], [idTable], [status], [discount], [totalPrice]) VALUES (57, CAST(N'2023-05-01' AS Date), CAST(N'2023-05-01' AS Date), 3, 1, 20, 104000)
INSERT [dbo].[Bill] ([id], [DateCheckIn], [DateCheckOut], [idTable], [status], [discount], [totalPrice]) VALUES (58, CAST(N'2023-05-01' AS Date), CAST(N'2023-05-01' AS Date), 5, 1, 20, 900000)
INSERT [dbo].[Bill] ([id], [DateCheckIn], [DateCheckOut], [idTable], [status], [discount], [totalPrice]) VALUES (59, CAST(N'2023-05-01' AS Date), CAST(N'2023-05-01' AS Date), 10, 1, 20, 62400)
INSERT [dbo].[Bill] ([id], [DateCheckIn], [DateCheckOut], [idTable], [status], [discount], [totalPrice]) VALUES (60, CAST(N'2023-05-01' AS Date), CAST(N'2023-05-01' AS Date), 23, 1, 20, 38400)
INSERT [dbo].[Bill] ([id], [DateCheckIn], [DateCheckOut], [idTable], [status], [discount], [totalPrice]) VALUES (61, CAST(N'2023-05-01' AS Date), CAST(N'2023-05-01' AS Date), 25, 1, 20, 48000)
INSERT [dbo].[Bill] ([id], [DateCheckIn], [DateCheckOut], [idTable], [status], [discount], [totalPrice]) VALUES (62, CAST(N'2023-05-01' AS Date), CAST(N'2023-05-01' AS Date), 17, 1, 20, 240000)
INSERT [dbo].[Bill] ([id], [DateCheckIn], [DateCheckOut], [idTable], [status], [discount], [totalPrice]) VALUES (63, CAST(N'2023-05-01' AS Date), CAST(N'2023-05-01' AS Date), 15, 1, 0, 660000)
INSERT [dbo].[Bill] ([id], [DateCheckIn], [DateCheckOut], [idTable], [status], [discount], [totalPrice]) VALUES (64, CAST(N'2023-05-01' AS Date), CAST(N'2023-05-01' AS Date), 30, 1, 20, 319996.8)
INSERT [dbo].[Bill] ([id], [DateCheckIn], [DateCheckOut], [idTable], [status], [discount], [totalPrice]) VALUES (65, CAST(N'2023-05-01' AS Date), CAST(N'2023-05-01' AS Date), 27, 1, 20, 639993.6)
INSERT [dbo].[Bill] ([id], [DateCheckIn], [DateCheckOut], [idTable], [status], [discount], [totalPrice]) VALUES (66, CAST(N'2023-05-01' AS Date), CAST(N'2023-05-01' AS Date), 1, 1, 0, 720000)

GO
-- Thêm bill info
INSERT [dbo].[BillInfo] ([id], [idBill], [idFood], [count]) VALUES (82, 52, 1, 2)
INSERT [dbo].[BillInfo] ([id], [idBill], [idFood], [count]) VALUES (87, 56, 1, 2)
INSERT [dbo].[BillInfo] ([id], [idBill], [idFood], [count]) VALUES (88, 56, 3, 2)
INSERT [dbo].[BillInfo] ([id], [idBill], [idFood], [count]) VALUES (89, 57, 3, 2)
INSERT [dbo].[BillInfo] ([id], [idBill], [idFood], [count]) VALUES (90, 57, 6, 2)
INSERT [dbo].[BillInfo] ([id], [idBill], [idFood], [count]) VALUES (91, 58, 4, 15)
INSERT [dbo].[BillInfo] ([id], [idBill], [idFood], [count]) VALUES (92, 59, 6, 2)
INSERT [dbo].[BillInfo] ([id], [idBill], [idFood], [count]) VALUES (93, 59, 7, 4)
INSERT [dbo].[BillInfo] ([id], [idBill], [idFood], [count]) VALUES (94, 60, 7, 4)
INSERT [dbo].[BillInfo] ([id], [idBill], [idFood], [count]) VALUES (95, 61, 6, 4)
INSERT [dbo].[BillInfo] ([id], [idBill], [idFood], [count]) VALUES (96, 62, 4, 4)
INSERT [dbo].[BillInfo] ([id], [idBill], [idFood], [count]) VALUES (97, 63, 4, 4)
INSERT [dbo].[BillInfo] ([id], [idBill], [idFood], [count]) VALUES (98, 64, 5, 4)
INSERT [dbo].[BillInfo] ([id], [idBill], [idFood], [count]) VALUES (99, 65, 5, 8)
INSERT [dbo].[BillInfo] ([id], [idBill], [idFood], [count]) VALUES (100, 55, 5, 4)
INSERT [dbo].[BillInfo] ([id], [idBill], [idFood], [count]) VALUES (101, 54, 5, 4)
INSERT [dbo].[BillInfo] ([id], [idBill], [idFood], [count]) VALUES (102, 54, 3, 4)
INSERT [dbo].[BillInfo] ([id], [idBill], [idFood], [count]) VALUES (103, 54, 2, 6)
INSERT [dbo].[BillInfo] ([id], [idBill], [idFood], [count]) VALUES (107, 53, 1, 4)
GO
create or alter proc USP_InsertBill
@idTable int
as
begin
	insert into Bill(DateCheckIn,DateCheckOut,idTable,status,discount)values
	(GETDATE(),NULL,@idTable,0,0)
end;
go
create or alter proc USP_InsertBillInfo
@idBill int, @idFood int, @count int
as
begin
	declare @isExistBillInfo int;
	declare @foodCount int = 1;

	select @isExistBillInfo = b.id, @foodCount = b.count from BillInfo as b
	where idBill=@idBill and idFood=@idFood
	if(@isExistBillInfo > 0)
		begin
			declare @newCount int = @foodCount+@count;
			if(@newCount > 0)
				update BillInfo set count = @newCount where idBill=@idBill and idFood=@idFood;
			else
				delete BillInfo where idBill=@idBill and idFood=@idFood;
		end
	else
		begin
		  	INSERT into BillInfo(idBill,idFood,[count])VALUES
			(@idBill,@idFood,@count)
		end
end
go
create or alter trigger UTG_UpdateBillInfo
on BillInfo for insert, update
as
begin
	declare @idBill int;
	select @idBill = idBill from inserted
	declare @idTable int

	select @idTable = idTable from Bill where id = @idBill and status = 0
	declare @count int;
	select @count = count(*) from BillInfo where idBill=@idBill
	if(@count > 0)
		update TableFood set status = N'Có người' where id = @idTable;
	else
		update TableFood set status = N'Trống' where id = @idTable;
end
go
create or alter trigger UTG_UpdateBill
on Bill for update
as
begin
	declare @idBill int;
	select @idBill = id from inserted
	declare @idTable int;
	select @idTable = idTable from Bill where id = @idBill
	declare @count int = 0;
	select @count = COUNT(*) from Bill where idTable = @idTable and status = 0
	if(@count = 0)
		  update TableFood set status = N'Trống' where id=@idTable;
end
go
-- PROC chuyển bàn (Hoán đổi 2 bàn với nhau)
create or alter proc USP_SwitchTable
@idTable1 int, @idTable2 int, @action varchar(100)
as begin
-- Hoán đổi bàn có idTable1 với bàn có idTable2
	declare @idFirstBill int, @idSecondBill int;

	declare @isFirstTableEmpty int = 1;
	declare @isSecondTableEmpty int = 1;

	select @idFirstBill=id from Bill where idTable = @idTable1 and status = 0;
	select @idSecondBill=id from Bill where idTable = @idTable2 and status = 0

	select id into BillTemp1 from Bill where idTable=@idTable1 and status = 0;
	select id into BillTemp2 from Bill where idTable=@idTable2 and status = 0;

	-- TH1: Bill1 null, Bill2 không null
	if((@idFirstBill IS NULL) AND (@idSecondBill IS NOT NULL))
		begin
			update Bill set idTable=@idTable1 where id in(select * from BillTemp2)
		end
	-- TH2: Bill2 null, Bill1 không null
	else if((@idFirstBill IS NOT NULL) AND (@idSecondBill IS NULL))
		begin
			if(@action='move')
				begin
					update Bill set idTable=@idTable2 where id in(select * from BillTemp1)
				end
		end		
	-- TH3: Bill1 không null, Bill2 không null
	else if((@idFirstBill IS NOT NULL) AND (@idSecondBill IS NOT NULL))
		begin
			if(@action = 'move')
				begin
					update Bill set idTable=@idTable1 where id in(select * from BillTemp2)
					update Bill set idTable=@idTable2 where id in(select * from BillTemp1)
				end
			else if(@action = 'merge')
				begin
					update Bill set idTable=@idTable1 where id in(select * from BillTemp2)
				end

		end	
-- Kiểm tra trạng thái rỗng cho bàn
	if((select count(*) from Bill as b, BillInfo as bi where b.id=bi.idBill and b.idTable=@idTable1 and b.status=0) > 0)
		begin
			 set @isFirstTableEmpty = 0
		end
	if((select count(*) from Bill as b, BillInfo as bi where b.id=bi.idBill and b.idTable=@idTable2 and b.status=0) > 0)
		begin
			 set @isSecondTableEmpty = 0
		end
	if(@isFirstTableEmpty = 1)
		begin
			update TableFood set status=N'Trống' where id=@idTable1; 
		end
	else
		begin
			update TableFood set status=N'Có người' where id=@idTable1; 
		end
	if(@isSecondTableEmpty = 1)
		begin
			update TableFood set status=N'Trống' where id=@idTable2;
		end
	else
		begin
			update TableFood set status=N'Có người' where id=@idTable2;
		end
	drop table BillTemp1;
	drop table BillTemp2;
end
go
create or alter proc USP_GetListBillByDate
@checkIn date, @checkOut date as
begin
	select t.name as [Tên bàn], b.totalPrice as[Tổng tiền], DateCheckIn as [Ngày vào],
	DateCheckOut as[Ngày ra], discount as[Giảm giá] from Bill as b, TableFood as t
	where DateCheckIn >= @checkIn AND DateCheckOut <= @checkOut AND b.status=1
	and t.id=b.idTable 
end
go
create or alter proc USP_UpdateAccount
@UserName nvarchar(100), @DisplayName nvarchar(100), @password nvarchar(100), @newPassword nvarchar(100)
as
begin
	declare @isRightPass int = 0;
	
	select @isRightPass = count(*) from Account where UserName=@UserName and PassWord=@password
	
	if(@isRightPass = 1)
		begin
		   if(@newPassword IS NULL or @newPassword = '')
			begin
				update Account set DisplayName=@DisplayName where UserName=@UserName
			end
			else
				update Account set DisplayName=@DisplayName, PassWord=@newPassword where UserName=@UserName
		end
end
go
create or alter trigger UTG_DeleteBillInfo on BillInfo for Delete as
begin
	select distinct idBill into tbl_idBillTemp from deleted
	select distinct idTable into tbl_idTableTemp from Bill where id in(select * from tbl_idBillTemp)

	declare @i int, @count int;
	set @i=1; set @count=0;
	 WHILE (@i <= (SELECT COUNT(*) FROM tbl_idTableTemp))
			BEGIN
				DECLARE @idTable int;
				SET @idTable = (SELECT b.idTable FROM (SELECT ROW_NUMBER() OVER (ORDER BY idTable ASC) AS rownumber, idTable
					FROM (SELECT DISTINCT idTable FROM tbl_idTableTemp) as a) AS b WHERE b.rownumber=@i)

				set @count = (select count(*) from BillInfo as bi, Bill as b, TableFood as t where bi.idBill=b.id and b.idTable=@idTable)
				if(@count = 0)
					begin
						update TableFood set status = N'Trống' where id = @idTable
					end
				SET @i = @i + 1;
			END;

	drop table tbl_idBillTemp;
	drop table tbl_idTableTemp;
end
go
CREATE or alter FUNCTION [dbo].[fuConvertToUnsign] ( @strInput NVARCHAR(4000) )
RETURNS NVARCHAR(4000) 
AS BEGIN 
	IF (@strInput IS NULL or @strInput = '') 
		RETURN @strInput
	DECLARE @RT NVARCHAR(4000) 
	DECLARE @SIGN_CHARS NCHAR(136) 
	DECLARE @UNSIGN_CHARS NCHAR (136) 
	SET @SIGN_CHARS = N'ăâđêôơưàảãạáằẳẵặắầẩẫậấèẻẽẹéềểễệế ìỉĩịíòỏõọóồổỗộốờởỡợớùủũụúừửữựứỳỷỹỵý ĂÂĐÊÔƠƯÀẢÃẠÁẰẲẴẶẮẦẨẪẬẤÈẺẼẸÉỀỂỄỆẾÌỈĨỊÍ ÒỎÕỌÓỒỔỖỘỐỜỞỠỢỚÙỦŨỤÚỪỬỮỰỨỲỶỸỴÝ' +NCHAR(272)+ NCHAR(208) SET @UNSIGN_CHARS = N'aadeoouaaaaaaaaaaaaaaaeeeeeeeeee iiiiiooooooooooooooouuuuuuuuuuyyyyy AADEOOUAAAAAAAAAAAAAAAEEEEEEEEEEIIIII OOOOOOOOOOOOOOOUUUUUUUUUUYYYYYDD' 
	DECLARE @COUNTER int 
	DECLARE @COUNTER1 int 
	SET @COUNTER = 1 
	WHILE (@COUNTER <=LEN(@strInput)) 
		BEGIN 
			SET @COUNTER1 = 1 
			WHILE (@COUNTER1 <=LEN(@SIGN_CHARS)+1) 
				BEGIN 
					IF UNICODE(SUBSTRING(@SIGN_CHARS, @COUNTER1,1)) = UNICODE(SUBSTRING(@strInput,@COUNTER ,1) ) 
						BEGIN 
							IF @COUNTER=1 SET @strInput = SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) + SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)-1) 
							ELSE 
								SET @strInput = SUBSTRING(@strInput, 1, @COUNTER-1) +SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) + SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)- @COUNTER) 
								BREAK 
						END 
						SET @COUNTER1 = @COUNTER1 +1 
				END 
				SET @COUNTER = @COUNTER +1 
		END 
		SET @strInput = replace(@strInput,' ','-')
	RETURN @strInput 
END
go
create or alter proc USP_GetListBillByDateAndPage
@checkIn date, @checkOut date, @page int
as begin
	declare @pageRows int = 10;
	declare @selectRows int = @pageRows
	declare @exceptRows int = (@page - 1) * @pageRows;
	
	-- Tạo bảng tạm
	;with BillShow as(select b.id, t.name as [Tên bàn], b.totalPrice as[Tổng tiền], DateCheckIn as [Ngày vào],
	DateCheckOut as[Ngày ra], discount as[Giảm giá] from Bill as b, TableFood as t
	where DateCheckIn >= @checkIn AND DateCheckOut <= @checkOut AND b.status = 1
	and t.id=b.idTable)

	select top (@selectRows) * from BillShow where id not in
	(select top (@exceptRows) id from BillShow)
end
go
create or alter proc USP_GetNumBillByDate
@checkIn date, @checkOut date as
begin
	select count(*)	from Bill as b, TableFood as t
	where DateCheckIn >= @checkIn AND DateCheckOut <= @checkOut AND b.status=1
	and t.id=b.idTable 
end
GO
create or alter proc USP_DeleteCategory
@idCategory int as
begin
	if (not exists (select * from FoodCategory where id = @idCategory))
		begin
		   select -1 as [result];
		end
	else if(exists (select * from BillInfo as bi, Bill as b, Food as f where bi.idBill=b.id and bi.idFood=f.id and f.idCategory=@idCategory and b.status = 0))
		begin
		   select 0 as [result];
		end
	else
		begin
			select 1 as [result]; -- Cho phép xóa 
		end
end
go
create or alter proc USP_DeleteTable(@idTable int) as 
begin
-- Tìm ra danh sách các Bill chưa idTable
-- Tạo bảng tạm
	;with BillShow as(select b.id from Bill as b, TableFood as t where b.idTable=@idTable)
	-- Xóa dữ liệu trong bảng BillInfo (chưa idBill -> idTable)
	delete BillInfo where idBill in(select * from BillShow);
	-- Xóa dữ liệu trong bảng Bill -> idTable
	delete Bill where idTable=@idTable;
	-- Xóa dữ liệu idTable
	delete TableFood where id=@idTable;
end
go
select * from TableFood
select * FROM Bill;
select * from BillInfo;
select * from Food;
select * from FoodCategory;
SELECT * FROM Food where idCategory = 10
go
SELECT * FROM Food where idCategory = 1
delete BillInfo;
delete Bill
go
update TableFood set status = N'Trống'
go
--SELECT a.name, b.title, c.join_date FROM dbo.users AS a,dbo.projects AS b,dbo.user_project AS c
--WHERE a.id=c.user_id AND b.id=c.project_id
--ORDER BY a.id OFFSET 7  ROWS FETCH NEXT 4 ROWS ONLY