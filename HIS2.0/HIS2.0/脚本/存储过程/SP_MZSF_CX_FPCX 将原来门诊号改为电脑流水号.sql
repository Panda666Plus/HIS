USE [Trasen_0329]
GO
/****** Object:  StoredProcedure [dbo].[SP_MZSF_CX_FPCX]    Script Date: 2017/8/31 11:23:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

ALTER PROCEDURE [dbo].[SP_MZSF_CX_FPCX]          
 (            
 @rq1 varchar(30),          
 @rq2 varchar(30),          
 @FPH VARCHAR(30),          
 --@BLH VARCHAR(30), 
 -- Add By Mr.Chan 2017-08-31
 @DNLSH BIGINT,         
 @BRXM VARCHAR(20),          
 @SFY INT,          
 @YBLX INT,          
 @bak smallint,          
 @lx int,-- 类型0=收费1=挂号 -1=全部          
 @klx int, --卡类型 Add By Zj 2012-12-27           
 @kh varchar(30),-- 卡号          
 @fph1 varchar(30),          
 @fph2 varchar(30),          
 @zffs varchar(30),          
 @fpid varchar(50)          
 )           
as          
/*          
  退费三级审核发票信息查询 Add by zp 2014-01-26          
exec SP_MZSF_CX_FPCX '2001-01-01 00:00:00','2012-01-01 00:00:00','','','',0,0,0,-1,'','','','',''         
        
exec SP_MZSF_CX_FPCX '2014-03-06 00:00:00','2014-03-09 23:59:59','','','','','',0,0,'000984','','','','00000000-0000-0000-0000-000000000000',0         
select  top 10 * from mz_fpb          
select * from mz_brxx          
*/          
BEGIN          
declare @fpb varchar(30)          
declare @ss varchar(8000)          
set @fpb='mz_fpb'          
if @BAK=1           
begin          
set @fpb='mz_fpb_h'          
end           
          
