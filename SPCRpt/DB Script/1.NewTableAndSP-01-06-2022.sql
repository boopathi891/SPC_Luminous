USE [dbSPC]
GO
/****** Object:  StoredProcedure [dbo].[SP_XBar_R_Chart_SPCDashboard]    Script Date: 5/31/2022 10:06:07 AM ******/
DROP PROCEDURE [dbo].[SP_XBar_R_Chart_SPCDashboard]
GO
/****** Object:  Table [dbo].[tbl_temp_SPCDashboard]    Script Date: 5/31/2022 10:06:07 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbl_temp_SPCDashboard]') AND type in (N'U'))
DROP TABLE [dbo].[tbl_temp_SPCDashboard]
GO
/****** Object:  Table [dbo].[tbl_temp_SPCDashboard]    Script Date: 5/31/2022 10:06:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_temp_SPCDashboard](
	[GroupNo] [varchar](200) NULL,
	[Weight_Temp] [numeric](18, 4) NULL
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[SP_XBar_R_Chart_SPCDashboard]    Script Date: 5/31/2022 10:06:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- EXEC SP_XBar_R_Chart '2.8HMM+VE','Positive'
CREATE PROCEDURE [dbo].[SP_XBar_R_Chart_SPCDashboard]
  (@ProductId varchar(200),
	@ProductType varchar(200))
	AS
	BEGIN
 
select AVG(Weight_Temp) as XBar,MAX(Weight_Temp) as Max_Weight
,MIN(Weight_Temp) as Min_Weight,(MAX(Weight_Temp)-MIN(Weight_Temp)) as Range,GroupNo ,
(Select Weight_Mean from tbl_Product_Master with (nolock) where Product_Id=@ProductId AND Product_Type=@ProductType) as Mean,
(Select Weight_USL from tbl_Product_Master with (nolock) where Product_Id=@ProductId AND Product_Type=@ProductType) as USL,
(Select Weight_LSL from tbl_Product_Master with (nolock) where Product_Id=@ProductId AND Product_Type=@ProductType) as LSL 
from tbl_temp_SPCDashboard(nolock) group by GroupNo order by GroupNo asc
END


GO
