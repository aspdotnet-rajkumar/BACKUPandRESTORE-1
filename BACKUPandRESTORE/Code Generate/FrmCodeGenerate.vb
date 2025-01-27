﻿Public Class FrmCodeGenerate

    Private Sub BtnGenerate_Click(sender As Object, e As EventArgs) Handles BtnGenerate.Click
        Try
            If IsBlankComboBox(cbxtype) Then Exit Sub
            If IsBlankTextBox(TxtTablename) Then Exit Sub
            If cbxtype.Text = "Main" Then
                Create_SQLMainScript()
            ElseIf cbxtype.Text = "Line" Then
                Create_SQLLineScript()
            ElseIf cbxtype.Text = "Get" Then

                Create_SQLGetallScript()
            ElseIf cbxtype.Text = "Class" Then

                Generate_Classfile()

            ElseIf cbxtype.Text = "Master_Single_Query" Then

            Else

            End If

1:



            'For irow = 0 To ds.Tables(0).Rows.Count - 1

            '    Dim slno = irow + 1
            '    '    Dim empcode = ds.Tables(0).Rows(irow)("empcode")


            '    Dtable.Rows.Add()
            '    Dtable.Rows(irows)("SNO") = slno
            '    Dtable.Rows(irows)("EmpCode") = mReport
            '    'Dtable.Rows(irows)("Name") = empname & vbCrLf & deptname


            '    irows = irows + 1





            'Next

            'Dtable.Rows.Add()
            ''     irows = irows + 1
            'Dtable.Rows(irows)("SNO") = ""
            'Dtable.Rows(irows)("EmpCode") = mReport


            'dgvReport.DataSource = Dtable
            'With dgvReport
            '    .RowHeadersVisible = False
            '    .Columns(0).HeaderCell.Value = "SNO" & vbCrLf & ""
            '    .Columns(1).HeaderCell.Value = "Emp.Code" & vbCrLf & ""

            'End With



        Catch ex As Exception
            Show_Message(ex.ToString())
        End Try
    End Sub

    Private Sub FrmCodeGenerate_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try

        Catch ex As Exception

        End Try
    End Sub

    Private Sub RichTextBox1_TextChanged(sender As Object, e As EventArgs) Handles Rtbox.TextChanged
        ''SELECT 'Create Proc sp_Save' + 'GST_PartyDebitNote_Main '
        ''+CHAR(13) + CHAR(10)+
        '( '
    End Sub

    Private Sub Btnsave_Click(sender As Object, e As EventArgs) Handles Btnsave.Click
        Dim saveDlg As SaveFileDialog = New SaveFileDialog()
        Dim filename As String = ""
        saveDlg.Filter = "Rich Text File (*.rtf)|*.rtf|Plain Text File (*.txt)|*.txt"
        saveDlg.DefaultExt = "*.rtf"
        saveDlg.FilterIndex = 1
        saveDlg.Title = "Save the contents"
        Dim retval As DialogResult = saveDlg.ShowDialog()

        If retval = DialogResult.OK Then
            filename = saveDlg.FileName
        Else
            Return
        End If

        Dim stream_type As RichTextBoxStreamType

        If saveDlg.FilterIndex = 2 Then
            stream_type = RichTextBoxStreamType.PlainText
        Else
            stream_type = RichTextBoxStreamType.RichText
        End If

        Rtbox.SaveFile(filename, stream_type)
    End Sub

    Private Sub Create_SQLMainScript()
        Try
            Rtbox.Text = ""
            Rtbox.Clear()

            ds = Nothing
            ds = New DataSet


            'ds = mobjclssalaryreport.GetEmpSalaryDetails_Report(selectedvalues, sex, ATM, msaldate, msaldate, mfilterdata, "GetEmp_SalaryAbstractnew_Details")

            Dim irows As Integer = 0

            Dim mReport As String = ""
            Dim nl As String = vbCrLf


            Dim Dtable As New DataTable
            Dtable.Columns.Add("SNO", Type.GetType("System.String"))
            Dtable.Columns.Add("EMPCODE", Type.GetType("System.String"))

            mReport = "USE [eGeneral]" & vbCrLf & " Go " & nl
            mReport &= "SET ANSI_NULLS ON" & nl & " Go " & nl
            mReport &= "SET QUOTED_IDENTIFIER ON" & nl & " Go " & nl
            mReport &= "--- =================================================================" & nl
            mReport &= "--- Author		    :	" & mvarUserID & nl
            mReport &= "--- Created Date	:	" & DateTime.Now & nl
            mReport &= "--- Author		    :	" & mvarUserID & nl
            mReport &= "--- Modified Date	:	" & DateTime.Now & nl
            mReport &= "--- Description	    :   " & " (Insert / Update)" & nl


            mReport &= "--- =================================================================" & nl
            mReport &= "---Grant Execute On  Sp_Save" & TxtTablename.Text & "  To Public " & nl
            mReport &= "---Select * from " & TxtTablename.Text & " " & nl
            mReport &= "---=================================================================" & nl
            mReport &= "Create Proc sp_Save" & TxtTablename.Text & nl & "(" & nl



            ds = Nothing
            ds = New DataSet
            SSQL = ""
            SSQL = " SELECT  '@'+column_name    as COl_Name"
            SSQL &= "   FROM INFORMATION_SCHEMA.Columns"
            SSQL &= " where TABLE_NAME = '" & TxtTablename.Text & "'"
            SSQL &= " and COLUMN_NAME not in ('Authorize','Delete_Mode','Created_By', 'Created_Date','Modified_By' ,'Modified_Date') "

            ds = ReturnMultipleValue(SSQL, mvarDbasename)
            If ds.Tables(0).Rows.Count <= 0 Then
            Else
                For irow = 0 To ds.Tables(0).Rows.Count - 1
                    Dim mDesc = ds.Tables(0).Rows(irow)("COl_Name")
                    mReport &= mDesc & "			   varchar(50)  ," & nl
                    Dtable.Rows.Add()
                    'Dtable.Rows(irows)("Name") = empname & vbCrLf & deptname
                Next
            End If
            mReport &= "@Authorize					Varchar(1)," & nl
            mReport &= "@Delete_Mode				Varchar(1)," & nl
            mReport &= "@Screen_Mode				Varchar(20)," & nl
            mReport &= "@User_Code					Varchar(15)," & nl
            mReport &= "@iSuccessFlag			    bit	output" & nl & " ) " & nl & " as " & nl

            mReport &= " set nocount on" & nl
            mReport &= " set dateformat ymd" & nl
            mReport &= " Declare @sqlstr			Varchar(8000)" & nl
            mReport &= " Declare @iMode			Numeric(18,0)" & nl
            mReport &= " Declare @iServerDate	Varchar(19)" & nl
            mReport &= " Set @iServerDate=(Select Convert(VARCHAR(19),getdate(),120))" & nl


            mReport &= " Set @iMode=(" & nl
            mReport &= " 		Select Count(Entry_No) from " & TxtTablename.Text & nl
            mReport &= " 		Where	Comp_Code		=	@Comp_Code " & nl
            mReport &= " 		And		Location_Code	=	@Location_Code " & nl
            mReport &= " 		And		Fin_Year_Code	=	@Fin_Year_Code " & nl
            mReport &= " 		And		Entry_No		=	@Entry_No " & nl
            mReport &= " 		And		Entry_date		=	@Entry_Date  " & nl
            mReport &= " 		)" & nl


            mReport &= " BEGIN TRAN TRAN1" & nl
            mReport &= " select @sqlstr=''" & nl
            mReport &= " IF @iMode<=0 " & nl
            mReport &= " BEGIN" & nl
            mReport &= " select @sqlstr = 'Insert into   " & TxtTablename.Text & "   Values (   ' " & nl


            ds = Nothing
            ds = New DataSet
            SSQL = ""
            SSQL = " SELECT  'select @sqlstr= @sqlstr + '',''+ '''' + ltrim(rtrim(@'+column_name +'		   ' +' ))				+ ''''' as COl_Name "
            SSQL &= "   FROM INFORMATION_SCHEMA.Columns"
            SSQL &= " where TABLE_NAME = '" & TxtTablename.Text & "'"
            SSQL &= " and COLUMN_NAME not in ('Authorize','Delete_Mode','Created_By', 'Created_Date','Modified_By' ,'Modified_Date') "

            ds = ReturnMultipleValue(SSQL, mvarDbasename)
            If ds.Tables(0).Rows.Count <= 0 Then
            Else
                For irow = 0 To ds.Tables(0).Rows.Count - 1
                    Dim mDesc = ds.Tables(0).Rows(irow)("COl_Name")
                    mReport &= mDesc & "		 " & nl
                    Dtable.Rows.Add()
                Next
            End If

            mReport &= " select @sqlstr= @sqlstr + ','+ '''' + ltrim(rtrim(@Authorize))					+ ''''" & nl
            mReport &= " select @sqlstr= @sqlstr + ','+ '''' + ltrim(rtrim(@Delete_Mode))				+ ''''" & nl
            mReport &= " select @sqlstr= @sqlstr + ','+ '''' + @User_Code								+ ''''" & nl
            mReport &= " select @sqlstr= @sqlstr + ','+ '''' + @iServerDate								+ '''' " & nl
            mReport &= " select @sqlstr= @sqlstr + ','+ '''' + @User_Code								+ ''''" & nl
            mReport &= " select @sqlstr= @sqlstr + ','+ '''' + @iServerDate								+ '''' + ')'" & nl
            mReport &= "                                     End" & nl
            mReport &= " ELSE IF @iMode=1 " & nl
            mReport &= " BEGIN " & nl


            mReport &= " select @sqlstr = 'Update  " & TxtTablename.Text & " Set '" & nl

            ds = Nothing
            ds = New DataSet
            SSQL = ""

            SSQL = " SELECT  '   select @sqlstr= @sqlstr + '',' +column_name     " '= 			+ '''' + '  as +  as "
            SSQL &= "  COl_Name "
            SSQL &= "  ,'@'+column_name    as Updat_Col  "



            SSQL &= "   FROM INFORMATION_SCHEMA.Columns"
            SSQL &= " where TABLE_NAME = '" & TxtTablename.Text & "'"
            SSQL &= " and COLUMN_NAME not in ('Comp_Code','Location_Code','Fin_Year_Code','Entry_No','Entry_Date','Authorize','Delete_Mode','Created_By', 'Created_Date','Modified_By' ,'Modified_Date') "

            ds = ReturnMultipleValue(SSQL, mvarDbasename)
            If ds.Tables(0).Rows.Count <= 0 Then
            Else
                For irow = 0 To ds.Tables(0).Rows.Count - 1
                    Dim mDesc = ds.Tables(0).Rows(irow)("COl_Name")
                    Dim mupdates = ds.Tables(0).Rows(irow)("Updat_Col")

                    mReport &= mDesc & "='   + '''' + ltrim(rtrim(" & mupdates & "))    + '''' " & nl
                    Dtable.Rows.Add()
                Next
            End If

            mReport &= " select @sqlstr= @sqlstr + ',Authorize ='			+ '''' + ltrim(rtrim(@Authorize))			+ '''' " & nl
            mReport &= " select @sqlstr= @sqlstr + ',Delete_Mode='			+ '''' + ltrim(rtrim(@Delete_Mode))			+ '''' " & nl
            mReport &= " select @sqlstr= @sqlstr + ',Modified_By='			+ '''' + ltrim(rtrim(@User_Code))			+ '''' " & nl
            mReport &= " select @sqlstr= @sqlstr + ',Modified_Date='		+ '''' + @iServerDate						+ '''' " & nl
            mReport &= " select @sqlstr= @sqlstr +  ' Where Comp_Code='		+ '''' + @Comp_Code							+ '''' " & nl
            mReport &= " select @sqlstr= @sqlstr +  ' And Location_Code='	+ '''' + @Location_Code						+ ''''  " & nl
            mReport &= " select @sqlstr= @sqlstr +  ' And Fin_Year_Code='	+ '''' + @Fin_Year_Code						+ ''''  " & nl
            mReport &= " select @sqlstr= @sqlstr +  ' And Entry_No='		+ '''' + @Entry_No							+ ''''  " & nl
            mReport &= " select @sqlstr= @sqlstr +  ' And Entry_Date='		+ '''' + @Entry_Date						+ ''''  " & nl
            mReport &= "  End" & nl
            mReport &= " exec(@sqlstr )" & nl
            mReport &= " print @sqlstr" & nl




            mReport &= " IF @iMode=1" & nl
            mReport &= "                BEGIN" & nl
            mReport &= " select @sqlstr = 'Delete from  " & TxtTablename.Text & "'" & nl
            mReport &= " select @sqlstr= @sqlstr +  ' Where Comp_Code='		+ '''' + @Comp_Code			+ ''''" & nl
            mReport &= " select @sqlstr= @sqlstr +  ' And Location_Code='	+ '''' + @Location_Code		+ ''''  " & nl
            mReport &= " select @sqlstr= @sqlstr +  ' And Fin_Year_Code='	+ '''' + @Fin_Year_Code		+ ''''  " & nl
            mReport &= " select @sqlstr= @sqlstr +  ' And Entry_No='		+ '''' + @Entry_No		+ ''''  " & nl
            mReport &= " select @sqlstr= @sqlstr +  ' And Entry_Date='	+ '''' + @Entry_Date	+ ''''  " & nl
            mReport &= " exec(@sqlstr )" & nl
            mReport &= "  End" & nl

            mReport &= " IF (@@error <> 0) goto ErrHand " & nl
            mReport &= " COMMIT TRAN TRAN1 " & nl
            mReport &= " select @iSuccessFlag=1 " & nl
            mReport &= " Return " & nl
            mReport &= " ErrHand: " & nl
            mReport &= "        BEGIN " & nl
            mReport &= " ROLLBACK TRAN TRAN1 " & nl
            mReport &= " select @iSuccessFlag=0 " & nl
            mReport &= " End " & nl






            Rtbox.Text = mReport


        Catch ex As Exception
            Show_Message(ex.Message)

        End Try
    End Sub

    Private Sub Create_SQLLineScript()
        Try
            Rtbox.Text = ""
            Rtbox.Clear()

            ds = Nothing
            ds = New DataSet


            'ds = mobjclssalaryreport.GetEmpSalaryDetails_Report(selectedvalues, sex, ATM, msaldate, msaldate, mfilterdata, "GetEmp_SalaryAbstractnew_Details")

            Dim irows As Integer = 0

            Dim mReport As String = ""
            Dim nl As String = vbCrLf





            mReport = "USE [eGeneral]" & vbCrLf & " Go " & nl
            mReport &= "SET ANSI_NULLS ON" & nl & " Go " & nl
            mReport &= "SET QUOTED_IDENTIFIER ON" & nl & " Go " & nl
            mReport &= "--- =================================================================" & nl
            mReport &= "--- Author		    :	" & mvarUserID & nl
            mReport &= "--- Created Date	:	" & DateTime.Now & nl
            mReport &= "--- Author		    :	" & mvarUserID & nl
            mReport &= "--- Modified Date	:	" & DateTime.Now & nl
            mReport &= "--- Description	    :   " & " Save" & TxtTablename.Text & " (Insert / Update)" & nl


            mReport &= "--- =================================================================" & nl
            mReport &= "---Grant Execute On  Sp_Save" & TxtTablename.Text & "  To Public " & nl
            mReport &= "---Select * from " & TxtTablename.Text & " " & nl
            mReport &= "---=================================================================" & nl
            mReport &= "Create Proc sp_Save" & TxtTablename.Text & nl & "(" & nl





            ds = Nothing
            ds = New DataSet
            SSQL = ""
            SSQL = " SELECT  '@'+column_name    as COl_Name"
            SSQL &= "   FROM INFORMATION_SCHEMA.Columns"
            SSQL &= " where TABLE_NAME = '" & TxtTablename.Text & "'"
            SSQL &= " and COLUMN_NAME not in ('Authorize','Delete_Mode','Created_By', 'Created_Date','Modified_By' ,'Modified_Date') "

            ds = ReturnMultipleValue(SSQL, mvarDbasename)
            If ds.Tables(0).Rows.Count <= 0 Then
            Else
                For irow = 0 To ds.Tables(0).Rows.Count - 1
                    Dim mDesc = ds.Tables(0).Rows(irow)("COl_Name")
                    mReport &= mDesc & "               varchar(50)  ," & nl

                    'Dtable.Rows(irows)("Name") = empname & vbCrLf & deptname
                Next
            End If
            mReport &= "@Authorize					Varchar(1)," & nl
            mReport &= "@Delete_Mode				Varchar(1)," & nl
            mReport &= "@Screen_Mode				Varchar(20)," & nl
            mReport &= "@User_Code					Varchar(15)," & nl
            mReport &= "@iSuccessFlag			    bit	output" & nl & " ) " & nl & " as " & nl

            mReport &= " set nocount on" & nl
            mReport &= " set dateformat ymd" & nl
            mReport &= " Declare @sqlstr			Varchar(8000)" & nl

            mReport &= " Declare @iServerDate	Varchar(19)" & nl
            mReport &= " Set @iServerDate=(Select Convert(VARCHAR(19),getdate(),120))" & nl



            mReport &= " BEGIN TRAN TRAN1" & nl

            mReport &= " select @sqlstr=''  "

            mReport &= " select @sqlstr = 'Insert into   " & TxtTablename.Text & "   Values (   ' " & nl


            ds = Nothing
            ds = New DataSet
            SSQL = ""
            'SSQL = " SELECT  'select @sqlstr= @sqlstr + '',''+ '''' + ltrim(rtrim(@'+column_name +'		   ' +' ))				+ ''''' as COl_Name "

            SSQL = " SELECT  ' @'+column_name     as  COl_Name"
            SSQL &= "   FROM INFORMATION_SCHEMA.Columns"
            SSQL &= " where TABLE_NAME = '" & TxtTablename.Text & "'"
            SSQL &= " and COLUMN_NAME not in ('Authorize','Delete_Mode','Created_By', 'Created_Date','Modified_By' ,'Modified_Date') "

            ds = ReturnMultipleValue(SSQL, mvarDbasename)
            If ds.Tables(0).Rows.Count <= 0 Then
            Else
                For irow = 0 To ds.Tables(0).Rows.Count - 1
                    Dim mDesc = ds.Tables(0).Rows(irow)("COl_Name")

                    mReport &= " select @sqlstr= @sqlstr + ','+ '''' + ltrim(rtrim( " & mDesc & "))			+ ''''" & nl

                    'mReport &= mDesc & "		 " & nl

                Next
            End If

            mReport &= " select @sqlstr= @sqlstr + ','+ '''' + ltrim(rtrim(@Authorize))			+ ''''" & nl
            mReport &= " select @sqlstr= @sqlstr + ','+ '''' + ltrim(rtrim(@Delete_Mode))		+ ''''" & nl
            mReport &= " select @sqlstr= @sqlstr + ','+ '''' + @User_Code						+ ''''" & nl
            mReport &= " select @sqlstr= @sqlstr + ','+ '''' + @iServerDate						+ '''' " & nl
            mReport &= " select @sqlstr= @sqlstr + ','+ '''' + @User_Code						+ ''''" & nl
            mReport &= " select @sqlstr= @sqlstr + ','+ '''' + @iServerDate						+ '''' + ')'" & nl
            mReport &= " exec(@sqlstr )" & nl
            mReport &= " print @sqlstr" & nl
            mReport &= " IF (@@error <> 0) goto ErrHand " & nl
            mReport &= " COMMIT TRAN TRAN1 " & nl
            mReport &= " select @iSuccessFlag=1 " & nl
            mReport &= " Return " & nl
            mReport &= " ErrHand: " & nl
            mReport &= "        BEGIN " & nl
            mReport &= " ROLLBACK TRAN TRAN1 " & nl
            mReport &= " select @iSuccessFlag=0 " & nl
            mReport &= " End " & nl

            Rtbox.Text = mReport


        Catch ex As Exception
            Show_Message(ex.Message)

        End Try
    End Sub
    Private Sub Create_SQLGetallScript()
        Try
            Rtbox.Text = ""
            Rtbox.Clear()

            ds = Nothing
            ds = New DataSet

            Dim irows As Integer = 0

            Dim mReport As String = ""
            Dim nl As String = vbCrLf
            mReport = "USE [eGeneral]" & vbCrLf & " Go " & nl
            mReport &= "SET ANSI_NULLS ON" & nl & " Go " & nl
            mReport &= "SET QUOTED_IDENTIFIER ON" & nl & " Go " & nl
            mReport &= "--- =================================================================" & nl
            mReport &= "--- Author		    :	" & mvarUserID & nl
            mReport &= "--- Created Date	:	" & DateTime.Now & nl
            mReport &= "--- Author		    :	" & mvarUserID & nl
            mReport &= "--- Modified Date	:	" & DateTime.Now & nl
            mReport &= "--- Description	    :   " & " Get" & TxtTablename.Text & " (Insert / Update)" & nl
            mReport &= "--- =================================================================" & nl
            mReport &= "---Grant Execute On  sp_Get" & TxtTablename.Text & "_Details     To Public " & nl
            mReport &= "---Select * from " & TxtTablename.Text & " " & nl
            mReport &= "---=================================================================" & nl
            mReport &= "Create Proc sp_Get" & TxtTablename.Text & "_Details" & nl & "(" & nl




            mReport &= "@Comp_Code					Varchar(20)," & nl
            mReport &= "@Location_Code				Varchar(20)," & nl
            mReport &= "@Fin_Year_Code				Varchar(5)," & nl
            mReport &= "@Entry_No					Varchar(15)," & nl
            mReport &= "@Authorize					Varchar(1)," & nl
            mReport &= "@Delete_Mode					Varchar(1)," & nl
            mReport &= "@Filter_Data					Varchar(150)" & nl & " ) " & nl & " as " & nl

            mReport &= " BEGIN  " & nl


            mReport &= " set nocount on" & nl
            mReport &= " set dateformat ymd" & nl
            mReport &= " Declare @sqlstr			Varchar(8000)" & nl

            mReport &= "        IF " & nl
            mReport &= "(	" & nl
            mReport &= "ltrim(rtrim(@Entry_No))= '' " & nl
            mReport &= "and ltrim(rtrim(@Authorize))= '' " & nl
            mReport &= "and ltrim(rtrim(@Delete_Mode))=''" & nl
            mReport &= "and LTRIM(rtrim(@Filter_Data))='ALL'" & nl

            mReport &= ")" & nl
            mReport &= "               BEGIN" & nl
            mReport &= "select @sqlstr= 'Select '" & nl
            mReport &= "select @sqlstr = @sqlstr + '            '         " & nl
            mReport &= "select @sqlstr = @sqlstr + '            '         " & nl
            mReport &= "select @sqlstr = @sqlstr + '            '         " & nl
            mReport &= "select @sqlstr = @sqlstr + '            '         " & nl
            mReport &= "select @sqlstr = @sqlstr + '            '         " & nl
            mReport &= "select @sqlstr = @sqlstr + '            '         " & nl
            mReport &= "select @sqlstr = @sqlstr + '            '         " & nl




            mReport &= " select @sqlstr = @sqlstr + ' from " + TxtTablename.Text + "  as a '" & nl
            mReport &= " select @sqlstr = @sqlstr + ' Where a.Comp_Code='			+ '''' + @Comp_Code			+ ''''" & nl
            mReport &= " select @sqlstr = @sqlstr + ' And a.Location_Code='			+ '''' + @Location_Code		+ ''''" & nl
            mReport &= " select @sqlstr = @sqlstr + ' And a.Fin_Year_Code='			+ '''' + @Fin_Year_Code		+ ''''" & nl
            mReport &= " select @sqlstr = @sqlstr + ' And a.Delete_Mode=0'" & nl
            mReport &= " select @sqlstr = @sqlstr + ' Order by a.Created_Date, a.Entry_Date, a.Entry_No'" & nl
            mReport &= "           End " & nl


            mReport &= "           ELSE IF " & nl
            mReport &= " (" & nl
            mReport &= "ltrim(rtrim(@Entry_No))= '' " & nl
            mReport &= "and ltrim(rtrim(@Authorize))= '1' " & nl
            mReport &= "and ltrim(rtrim(@Delete_Mode))=''" & nl
            mReport &= "and LTRIM(rtrim(@Filter_Data))='ALL'" & nl

            mReport &= " )" & nl
            mReport &= "             BEGIN " & nl

            mReport &= " select @sqlstr= 'Select '" & nl
            mReport &= "select @sqlstr = @sqlstr + '            '         " & nl
            mReport &= "select @sqlstr = @sqlstr + '            '         " & nl
            mReport &= "select @sqlstr = @sqlstr + '            '         " & nl
            mReport &= "select @sqlstr = @sqlstr + '            '         " & nl


            mReport &= " select @sqlstr = @sqlstr + ' from " + TxtTablename.Text + "  as a '" & nl
            mReport &= " select @sqlstr = @sqlstr + ' Where a.Comp_Code='			+ '''' + @Comp_Code			+ ''''" & nl
            mReport &= " select @sqlstr = @sqlstr + ' And a.Location_Code='			+ '''' + @Location_Code		+ ''''" & nl
            mReport &= " select @sqlstr = @sqlstr + ' And a.Fin_Year_Code='			+ '''' + @Fin_Year_Code		+ ''''" & nl
            mReport &= " select @sqlstr = @sqlstr + ' And a.Delete_Mode=0'" & nl
            mReport &= " select @sqlstr = @sqlstr + ' Order by a.Created_Date, a.Entry_Date, a.Entry_No'" & nl


            mReport &= "           End " & nl
            mReport &= "           ELSE IF " & nl
            mReport &= " (" & nl
            mReport &= " ltrim(rtrim(@Entry_No)) <>''" & nl
            mReport &= " and ltrim(rtrim(@Authorize))= '' " & nl
            mReport &= " and ltrim(rtrim(@Delete_Mode))=''" & nl
            mReport &= " and LTRIM(rtrim(@Filter_Data))='SPECIFY'" & nl

            mReport &= " )" & nl
            mReport &= "             BEGIN " & nl

            mReport &= " select @sqlstr= 'Select '" & nl
            mReport &= "select @sqlstr = @sqlstr + '            '         " & nl
            mReport &= "select @sqlstr = @sqlstr + '            '         " & nl
            mReport &= "select @sqlstr = @sqlstr + '            '         " & nl
            mReport &= "select @sqlstr = @sqlstr + '            '         " & nl


            mReport &= " select @sqlstr = @sqlstr + ' from " + TxtTablename.Text + "  as a '" & nl
            mReport &= " select @sqlstr = @sqlstr + ' Where a.Comp_Code='			+ '''' + @Comp_Code			+ ''''" & nl
            mReport &= " select @sqlstr = @sqlstr + ' And a.Location_Code='			+ '''' + @Location_Code		+ ''''" & nl
            mReport &= " select @sqlstr = @sqlstr + ' And a.Fin_Year_Code='			+ '''' + @Fin_Year_Code		+ ''''" & nl
            mReport &= " select @sqlstr = @sqlstr + ' And a.Entry_No='			+ '''' + @Entry_No		+ ''''" & nl

            mReport &= " select @sqlstr = @sqlstr + ' And a.Delete_Mode=0'" & nl
            mReport &= " select @sqlstr = @sqlstr + ' Order by a.Created_Date, a.Entry_Date, a.Entry_No'" & nl



            mReport &= "   End" & nl
            mReport &= " print (@sqlstr)" & nl
            mReport &= " exec(@sqlstr)" & nl
            mReport &= "         End" & nl



            Rtbox.Text = mReport


        Catch ex As Exception
            Show_Message(ex.Message)

        End Try
    End Sub

    Private Sub Generate_Classfile()
        Try
            Rtbox.Text = ""
            Rtbox.Clear()

            ds = Nothing
            ds = New DataSet

            Dim irows As Integer = 0

            Dim mReport As String = ""
            Dim nl As String = vbCrLf











            mReport = " " & vbCrLf & "   " & nl


            mReport &= " Option Explicit On" & nl

            mReport &= "#Region """ + "Imports" + """ " & nl
            mReport &= "Imports System" & nl
            mReport &= "Imports System.Data" & nl
            mReport &= "Imports System.Data.OleDb" & nl
            mReport &= "Imports C1.Win.C1FlexGrid.Classic" & nl
            mReport &= "Imports System.Data.SqlClient" & nl

            mReport &= "#End Region" & nl





            mReport &= "Public Class Cls_" & TxtTablename.Text & "_Details" & nl
            mReport &= "Public Function Get" & TxtTablename.Text & "_Details" + "(ByVal mvarvarietyCode As String, ByVal mvarFilterData As String)" & nl
            mReport &= "Try" & nl
            mReport &= "com = New OleDbCommand" & nl
            mReport &= "ds = New DataSet" & nl
            mReport &= "da = New OleDbDataAdapter" & nl
            mReport &= "con = New OleDbConnection(GetConnection(""" + "eCotton """ + "))" & nl
            mReport &= "Try" & nl
            mReport &= "com.CommandText = """ + "sp_Get" & TxtTablename.Text & "_Details" & nl
            mReport &= "com.CommandType = CommandType.StoredProcedure" & nl
            mReport &= "com.Connection = con" & nl
            mReport &= "com.Parameters.Add(""" + "@Comp_Code""" + ", OleDbType.VarChar, 10).Value = mvarCompanyCode" & nl
            mReport &= "com.Parameters.Add(""" + "@Location_Code""" + ", OleDbType.VarChar, 100).Value = mvarLocationCode" & nl
            mReport &= "com.Parameters.Add(""" + "@Fin_Year_Code""" + ", OleDbType.VarChar, 100).Value = mvarFinYearCode" & nl
            mReport &= "com.Parameters.Add(""" + "@Code""" + ", OleDbType.VarChar, 100).Value = mvarvarietyCode" & nl
            mReport &= "com.Parameters.Add(""" + "@Filter_Data""" + ", OleDbType.VarChar, 100).Value = mvarFilterData" & nl

            mReport &= "da = New OleDbDataAdapter(com)" & nl
            mReport &= "da.Fill(ds)" & nl
            mReport &= "da.Dispose()" & nl
            mReport &= "com.Dispose()" & nl
            mReport &= "con.Dispose()" & nl
            mReport &= "Catch ex As Exception" & nl
            mReport &= "Show_Message(ex.Message)" & nl
            mReport &= "da.Dispose()" & nl
            mReport &= "com.Dispose()" & nl
            mReport &= "con.Dispose()" & nl
            mReport &= "End Try" & nl
            mReport &= "Return ds" & nl
            mReport &= "Catch ex As Exception" & nl
            mReport &= "Throw New Exception(ex.Message)" & nl
            mReport &= "End Try" & nl
            mReport &= "Return ds" & nl

            mReport &= "End Sub" & nl








            mReport &= "Public Function SaveGSTpartyDebitNoteMain(ByVal mSaveString As String, ByVal mScreenMode As String, _" & nl
            mReport &= "                               ByVal mobjGrid As C1FlexGridClassic, _" & nl
            mReport &= "                           ByVal mNumberType As String) As String" & nl
            mReport &= "  Dim mInputParam As String() = Split(mSaveString, """ + "|| """ + ")" & nl
            mReport &= "   Dim SuccessFlag As Integer = 0" & nl
            mReport &= "   Dim mInvEntryNo As String = """ & nl

            mReport &= "   If mInputParam(0).Trim = "" Then" & nl
            mReport &= " mInvEntryNo = GetAutoNumber(1, mNumberType, """ + "GST Debit Note(Party)""" + ")" & nl
            mReport &= "   Else" & nl
            mReport &= "  mInvEntryNo = mInputParam(0)" & nl
            mReport &= "   End If" & nl
            mReport &= "   If Trim(mInvEntryNo) = "" Or mobjGrid.Rows <= 0 Then" & nl
            mReport &= "Return """ + "Error""" + "" & nl
            mReport &= " Exit Sub" & nl
            mReport &= "  End If" & nl
            mReport &= "   com = New OleDbCommand" & nl
            mReport &= "  con = New OleDbConnection(GetConnection(""" + "eGeneral""" + "))" & nl
            mReport &= "  con.Open()" & nl
            mReport &= "   com.Connection = con" & nl
            mReport &= "  Try" & nl
            mReport &= "com.CommandText = """ + "sp_SaveGSTPartyDebitNote_Main" & nl
            mReport &= "  com.CommandType = CommandType.StoredProcedure" & nl
            mReport &= "   com.Connection = con"
            mReport &= "      com.Parameters.Add(""" + "@Comp_Code """ + ", OleDbType.VarChar, 10).Value = mvarCompanyCode " & nl
            mReport &= "       com.Parameters.Add(""" + "@Location_Code """ + ", OleDbType.VarChar, 10).Value = mvarLocationCode " & nl
            mReport &= "       com.Parameters.Add(""" + "@Fin_Year_Code """ + ", OleDbType.VarChar, 10).Value = mvarFinYearCode " & nl

            mReport &= "      com.Parameters.Add(""" + "@Entry_No""" + ", OleDbType.VarChar, 100).Value = mInvEntryNo " & nl
            mReport &= "      com.Parameters.Add(""" + "@Entry_Date""" + ", OleDbType.VarChar, 10).Value = mInputParam(1) " & nl


            ds = Nothing
            ds = New DataSet
            SSQL = ""
            SSQL = " SELECT  'com.Parameters.Add(""" + "@'+column_name + '""" + " , OleDbType.VarChar, 200).Value =' as COl_Name "
            SSQL &= "   FROM INFORMATION_SCHEMA.Columns"
            SSQL &= " where TABLE_NAME = '" & TxtTablename.Text & "'"
            SSQL &= " and COLUMN_NAME not in ('Authorize','Delete_Mode','Created_By', 'Created_Date','Modified_By' ,'Modified_Date','Comp_Code','Location_Code','Fin_Year_Code','Entry_No','Entry_Date') "

            ds = ReturnMultipleValue(SSQL, mvarDbasename)
            Dim mmParam As Integer = 2
            If ds.Tables(0).Rows.Count <= 0 Then
            Else
                For irow = 0 To ds.Tables(0).Rows.Count - 1
                    Dim mDesc = ds.Tables(0).Rows(irow)("COl_Name")
                    Dim mesc = "mInputParam(" & mmParam & ")"

                    mReport &= mDesc & "		 " & "mInputParam(" & mmParam & ")" & nl
                    mmParam += 1
                Next
            End If








      
            'com.Parameters.Add("@Credit_Code", OleDbType.VarChar, 200).Value = mInputParam(2)
            'com.Parameters.Add("@Credit_To", OleDbType.VarChar, 200).Value = mInputParam(3)
            'com.Parameters.Add("@Debit_To", OleDbType.VarChar, 200).Value = mInputParam(4)
            'com.Parameters.Add("@Party_Credit_No", OleDbType.VarChar, 100).Value = mInputParam(5)
            'com.Parameters.Add("@Party_Credit_Date", OleDbType.VarChar, 10).Value = mInputParam(6)
            'com.Parameters.Add("@Ref_Doc_No", OleDbType.VarChar, 20).Value = mInputParam(7)
            'com.Parameters.Add("@Ref_Doc_Date", OleDbType.VarChar, 50).Value = mInputParam(8)
            'com.Parameters.Add("@Rev_Charge", OleDbType.VarChar, 50).Value = ""
            'com.Parameters.Add("@Other_Instructions", OleDbType.VarChar, 1000).Value = mInputParam(9)
            'com.Parameters.Add("@Tot_Amount", OleDbType.VarChar, 100).Value = mInputParam(10)
            'com.Parameters.Add("@gst_pers", OleDbType.VarChar, 100).Value = mInputParam(11)
            'com.Parameters.Add("@cgst", OleDbType.VarChar, 100).Value = mInputParam(12)
            'com.Parameters.Add("@sgst", OleDbType.VarChar, 100).Value = mInputParam(13)
            'com.Parameters.Add("@igst", OleDbType.VarChar, 100).Value = mInputParam(14)
            'com.Parameters.Add("@Tot_GST", OleDbType.VarChar, 100).Value = mInputParam(15)
            'com.Parameters.Add("@TCS_Pers", OleDbType.VarChar, 100).Value = mInputParam(16)
            'com.Parameters.Add("@Tcs_Amt", OleDbType.VarChar, 100).Value = mInputParam(17)
            'com.Parameters.Add("@Total_Gross", OleDbType.VarChar, 20).Value = mInputParam(18)
            'com.Parameters.Add("@Round_Off", OleDbType.VarChar, 20).Value = mInputParam(19)
            'com.Parameters.Add("@Bill_Amount", OleDbType.VarChar, 20).Value = mInputParam(20)
            'com.Parameters.Add("@TDS_Pers", OleDbType.VarChar, 20).Value = mInputParam(21)
            'com.Parameters.Add("@TDS_Val", OleDbType.VarChar, 20).Value = mInputParam(22)
            'com.Parameters.Add("@Tot_net_Amt", OleDbType.VarChar, 20).Value = mInputParam(23)



            mReport &= "    com.Parameters.Add(""" + "@Authorize """ + ", OleDbType.VarChar, 1).Value = """ + "0 """ + "" & nl
            mReport &= "    com.Parameters.Add(""" + "@Delete_Mode """ + ", OleDbType.VarChar, 1).Value = """ + "0 """ + "" & nl
            mReport &= "    com.Parameters.Add(""" + "@Screen_Mode """ + ", OleDbType.VarChar, 20).Value = mScreenMode  " & nl
            mReport &= "    com.Parameters.Add(""" + "@User_Code """ + ", OleDbType.VarChar, 15).Value = mvarUserID   " & nl
            mReport &= "    Dim OutParam As OleDbParameter = New OleDbParameter(""" + "@iSuccessFlag """ + ", OleDbType.Integer)" & nl
            mReport &= "    OutParam.Direction = ParameterDirection.Output" & nl
            mReport &= "   com.Parameters.Add(OutParam)" & nl
            mReport &= "    com.ExecuteNonQuery()" & nl
            mReport &= "     If com.Parameters(""" + "@iSuccessFlag """ + ").Value = 1 Then" & nl
            mReport &= "SuccessFlag = 0" & nl
            mReport &= "     SuccessFlag = SaveGSTpartyDebitNoteLine(mInvEntryNo, mInputParam(1), mSaveString, mobjGrid, mScreenMode)" & nl
            mReport &= "      If SuccessFlag = 0 Then" & nl
            mReport &= " """ + "Error" & nl
            mReport &= "     End If" & nl
            mReport &= "     Else" & nl
            mReport &= "     Return """ + "Error" & nl
            mReport &= "      End If" & nl
            mReport &= "  Catch ex As Exception" & nl
            mReport &= "      Show_Message(""" + " Commit Exception Type: {0}  """ + " , ex.GetType.ToString())" & nl
            mReport &= "      Show_Message(""" + "  Message: {0}""" + ", ex.Message)" & nl
            mReport &= "     da.Dispose()" & nl
            mReport &= "     com.Dispose()" & nl
            mReport &= "     con.Dispose()" & nl
            mReport &= "  End Try" & nl
            mReport &= "Return mInvEntryNo" & nl
            mReport &= "End Sub" & nl & nl & nl & nl
            mReport &= "   "





            mReport &= "SET ANSI_NULLS ON" & nl & " Go " & nl
            mReport &= "SET QUOTED_IDENTIFIER ON" & nl & " Go " & nl
            mReport &= "--- =================================================================" & nl
            mReport &= "--- Author		    :	" & mvarUserID & nl
            mReport &= "--- Created Date	:	" & DateTime.Now & nl
            mReport &= "--- Author		    :	" & mvarUserID & nl
            mReport &= "--- Modified Date	:	" & DateTime.Now & nl
            mReport &= "--- Description	    :   " & " Get" & TxtTablename.Text & " (Insert / Update)" & nl
            mReport &= "--- =================================================================" & nl
            mReport &= "---Grant Execute On  sp_Get" & TxtTablename.Text & "_Details     To Public " & nl
            mReport &= "---Select * from " & TxtTablename.Text & " " & nl
            mReport &= "---=================================================================" & nl
            mReport &= "Create Proc sp_Get" & TxtTablename.Text & "_Details" & nl & "(" & nl




            mReport &= "@Comp_Code					Varchar(20)," & nl
            mReport &= "@Location_Code				Varchar(20)," & nl
            mReport &= "@Fin_Year_Code				Varchar(5)," & nl
            mReport &= "@Entry_No					Varchar(15)," & nl
            mReport &= "@Authorize					Varchar(1)," & nl
            mReport &= "@Delete_Mode					Varchar(1)," & nl
            mReport &= "@Filter_Data					Varchar(150)" & nl & " ) " & nl & " as " & nl

            mReport &= " BEGIN  " & nl


            mReport &= " set nocount on" & nl
            mReport &= " set dateformat ymd" & nl
            mReport &= " Declare @sqlstr			Varchar(8000)" & nl

            mReport &= "        IF " & nl
            mReport &= "(	" & nl
            mReport &= "ltrim(rtrim(@Entry_No))= '' " & nl
            mReport &= "and ltrim(rtrim(@Authorize))= '' " & nl
            mReport &= "and ltrim(rtrim(@Delete_Mode))=''" & nl
            mReport &= "and LTRIM(rtrim(@Filter_Data))='ALL'" & nl

            mReport &= ")" & nl
            mReport &= "               BEGIN" & nl
            mReport &= "select @sqlstr= 'Select '" & nl
            mReport &= "select @sqlstr = @sqlstr + '            '         " & nl
            mReport &= "select @sqlstr = @sqlstr + '            '         " & nl
            mReport &= "select @sqlstr = @sqlstr + '            '         " & nl
            mReport &= "select @sqlstr = @sqlstr + '            '         " & nl
            mReport &= "select @sqlstr = @sqlstr + '            '         " & nl
            mReport &= "select @sqlstr = @sqlstr + '            '         " & nl
            mReport &= "select @sqlstr = @sqlstr + '            '         " & nl




            mReport &= " select @sqlstr = @sqlstr + ' from " + TxtTablename.Text + "  as a '" & nl
            mReport &= " select @sqlstr = @sqlstr + ' Where a.Comp_Code='			+ '''' + @Comp_Code			+ ''''" & nl
            mReport &= " select @sqlstr = @sqlstr + ' And a.Location_Code='			+ '''' + @Location_Code		+ ''''" & nl
            mReport &= " select @sqlstr = @sqlstr + ' And a.Fin_Year_Code='			+ '''' + @Fin_Year_Code		+ ''''" & nl
            mReport &= " select @sqlstr = @sqlstr + ' And a.Delete_Mode=0'" & nl
            mReport &= " select @sqlstr = @sqlstr + ' Order by a.Created_Date, a.Entry_Date, a.Entry_No'" & nl
            mReport &= "           End " & nl


            mReport &= "           ELSE IF " & nl
            mReport &= " (" & nl
            mReport &= "ltrim(rtrim(@Entry_No))= '' " & nl
            mReport &= "and ltrim(rtrim(@Authorize))= '1' " & nl
            mReport &= "and ltrim(rtrim(@Delete_Mode))=''" & nl
            mReport &= "and LTRIM(rtrim(@Filter_Data))='ALL'" & nl

            mReport &= " )" & nl
            mReport &= "             BEGIN " & nl

            mReport &= " select @sqlstr= 'Select '" & nl
            mReport &= "select @sqlstr = @sqlstr + '            '         " & nl
            mReport &= "select @sqlstr = @sqlstr + '            '         " & nl
            mReport &= "select @sqlstr = @sqlstr + '            '         " & nl
            mReport &= "select @sqlstr = @sqlstr + '            '         " & nl


            mReport &= " select @sqlstr = @sqlstr + ' from " + TxtTablename.Text + "  as a '" & nl
            mReport &= " select @sqlstr = @sqlstr + ' Where a.Comp_Code='			+ '''' + @Comp_Code			+ ''''" & nl
            mReport &= " select @sqlstr = @sqlstr + ' And a.Location_Code='			+ '''' + @Location_Code		+ ''''" & nl
            mReport &= " select @sqlstr = @sqlstr + ' And a.Fin_Year_Code='			+ '''' + @Fin_Year_Code		+ ''''" & nl
            mReport &= " select @sqlstr = @sqlstr + ' And a.Delete_Mode=0'" & nl
            mReport &= " select @sqlstr = @sqlstr + ' Order by a.Created_Date, a.Entry_Date, a.Entry_No'" & nl


            mReport &= "           End " & nl
            mReport &= "           ELSE IF " & nl
            mReport &= " (" & nl
            mReport &= " ltrim(rtrim(@Entry_No)) <>''" & nl
            mReport &= " and ltrim(rtrim(@Authorize))= '' " & nl
            mReport &= " and ltrim(rtrim(@Delete_Mode))=''" & nl
            mReport &= " and LTRIM(rtrim(@Filter_Data))='SPECIFY'" & nl

            mReport &= " )" & nl
            mReport &= "             BEGIN " & nl

            mReport &= " select @sqlstr= 'Select '" & nl
            mReport &= "select @sqlstr = @sqlstr + '            '         " & nl
            mReport &= "select @sqlstr = @sqlstr + '            '         " & nl
            mReport &= "select @sqlstr = @sqlstr + '            '         " & nl
            mReport &= "select @sqlstr = @sqlstr + '            '         " & nl


            mReport &= " select @sqlstr = @sqlstr + ' from " + TxtTablename.Text + "  as a '" & nl
            mReport &= " select @sqlstr = @sqlstr + ' Where a.Comp_Code='			+ '''' + @Comp_Code			+ ''''" & nl
            mReport &= " select @sqlstr = @sqlstr + ' And a.Location_Code='			+ '''' + @Location_Code		+ ''''" & nl
            mReport &= " select @sqlstr = @sqlstr + ' And a.Fin_Year_Code='			+ '''' + @Fin_Year_Code		+ ''''" & nl
            mReport &= " select @sqlstr = @sqlstr + ' And a.Entry_No='			+ '''' + @Entry_No		+ ''''" & nl

            mReport &= " select @sqlstr = @sqlstr + ' And a.Delete_Mode=0'" & nl
            mReport &= " select @sqlstr = @sqlstr + ' Order by a.Created_Date, a.Entry_Date, a.Entry_No'" & nl



            mReport &= "   End" & nl
            mReport &= " print (@sqlstr)" & nl
            mReport &= " exec(@sqlstr)" & nl
            mReport &= "         End" & nl



            Rtbox.Text = mReport


        Catch ex As Exception
            Show_Message(ex.Message)

        End Try
    End Sub






End Class