CREATE TABLE #temp(          
 [FPID] UNIQUEIDENTIFIER  NOT NULL,            
 [YBLX] [int] NOT NULL DEFAULT ((0)),          
 [YBJYDJH] [varchar](30) COLLATE Chinese_PRC_CI_AS NULL,          
 [BLH] [varchar](30) COLLATE Chinese_PRC_CI_AS NULL,          
 [DNLSH] bigint NULL,          
 [BRXM] [varchar](30) COLLATE Chinese_PRC_CI_AS NULL,          
 [SFRQ] DATETIME NULL,          
 [SFY] [int] NOT NULL DEFAULT ((0)),          
 [FPH] [varchar](30) COLLATE Chinese_PRC_CI_AS NULL,          
 [ZJE] [decimal](18, 2) NOT NULL DEFAULT ((0)),          
 [YBZHZF] [decimal](18, 2) NOT NULL DEFAULT ((0)),          
 [YBJJZF] [decimal](18, 2) NOT NULL DEFAULT ((0)),          
 [YBBZZF] [decimal](18, 2) NOT NULL DEFAULT ((0)),          
 [YLKZF] [decimal](18, 2) NOT NULL DEFAULT ((0)),          
 [YHJE] [decimal](18, 2) NOT NULL DEFAULT ((0)),          
 [CWJZ] [decimal](18, 2) NOT NULL DEFAULT ((0)),          
 [QFGZ] [decimal](18, 2) NOT NULL DEFAULT ((0)),          
 [XJZF] [decimal](18, 2) NOT NULL DEFAULT ((0)),          
 [ZPZF] [decimal](18, 2) NOT NULL DEFAULT ((0)),          
 [SRJE] [decimal](18, 2) NOT NULL DEFAULT ((0)),          
 [ZFFPID] UNIQUEIDENTIFIER,          
 [BGHPBZ] [smallint] NOT NULL DEFAULT ((0)),          
 [JLZT] [varchar](2) NOT NULL DEFAULT ((0)),          
 [ZFRQ]  datetime,          
 [ZFY] [int] NOT NULL DEFAULT (0),          
 [YXBZ] [int] NOT NULL DEFAULT (0),          
 [YBLXMC] VARCHAR(30),          
 [KH] varchar(30),          
 [GHXXID] UNIQUEIDENTIFIER,          
 [YXTF] INT,                ---允许退费 0不允许退费 1允许退费          
 [TFSQID] UNIQUEIDENTIFIER,  ---退费申请id ,    
 ks varchar(50)       
)          
set @ss='insert into #temp(FPID,YBLX,YBJYDJH,BLH,BRXM,SFRQ,SFY,FPH,ZJE,YBZHZF,YBJJZF,YBBZZF,YLKZF,          
YHJE,CWJZ,QFGZ,XJZF,ZPZF,SRJE,ZFFPID,BGHPBZ,JLZT,ZFRQ,ZFY,YXBZ,YBLXMC,KH,DNLSH,GHXXID,YXTF,TFSQID,ks)          
select A.FPID,YBLX,YBJYDJH,BLH,BRXM,SFRQ,SFY,FPH,ZJE,YBZHZF,YBJJZF,YBBZZF,YLKZF,          
YHJE,CWJZ,QFGZ,XJZF,ZPZF,SRJE,ZFFPID,BGHPBZ,JLZT,NULL zfrq,0 zfy,0 yxbz,NAME,c.KH,DNLSH,a.GHXXID,1,dbo.FUN_GETEMPTYGUID()      
,dbo.fun_getDeptname(a.ksdm)  as ks     
from mz_fpb a (nolock)           
left join jc_yblx b (nolock) on a.yblx=b.id           
left join YY_kdjb c (nolock) on a.kdjid=c.kdjid           
left join (select distinct FPID FROM MZ_TFSQRECORD where SHBZ=1 AND FSBZ=1 and TFBZ =0 and BSCBZ = 0) d on d.FPID=a.FPID          
where a.bscbz=0 and (BGHPBZ='+convert(varchar,@lx)+' or '+convert(varchar,@lx)+'=-1 or '+convert(varchar,@lx)+'=2) '           
          
