Option Explicit Off
Imports System.Windows.Interop
Imports System.Data.SqlClient
Imports System.IO
Imports System.Threading
Imports System.ComponentModel

Class MainWindow
    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs) Handles MyBase.Loaded
        ExtendGlass()
        GridUser.Visibility = Windows.Visibility.Hidden
        gridbatchctrl.Visibility = Windows.Visibility.Hidden
        combobox1.IsEnabled = False
        ButtonL4.IsEnabled = False
        bwload.RunWorkerAsync()
    End Sub
    Protected Overrides Sub OnMouseLeftButtonDown(e As MouseButtonEventArgs)
        MyBase.OnMouseLeftButtonDown(e)
        Me.DragMove()
    End Sub
    Private Declare Sub DwmIsCompositionEnabled Lib "dwmapi.dll" (ByRef b As Boolean)
    Private Declare Sub DwmExtendFrameIntoClientArea Lib "dwmapi.dll" (ByVal hWnd As IntPtr, ByRef pMarInset As Margins)
    Private Structure Margins
        Public Sub New(ByVal t As Thickness)
            Left = CInt(t.Left)
            Right = CInt(t.Right)
            Top = CInt(t.Top)
            Bottom = CInt(t.Bottom)
        End Sub
        Public Left As Integer
        Public Right As Integer
        Public Top As Integer
        Public Bottom As Integer
    End Structure
    Private Sub ExtendGlass()
        Try
            Dim b As Boolean
            DwmIsCompositionEnabled(b)
            If b Then
                Dim hWnd As IntPtr = New WindowInteropHelper(Me).Handle
                If hWnd <> IntPtr.Zero Then
                    Me.Background = Brushes.Transparent
                    HwndSource.FromHwnd(hWnd).CompositionTarget.BackgroundColor = Colors.Transparent
                    Dim m As New Margins(New Thickness(-1))
                    DwmExtendFrameIntoClientArea(hWnd, m)
                End If
            End If
        Catch
        End Try
    End Sub

    Dim lesson As String
    Dim score0 As String
    Dim score1 As String
    Dim score2 As String
    Dim score3 As String
    Dim score4 As String
    Dim time As String
    Dim testmode As String
    Dim dataet As String
    Dim username As String
    Dim password As String
    Dim cookielogin As String
    Dim accountid As String
    Dim accountidtmp As String
    Dim aspsession As String
    Dim ecpacidtmp() As String
    Dim ecpacid As String
    Dim data As String
    Public cookiesend As String
    Dim isall As String = 0
    WithEvents bw As New BackgroundWorker()
    WithEvents bwuser As New BackgroundWorker()
    WithEvents bwload As New BackgroundWorker()
    WithEvents bwall As New BackgroundWorker()
    Dim progress As Double = 0
    Dim progressa As Double = 0
    Dim timewait As Integer
    Dim num As Integer
    Dim numa As Integer
    Public isloggedin As Boolean = False
    Public isbwbusy As Boolean = False
    Dim olesson As String
    WithEvents notify As New System.Windows.Forms.NotifyIcon

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        random()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If bw.IsBusy = False Then
            Dim ok As Integer = MsgBox("Are you sure?", MsgBoxStyle.OkCancel, "Confirm")
            If ok = 1 Then
                olesson = TextBox3.Text
                buttonone.Visibility = Windows.Visibility.Hidden
                Button1.Visibility = Windows.Visibility.Hidden
                gridbatchctrl.Visibility = Windows.Visibility.Visible
                If isall = 0 Then
                    progressbar2.Visibility = Windows.Visibility.Hidden
                    prouser.Visibility = Windows.Visibility.Hidden
                    pro3.Visibility = Windows.Visibility.Hidden
                Else
                    GridLogin.Visibility = Windows.Visibility.Hidden
                    GridUser.Visibility = Windows.Visibility.Visible
                    ButtonL2.Visibility = Windows.Visibility.Hidden
                    progressbar2.Visibility = Windows.Visibility.Visible
                    prouser.Visibility = Windows.Visibility.Visible
                    pro3.Visibility = Windows.Visibility.Visible
                End If
                bw.WorkerSupportsCancellation = True
                bw.WorkerReportsProgress = True
                ButtonL2.IsEnabled = False
                combobox2.IsEnabled = False
                combobox3.IsEnabled = False
                num = Val(combobox2.Text)
                timewait = Val(combobox3.Text) * 1000
                If isall = 0 Then
                    bw.RunWorkerAsync()
                Else
                    combobox1.BindingGroup = New BindingGroup
                    combobox1.SelectedIndex = 0
                    numa = combobox1.Items.Count
                    username = combobox1.Text.Replace(" ", "")
                    bwall.WorkerSupportsCancellation = True
                    bwall.WorkerReportsProgress = True
                    bwall.RunWorkerAsync()
                End If
            End If
        Else
            MsgBox("Wait")
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As RoutedEventArgs) Handles Button3.Click
        Dim about = New AboutWindow
        about.Owner = Me
        about.ShowDialog()
    End Sub

    Private Sub ButtonL4_Click(sender As Object, e As RoutedEventArgs) Handles ButtonL4.Click
        Me.Cursor = System.Windows.Input.Cursors.Wait
        ButtonL4.IsEnabled = False
        combobox1.IsEnabled = False
        username = combobox1.Text.Replace(" ", "")
        If bwuser.IsBusy = False Then
            bwuser.RunWorkerAsync()
        End If
    End Sub

    Private Sub bwuser_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwuser.DoWork
        If isloggedin = False Then
            login(username)
        Else
            logout()
        End If
        Return
    End Sub

    Private Sub bwuser_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwuser.RunWorkerCompleted
        If isloggedin = True Then
            namet.Content = username
            GridLogin.Visibility = Windows.Visibility.Hidden
            GridUser.Visibility = Windows.Visibility.Visible
            ButtonL4.IsEnabled = True
            combobox1.IsEnabled = True
        Else
            GridUser.Visibility = Windows.Visibility.Hidden
            GridLogin.Visibility = Windows.Visibility.Visible
            ButtonL2.IsEnabled = True
            ButtonL4.IsEnabled = True
            combobox1.IsEnabled = True
        End If
        Me.Cursor = System.Windows.Input.Cursors.AppStarting
    End Sub

    Private Sub bwall_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwall.DoWork
        isbwbusy = True
        For i As Integer = 1 To numa
            If bwall.CancellationPending Then
                Return
            End If
            If isloggedin = True Then
                logout()
            End If
            progressa = i / numa * 100
            login(username)
            bwall.ReportProgress(progressa)
            Thread.Sleep(1000)
            Do
                Thread.Sleep(1000)
            Loop Until bw.IsBusy = False And isbwbusy = False
        Next
        Return
    End Sub

    Private Sub bwall_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles bwall.ProgressChanged
        combobox1.SelectedIndex += 1
        username = combobox1.Text.Replace(" ", "")
        progressbar2.Value = progressa
        pro3.Content = Int(progressa).ToString + "%"
        namet.Content = username
        progressbar1.Value = 0
        If bw.IsBusy = False Then
            TextBox3.Text = olesson
            bw.WorkerSupportsCancellation = True
            bw.WorkerReportsProgress = True
            bw.RunWorkerAsync()
            isbwbusy = False
        End If
    End Sub
    Private Sub bwall_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwall.RunWorkerCompleted
        If Me.IsVisible = True Then
            MsgBox("Done")
        End If
        buttonone.Visibility = Windows.Visibility.Visible
        Button1.Visibility = Windows.Visibility.Visible
        gridbatchctrl.Visibility = Windows.Visibility.Hidden
    End Sub

    Private Sub ButtonL2_Click(sender As Object, e As RoutedEventArgs) Handles ButtonL2.Click
        Me.Cursor = System.Windows.Input.Cursors.Wait
        ButtonL2.IsEnabled = False
        If bwuser.IsBusy = False Then
            bwuser.RunWorkerAsync()
        End If
    End Sub

    Private Sub Buttonse_Click_1(sender As Object, e As RoutedEventArgs) Handles Buttonse.Click
        Dim WebBrowser1 As New WebBrowser
        WebBrowser1.Owner = Me
        WebBrowser1.aspsession = aspsession
        WebBrowser1.ecpacid = ecpacid
        WebBrowser1.ShowDialog()
        If WebBrowser1.lesson = "" Then
        Else
            TextBox3.Text = WebBrowser1.lesson
        End If
    End Sub

    Private Sub Button02_Click(sender As Object, e As RoutedEventArgs) Handles Button02.Click
        randomplus()
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        Dim pwdmgr As New PasswordMgr
        pwdmgr.Owner = Me
        pwdmgr.ShowDialog()
        Try
            Dim PasswordsDataSet As youret.PasswordsDataSet = CType(Me.FindResource("PasswordsDataSet"), youret.PasswordsDataSet)
            Dim PasswordsDataSetTableTableAdapter As youret.PasswordsDataSetTableAdapters.TableTableAdapter = New youret.PasswordsDataSetTableAdapters.TableTableAdapter()
            PasswordsDataSetTableTableAdapter.Fill(PasswordsDataSet.Table)
            Dim TableViewSource As System.Windows.Data.CollectionViewSource = CType(Me.FindResource("TableViewSource"), System.Windows.Data.CollectionViewSource)
            TableViewSource.View.MoveCurrentToFirst()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Function random()
        TextBox4.Text = (Val(TextBox5.Text) + Val(TextBox6.Text) + Val(TextBox7.Text) + Val(TextBox8.Text)) / 4
        TextBox10.Text = Val(TextBox5.Text) + Format(Rnd(), ".00")
        TextBox11.Text = Val(TextBox6.Text) + Format(Rnd(), ".00")
        TextBox12.Text = Val(TextBox7.Text) + Format(Rnd(), ".00")
        TextBox13.Text = Val(TextBox8.Text) + Format(Rnd(), ".00")
        TextBox9.Text = Format((Val(TextBox10.Text) + Val(TextBox11.Text) + Val(TextBox12.Text) + Val(TextBox13.Text)) / 4, ".00")
        TextBox15.Text = Int(400 + 200 * Rnd())
        Return 0
    End Function

    Public Function randomplus()
        TextBox4.Text = (Val(TextBox5.Text) + Val(TextBox6.Text) + Val(TextBox7.Text) + Val(TextBox8.Text)) / 4
        TextBox10.Text = Val(TextBox5.Text) + Format((4 * Rnd() + 1) * Rnd(), ".00")
        TextBox11.Text = Val(TextBox6.Text) + Format((4 * Rnd() + 1) * Rnd(), ".00")
        TextBox12.Text = Val(TextBox7.Text) + Format((4 * Rnd() + 1) * Rnd(), ".00")
        TextBox13.Text = Val(TextBox8.Text) + Format((4 * Rnd() + 1) * Rnd(), ".00")
        TextBox9.Text = Format((Val(TextBox10.Text) + Val(TextBox11.Text) + Val(TextBox12.Text) + Val(TextBox13.Text)) / 4, ".00")
        TextBox15.Text = Int(400 + 200 * Rnd())
        Return 0
    End Function

    Public Function go()
        Dim web As New System.Net.WebClient()
        With web
            .Headers.Clear()
            .Headers.Add("Accept", "*/*")
            .Headers.Add("Accept-Language", "zh-CN")
            .Headers.Add("Referer", "http://cn.myet.com/Upload/ElizaWeb4Content/FlashSettings/LessonSelfTest.swf?CultureName=zh-CN&MyETURLPrefix=http://cn.myet.com/ElizaWeb&Sock")
            .Headers.Add("x-flash-version", "11,5,502,149")
            .Headers.Add("Content-Type", "application/x-www-form-urlencoded")
            .Headers.Add("Accept-Encoding", "gzip, deflate")
            .Headers.Add("User-Agent", "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.2; WOW64; Trident/7.0; .NET4.0E; .NET4.0C; InfoPath.3; Media Center PC 6.0; .NET CLR 3.5.30729; .NET CLR 2.0.50727; .NET CLR 3.0.30729; WWTClient2)")
            .Headers.Add("Pragma", "no-cache")
            .Headers.Add("Cookie", cookiesend)
        End With
        lesson = TextBox3.Text
        score0 = TextBox9.Text
        score1 = TextBox10.Text
        score2 = TextBox11.Text
        score3 = TextBox12.Text
        score4 = TextBox13.Text
        time = TextBox15.Text
        If CheckBox1.IsChecked = True Then
            testmode = 1
        Else
            testmode = 0
        End If
        Dim randomid As String = Format(Rnd(), "0.000000000") + Rnd() / 8
        dataet = "RandomID=" + randomid + "&FlashData=<ScoreInfo AccountID=""" + accountid + """ LessonID=""" + lesson + """ Score=""" + score0 + """ PronunciationScore=""" + score1 + """ PitchScore=""" + score2 + """ TimingScore=""" + score3 + """ IntensityScore=""" + score4 + """ TimeElapsed=""" + time + """ WhereFrom="""" TestMode=""" + testmode + """> <ignoreWhitespace>false</ignoreWhitespace> </ScoreInfo>"
        Dim uri As New Uri("http://cn.myet.com/ElizaWeb/LessonSelfTestServices.aspx?op=UploadSpeakingScore")
        Try
            web.UploadStringAsync(uri, "POST", dataet)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        web.Dispose()
        Return 0
    End Function

    Public Function login(ByRef username As String)
        Dim conn As SqlConnection = New SqlConnection("Data Source=(LocalDB)\v11.0;AttachDbFilename=|DataDirectory|\Passwords.mdf;Integrated Security=True")
        Dim sql As String = "SELECT Password FROM [Table] WHERE (Username = '" + username + "')"
        Dim cmd As New SqlCommand(sql, conn)
        Try
            conn.Open()
            password = cmd.ExecuteScalar().ToString.Replace(" ", "")
            conn.Close()
            conn.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        data = "__VIEWSTATE=&hdnCultureName=zh-CN&hdnSysAdminBulletinStatus=None&ReturnUrl=LoginPost.aspx&hdnPassword=&hdnWarningMsg=&hdnAutoLogin=N&hdnIsFirstLogin=Y&hdnLoginVerifyUrl=&hdnClientIP=&UserName=" + username + "&Password=" + password + "&btnLogin=%E7%99%BB%E5%BD%95"
        Dim web As New System.Net.WebClient()
        With web
            .Headers.Add("Accept", "application/x-ms-application, image/jpeg, application/xaml+xml, image/gif, image/pjpeg, application/x-ms-xbap, application/x-shockwave-flash, */*")
            .Headers.Add("Accept-Language", "zh-CN")
            .Headers.Add("Accept-Encoding", "gzip, deflate")
            .Headers.Add("Content-Type", "application/x-www-form-urlencoded")
            .Headers.Add("User-Agent", "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.2; WOW64; Trident/7.0; .NET4.0E; .NET4.0C; InfoPath.3; Media Center PC 6.0; .NET CLR 3.5.30729; .NET CLR 2.0.50727; .NET CLR 3.0.30729; WWTClient2)")
            .Headers.Add("Referer", "http://cn.myet.com/ElizaWeb/Alliance/MyET_Login.aspx?EptParams=BTKH2sQnFyVfRAldwWOL7CVXYIplizhUheIcGB2ULMs=&LoginVerifyUrl=")
        End With
        web.UploadString("http://cn.myet.com/ElizaWeb/Authentication/ValidateMyETUsernameNPassword.aspx?SaveAccount=N", data)
        aspsession = web.ResponseHeaders.Get("Set-Cookie").Substring(18, 24)
        web.Dispose()
        Dim url0 As String
        cookielogin = "LLang=EN; TargetServerKey=CN1-LLabs; APVersion=5705; WPort=49156; LastSessionID=" + aspsession + "; RememberMyAccount=N; ASP.NET_SessionId=" + aspsession + "; ContentProvider=; IsCookieSupported=Y; IsAllianceAccount=N; TmpRememberMyAccount=N; ConnType=Local; APArgs=ContentProvider=&Version=5705&MediaLocation=&ServerKey=&LaunchMyETArg=&MAC=9E-05-40-A5-BE-DE&SocketPort=1024&PPort=300&WPort=49157; APFixedArgs=Version=5705&MediaLocation=&MAC=9E-05-40-A5-BE-DE&SocketPort=1024&PPort=300&WPort=49157; Mac=9E-05-40-A5-BE-DE"
        Dim web2 As New System.Net.WebClient()
        With web2
            .Headers.Add("Accept", "application/x-ms-application, image/jpeg, application/xaml+xml, image/gif, image/pjpeg, application/x-ms-xbap, application/x-shockwave-flash, */*")
            .Headers.Add("Accept-Language", "zh-CN")
            .Headers.Add("Accept-Encoding", "gzip, deflate")
            .Headers.Add("Content-Type", "application/x-www-form-urlencoded")
            .Headers.Add("User-Agent", "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.2; WOW64; Trident/7.0; .NET4.0E; .NET4.0C; InfoPath.3; Media Center PC 6.0; .NET CLR 3.5.30729; .NET CLR 2.0.50727; .NET CLR 3.0.30729; WWTClient2)")
            .Headers.Add("Referer", "http://cn.myet.com/ElizaWeb/Authentication/ValidateMyETUsernameNPassword.aspx?SaveAccount=N")
            .Headers.Add("Cookie", cookielogin)
        End With
        url0 = "http://cn.myet.com/ElizaWeb/Authentication/LoginPost.aspx?ESID=" + aspsession + "&UserName=" + username
        web2.DownloadString(url0)
        Dim errorstate As Boolean = False
        Try
            ecpacidtmp = web2.ResponseHeaders.Get("Set-Cookie").Split("; path=/")
            ecpacid = ecpacidtmp(2).Substring(16, 24)
        Catch ex As Exception
            MsgBox("Error", MsgBoxStyle.Critical, "YourET")
            errorstate = True
        End Try
        web2.Dispose()
        If errorstate = False Then
            Dim web3 As New System.Net.WebClient()
            With web3
                .Headers.Add("Accept", "application/x-ms-application, image/jpeg, application/xaml+xml, image/gif, image/pjpeg, application/x-ms-xbap, application/x-shockwave-flash, */*")
                .Headers.Add("Accept-Language", "zh-CN")
                .Headers.Add("Accept-Encoding", "gzip, deflate")
                .Headers.Add("User-Agent", "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.2; WOW64; Trident/7.0; .NET4.0E; .NET4.0C; InfoPath.3; Media Center PC 6.0; .NET CLR 3.5.30729; .NET CLR 2.0.50727; .NET CLR 3.0.30729; WWTClient2)")
                .Headers.Add("Referer", "http://cn.myet.com/ElizaWeb/Home.aspx")
                .Headers.Add("Cookie", cookielogin)
                .Headers.Add("EcpACID", ecpacid)
                .Headers.Add("OnlinePurchaseEntrance", "ELIZAWEB")
                .Headers.Add("TmpPassword", password)
            End With
            accountidtmp = web3.DownloadString("http://cn.myet.com/ElizaWeb/PersonalizedPage.aspx")
            accountid = accountidtmp.Substring(195, 6)
            web3.Dispose()
            cookiesend = "LLang=EN; TargetServerKey=CN1-LLabs; APVersion=5705; WPort=49155; LastSessionID=" + aspsession + "; MyETAccountID=" + username + "; RememberMyAccount=N; LastLessonID=CN-PEP-XXQ-00033; ASP.NET_SessionId=" + aspsession + "; ContentProvider=; IsCookieSupported=Y; IsAllianceAccount=N; TmpRememberMyAccount=Y; TmpPassword=" + password + "; EcpACID=" + ecpacid + "; CourseIDInSessionInfo=PEP-XXQ-005; ConnType=Local; APArgs=ContentProvider=&Version=5705&MediaLocation=&ServerKey=&LaunchMyETArg=&MAC=9E-05-40-A5-BE-DE&SocketPort=1024&PPort=300&WPort=49157; APFixedArgs=Version=5705&MediaLocation=&MAC=9E-05-40-A5-BE-DE&SocketPort=1024&PPort=300&WPort=49157; Mac=9E-05-40-A5-BE-DE"
        End If
        If errorstate = False Then
            isloggedin = True
        Else
            isloggedin = False
        End If
        Return 0
    End Function

    Public Function logout()
        Dim web4 As New System.Net.WebClient()
        With web4
            .Headers.Add("Accept", "application/x-ms-application, image/jpeg, application/xaml+xml, image/gif, image/pjpeg, application/x-ms-xbap, application/x-shockwave-flash, */*")
            .Headers.Add("Accept-Language", "zh-CN")
            .Headers.Add("Accept-Encoding", "gzip, deflate")
            .Headers.Add("User-Agent", "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; WOW64; Trident/6.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; WWTClient2; .NET4.0C; .NET4.0E; BRI/2; InfoPath.3; MASP)")
            .Headers.Add("Referer", "http://cn.myet.com/ElizaWeb/Home.aspx")
            .Headers.Add("Cookie", cookielogin)
        End With
        web4.DownloadString("http://cn.myet.com/ElizaWeb/Logout.aspx")
        web4.Dispose()
        isloggedin = False
        Return 0
    End Function

    Public Function plusone()
        Try
            TextBox3.Text = TextBox3.Text.Substring(0, 11) + Format(Val(TextBox3.Text.Substring(12)) + 1, "00000")
        Catch ex As Exception
        End Try
        Return 0
    End Function

    Private Sub Bw_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bw.DoWork
        For i As Integer = 1 To num
            If bw.CancellationPending Then
                Return
            End If
            progress = i / num * 100
            bw.ReportProgress(progress)
            If i < num Then
                Thread.Sleep(timewait)
            End If
        Next
        Return
    End Sub

    Private Sub Bw_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles bw.ProgressChanged
        randomplus()
        go()
        If progress < 100 Then
            plusone()
        End If
        progressbar1.Value = progress
        pro2.Content = Int(progress).ToString + "%"
    End Sub

    Private Sub Bw_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bw.RunWorkerCompleted
        If isall = 0 Then
            gridbatchctrl.Visibility = Windows.Visibility.Hidden
            Button1.Visibility = Windows.Visibility.Visible
            buttonone.Visibility = Windows.Visibility.Visible
            ButtonL2.Visibility = Windows.Visibility.Visible
            ButtonL2.IsEnabled = True
            combobox2.IsEnabled = True
            combobox3.IsEnabled = True
            If Me.IsVisible = True Then
                MsgBox("Done")
            End If
        End If
    End Sub

    Private Sub Buttonstop_Click(sender As Object, e As RoutedEventArgs) Handles buttonstop.Click
        If bwall.IsBusy = True Then
            bwall.CancelAsync()
        End If
        bw.CancelAsync()
        gridbatchctrl.Visibility = Windows.Visibility.Hidden
        Button1.Visibility = Windows.Visibility.Visible
        buttonone.Visibility = Windows.Visibility.Visible
        ButtonL2.Visibility = Windows.Visibility.Visible
        ButtonL2.IsEnabled = True
        combobox2.IsEnabled = True
        combobox3.IsEnabled = True
    End Sub

    Private Sub buttonone_Click(sender As Object, e As RoutedEventArgs) Handles buttonone.Click
        If isall = 1 Then
            buttonone.Content = "One"
            isall = 0
        Else
            buttonone.Content = "All"
            isall = 1
        End If
    End Sub

    Private Sub MainWindow_Closing(sender As Object, e As System.ComponentModel.CancelEventArgs)
        If isloggedin = True Then
            logout()
        End If
    End Sub

    Protected Overrides Sub OnMouseRightButtonDown(e As MouseButtonEventArgs)
        MyBase.OnMouseRightButtonUp(e)
        notify.Text = "YourET - " + combobox1.Text.Replace(" ", "")
        notify.Icon = My.Resources.yourettray
        notify.Visible = True
        Me.Hide()
    End Sub

    Private Sub Notify_MouseClick(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles notify.Click
        Me.Show()
        notify.Visible = False
    End Sub

    Private Sub bwload_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwload.DoWork
        Try
            Dim PasswordsDataSet As youret.PasswordsDataSet = CType(Me.FindResource("PasswordsDataSet"), youret.PasswordsDataSet)
            Dim PasswordsDataSetTableTableAdapter As youret.PasswordsDataSetTableAdapters.TableTableAdapter = New youret.PasswordsDataSetTableAdapters.TableTableAdapter()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub bwload_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwload.RunWorkerCompleted
        Try
            Dim PasswordsDataSet As youret.PasswordsDataSet = CType(Me.FindResource("PasswordsDataSet"), youret.PasswordsDataSet)
            Dim PasswordsDataSetTableTableAdapter As youret.PasswordsDataSetTableAdapters.TableTableAdapter = New youret.PasswordsDataSetTableAdapters.TableTableAdapter()
            PasswordsDataSetTableTableAdapter.Fill(PasswordsDataSet.Table)
            Dim TableViewSource As System.Windows.Data.CollectionViewSource = CType(Me.FindResource("TableViewSource"), System.Windows.Data.CollectionViewSource)
            TableViewSource.View.MoveCurrentToFirst()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        combobox1.IsEnabled = True
        ButtonL4.IsEnabled = True
        labelloading.Visibility = Windows.Visibility.Hidden
    End Sub
End Class