if @rq1<>''          
  set @ss=@ss+' and sfrq>='''+@rq1+''' and sfrq<='''+@rq2+''''          
if @fph<>''          
set @ss=@ss+' and fph='''+@fph+''''          
--if @BLH<>''          
--set @ss=@ss+' and blh='''+@BLH+''''
-- Add By Mr.Chan 2017-08-31
IF @DNLSH<>0
SET @ss=@ss+' and dnlsh='''+CAST(@DNLSH AS VARCHAR(20))+''''          
if @brxm<>''          
set @ss=@ss+' and ( brxm like ''%'+@brxm+'%'' or dbo.getpywb(brxm,0)='''+@brxm+''' or dbo.getpywb(brxm,1)='''+@brxm+''')'          
if @sfy>0          
set @ss=@ss+' and sfy='+cast(@sfy as varchar(10))+''          
if @yblx>0          
set @ss=@ss+' and yblx='+cast(@yblx as varchar(10))+''          
if @klx<>0 --add by zj 2012-12-27          
set @ss=@ss+' and klx='+CAST(@klx as varchar(10))+''          
if @kh<>''          
set @ss=@ss+' and c.kh='''+@KH+''''          
if @fph1<>''          
  set @ss=@ss+' and fph>='''+@fph1+''' and fph<='''+@fph2+''''          
if @fpid<>''          
 set @ss=@ss + ' and a.fpid=''' + rtrim(@fpid) + ''''          
          
if @zffs='xjzf'  set @ss=@ss+' and xjzf<>0 '          
if @zffs='ylkzf'  set @ss=@ss+' and ylkzf<>0 '          
if @zffs='qfgz'  set @ss=@ss+' and qfgz<>0 '          
if @zffs='cwjz'  set @ss=@ss+' and cwjz<>0 '          
if @zffs='ybzf'  set @ss=@ss+' and (YBZHZF+YBJJZF+YBBZZF)<>0 '          
if @zffs='zpzf'  set @ss=@ss+' and zpzf<>0 '          
if @zffs='yhje'  set @ss=@ss+' and yhje<>0 '          
          
print @ss          
exec(@ss)          
         
---获取未申请退费的处方发票信息 Add By ZP 2014-02-07 通过YXTF区分          
set @ss='insert into #temp(FPID,YBLX,YBJYDJH,BLH,BRXM,SFRQ,SFY,FPH,ZJE,YBZHZF,YBJJZF,YBBZZF,YLKZF,          
YHJE,CWJZ,QFGZ,XJZF,ZPZF,SRJE,ZFFPID,BGHPBZ,JLZT,ZFRQ,ZFY,YXBZ,YBLXMC,KH,DNLSH,GHXXID,YXTF,TFSQID,ks)          
select FPID,YBLX,YBJYDJH,BLH,BRXM,SFRQ,SFY,FPH,ZJE,YBZHZF,YBJJZF,YBBZZF,YLKZF,          
YHJE,CWJZ,QFGZ,XJZF,ZPZF,SRJE,ZFFPID,BGHPBZ,JLZT,NULL zfrq,0 zfy,0 yxbz,NAME,KH,DNLSH,GHXXID,0,dbo.FUN_GETEMPTYGUID()     
,dbo.fun_getDeptname(a.ksdm)  as ks     
from '+@fpb+' a (nolock)           
left join jc_yblx b (nolock) on a.yblx=b.id           
left join YY_kdjb c (nolock) on a.kdjid=c.kdjid           
where bscbz=0 and (BGHPBZ='+convert(varchar,@lx)+' or '+convert(varchar,@lx)+'=-1 or '+convert(varchar,@lx)+'=2)           
and FPID NOT IN (SELECT FPID FROM #temp) '          
if @rq1<>''          
  set @ss=@ss+' and sfrq>='''+@rq1+''' and sfrq<='''+@rq2+''''          
if @fph<>''          
set @ss=@ss+' and fph='''+@fph+''''          
--if @BLH<>''          
--set @ss=@ss+' and blh='''+@BLH+''''    
-- Add By Mr.Chan  2017-08-31
IF @DNLSH<>0
SET @ss=@ss+' and dnlsh='''+CAST(@DNLSH AS VARCHAR(20))+''''      
if @brxm<>''          
set @ss=@ss+' and ( brxm like ''%'+@brxm+'%'' or dbo.getpywb(brxm,0)='''+@brxm+''' or dbo.getpywb(brxm,1)='''+@brxm+''')'          
if @sfy>0          
set @ss=@ss+' and sfy='+cast(@sfy as varchar(10))+''          
if @yblx>0          
set @ss=@ss+' and yblx='+cast(@yblx as varchar(10))+''          
if @klx<>0 --add by zj 2012-12-27          
set @ss=@ss+' and klx='+CAST(@klx as varchar(10))+''          
if @kh<>''          
set @ss=@ss+' and kh='''+@KH+''''          
if @fph1<>''          
  set @ss=@ss+' and fph>='''+@fph1+''' and fph<='''+@fph2+''''          
if @fpid<>''          
 set @ss=@ss + ' and fpid=''' + rtrim(@fpid) + ''''          
          
if @zffs='xjzf'  set @ss=@ss+' and xjzf<>0 '          
if @zffs='ylkzf'  set @ss=@ss+' and ylkzf<>0 '          
if @zffs='qfgz'  set @ss=@ss+' and qfgz<>0 '          
if @zffs='cwjz'  set @ss=@ss+' and cwjz<>0 '          
if @zffs='ybzf'  set @ss=@ss+' and (YBZHZF+YBJJZF+YBBZZF)<>0 '          
if @zffs='zpzf'  set @ss=@ss+' and zpzf<>0 '          
if @zffs='yhje'  set @ss=@ss+' and yhje<>0 '          
          
print @ss          
exec(@ss)          
          
          
update #temp set yxbz=1 where jlzt=0          
update a set a.yxbz=1,a.zfrq=b.sfrq,a.zfy=b.sfy from #temp a(nolock),#temp b(nolock) where a.fpid=b.zffpid           
update #temp set yxbz=2 where isnull(zffpid,dbo.FUN_GETEMPTYGUID()) in(select fpid from #temp (nolock))          
          
--00的记录状态表示需要参与金额统计          
update a set a.yxbz=1,jlzt='00' from #temp a,#temp b where a.zffpid=b.fpid and a.yxbz=2          
and (abs(a.xjzf)<>abs(b.xjzf) or abs(a.ylkzf)<>abs(b.ylkzf) or abs(a.cwjz)<>abs(b.cwjz)          
or abs(a.qfgz)<>abs(b.qfgz) )          
update b set b.jlzt='00' from #temp a,#temp b where a.zffpid=b.fpid and a.yxbz=1          
and (abs(a.xjzf)<>abs(b.xjzf) or abs(a.ylkzf)<>abs(b.ylkzf) or abs(a.cwjz)<>abs(b.cwjz)          
or abs(a.qfgz)<>abs(b.qfgz) )          
  
          
          
update #temp set yxbz=1,jlzt='0' where jlzt=1 and fpid not in(select isnull(zffpid,dbo.FUN_GETEMPTYGUID()) from #temp(nolock))          
update #temp set yxbz=1,jlzt='00' where jlzt=2 and  isnull(zffpid,dbo.FUN_GETEMPTYGUID()) not in(select fpid from #temp(nolock))          
          
set @ss='select  '''' 序号,1 选择,(case when bghpbz=1 then ''挂号'' when bghpbz=2 then ''诊疗卡费'' else ''收费'' end) 类型,          
blh 门诊号,brxm 姓名,fph 发票号,zje 发票金额,SFRQ 收费日期,dbo.fun_getempname(sfy) 收费员,          
(case when xjzf=0 then '''' else cast(xjzf as varchar(30)) end) 现金支付,          
(case when (ybzhzf+ybjjzf+ybbzzf)=0 then '''' else cast((ybzhzf+ybjjzf+ybbzzf) as varchar(30)) end)  医保支付,          
(case when yhje=0 then '''' else cast(yhje as varchar(30)) end) 优惠金额,          
(case when zpzf=0 then '''' else cast(zpzf as varchar(30)) end) 支票支付,          
(case when ylkzf=0 then '''' else cast(ylkzf as varchar(30)) end) 银联支付,          
(case when cwjz=0 then '''' else cast(cwjz as varchar(30)) end) 财务记账,          
(case when qfgz=0 then '''' else cast(qfgz as varchar(30)) end) 欠费挂账,          
(case when srje=0 then '''' else cast(srje as varchar(30)) end) 舍入金额,          
YBLXMC 医保类型,ybjydjh 就医号,FPID,sfy,jlzt  记录状态,          
ZFRQ 退费日期,dbo.fun_getempname(zfy) 退费员, kh 卡号,yblx,dnlsh as 电脑流水号,GHXXID,YXTF,TFSQID,ks from           
#temp a(nolock) where yxbz=1 order by sfrq'          
exec(@ss)          
          
drop table #temp          
--drop table #temp1          
--drop table #temp2          
          
END          
          
          
/*          
modify by wangzhi 2010-12-02          
 增加参数@fpid，来支持按FPID查询记录          
modify by wangzhi 2010-12-06          
 结果集增加输出字段'DNLSH','GHXXID'          
*/
