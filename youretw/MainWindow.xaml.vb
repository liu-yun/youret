Option Explicit Off
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Windows.Interop
Imports System.Threading
Imports System.Net
Imports System.Data.SqlClient
Imports youret.PasswordsDataSetTableAdapters

Class MainWindow
    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs) Handles MyBase.Loaded
        ExtendGlass()
        GridUser.Visibility = Visibility.Hidden
        GridBatchControl.Visibility = Visibility.Hidden
        ComboBoxUsers.IsEnabled = False
        ButtonLogin.IsEnabled = False
        _bwload.RunWorkerAsync()
    End Sub

    Protected Overrides Sub OnMouseLeftButtonDown(e As MouseButtonEventArgs)
        MyBase.OnMouseLeftButtonDown(e)
        DragMove()
    End Sub

    Private Declare Sub DwmIsCompositionEnabled Lib "dwmapi.dll" (ByRef b As Boolean)
    Private Declare Sub DwmExtendFrameIntoClientArea Lib "dwmapi.dll" (ByVal hWnd As IntPtr, ByRef pMarInset As Margins)

    Private Structure Margins
        Public Sub New(ByVal t As Thickness)
            _left = CInt(t.Left)
            _right = CInt(t.Right)
            _top = CInt(t.Top)
            _bottom = CInt(t.Bottom)
        End Sub

        Private _left As Integer
        Private _right As Integer
        Private _top As Integer
        Private _bottom As Integer
    End Structure

    Private Sub ExtendGlass()
        Try
            Dim b As Boolean
            DwmIsCompositionEnabled(b)
            If b Then
                Dim hWnd As IntPtr = New WindowInteropHelper(Me).Handle
                If hWnd <> IntPtr.Zero Then
                    Background = Brushes.Transparent
                    HwndSource.FromHwnd(hWnd).CompositionTarget.BackgroundColor = Colors.Transparent
                    Dim m As New Margins(New Thickness(-1))
                    DwmExtendFrameIntoClientArea(hWnd, m)
                End If
            End If
        Catch
        End Try
    End Sub

    Dim _username As String
    Dim _password As String
    Dim _cookielogin As String
    Dim _accountid As String
    Dim _aspsession As String
    Dim _ecpacid As String
    Dim _datalogin As String
    Dim _cookiesend As String
    Dim _isall As String = 0
    WithEvents _bwgo As New BackgroundWorker()
    WithEvents _bwuser As New BackgroundWorker()
    WithEvents _bwload As New BackgroundWorker()
    WithEvents _bwall As New BackgroundWorker()
    Dim _progresslessons As Double = 0
    Dim _progressusers As Double = 0
    Dim _timewait As Integer
    Dim _numlessons As Integer
    Dim _numusers As Integer
    Dim _isloggedin As Boolean = False
    Dim _isbwbusy As Boolean = False
    Dim _lessonstart As String
    WithEvents _notify As New NotifyIcon

    Private Sub ButtonRandom_Click(sender As Object, e As EventArgs) Handles ButtonRandom.Click
        Random()
    End Sub

    Private Sub ButtonGo_Click(sender As Object, e As EventArgs) Handles ButtonGo.Click
        If _bwgo.IsBusy = False Then
            Dim ok As Integer = MsgBox("Are you sure?", MsgBoxStyle.OkCancel, "Confirm")
            If ok = 1 Then
                _lessonstart = TextBoxLesson.Text
                ButtonOneAll.Visibility = Visibility.Hidden
                ButtonGo.Visibility = Visibility.Hidden
                GridBatchControl.Visibility = Visibility.Visible
                If _isall = 0 Then
                    ProgressBarUsers.Visibility = Visibility.Hidden
                    LabelUsers.Visibility = Visibility.Hidden
                    LabelUsersProgress.Visibility = Visibility.Hidden
                Else
                    GridLogin.Visibility = Visibility.Hidden
                    GridUser.Visibility = Visibility.Visible
                    ButtonLogout.Visibility = Visibility.Hidden
                    ProgressBarUsers.Visibility = Visibility.Visible
                    LabelUsers.Visibility = Visibility.Visible
                    LabelUsersProgress.Visibility = Visibility.Visible
                End If
                _bwgo.WorkerSupportsCancellation = True
                _bwgo.WorkerReportsProgress = True
                ButtonLogout.IsEnabled = False
                ComboBoxLessons.IsEnabled = False
                ComboBoxTime.IsEnabled = False
                _numlessons = Val(ComboBoxLessons.Text)
                _timewait = Val(ComboBoxTime.Text) * 1000
                If _isall = 0 Then
                    _bwgo.RunWorkerAsync()
                Else
                    ComboBoxUsers.BindingGroup = New BindingGroup
                    ComboBoxUsers.SelectedIndex = 0
                    _numusers = ComboBoxUsers.Items.Count
                    _username = ComboBoxUsers.Text.Replace(" ", "")
                    _bwall.WorkerSupportsCancellation = True
                    _bwall.WorkerReportsProgress = True
                    _bwall.RunWorkerAsync()
                End If
            End If
        Else
            MsgBox("Wait")
        End If
    End Sub

    Private Sub ButtonAbout_Click(sender As Object, e As RoutedEventArgs) Handles ButtonAbout.Click
        Dim about = New AboutWindow
        about.Owner = Me
        about.ShowDialog()
    End Sub

    Private Sub ButtonLogin_Click(sender As Object, e As RoutedEventArgs) Handles ButtonLogin.Click
        Cursor = Input.Cursors.Wait
        ButtonLogin.IsEnabled = False
        ComboBoxUsers.IsEnabled = False
        _username = ComboBoxUsers.Text.Replace(" ", "")
        If _bwuser.IsBusy = False Then
            _bwuser.RunWorkerAsync()
        End If
    End Sub

    Private Sub bwuser_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles _bwuser.DoWork
        If _isloggedin = False Then
            Login(_username)
        Else
            Logout()
        End If
        Return
    End Sub

    Private Sub bwuser_RunWorkerCompleted(ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs) _
        Handles _bwuser.RunWorkerCompleted
        If _isloggedin = True Then
            LabelUser.Content = _username
            GridLogin.Visibility = Visibility.Hidden
            GridUser.Visibility = Visibility.Visible
            ButtonLogin.IsEnabled = True
            ComboBoxUsers.IsEnabled = True
        Else
            GridUser.Visibility = Visibility.Hidden
            GridLogin.Visibility = Visibility.Visible
            ButtonLogout.IsEnabled = True
            ButtonLogin.IsEnabled = True
            ComboBoxUsers.IsEnabled = True
        End If
        Cursor = Input.Cursors.AppStarting
    End Sub

    Private Sub bwall_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles _bwall.DoWork
        _isbwbusy = True
        For i As Integer = 1 To _numusers
            If _bwall.CancellationPending Then
                Return
            End If
            If _isloggedin = True Then
                Logout()
            End If
            _progressusers = i / _numusers * 100
            Login(_username)
            _bwall.ReportProgress(_progressusers)
            Do
                Thread.Sleep(1000)
            Loop Until _bwgo.IsBusy = False And _isbwbusy = False
        Next
        Return
    End Sub

    Private Sub bwall_ProgressChanged(ByVal sender As Object, ByVal e As ProgressChangedEventArgs) _
        Handles _bwall.ProgressChanged
        LabelUser.Content = _username
        ComboBoxUsers.SelectedIndex += 1
        _username = ComboBoxUsers.Text.Replace(" ", "")
        ProgressBarUsers.Value = _progressusers
        LabelUsersProgress.Content = Int(_progressusers).ToString + "%"
        ProgressBarLessons.Value = 0
        If _bwgo.IsBusy = False Then
            TextBoxLesson.Text = _lessonstart
            _bwgo.WorkerSupportsCancellation = True
            _bwgo.WorkerReportsProgress = True
            _bwgo.RunWorkerAsync()
            _isbwbusy = False
        End If
    End Sub

    Private Sub bwall_RunWorkerCompleted(ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs) _
        Handles _bwall.RunWorkerCompleted
        If IsVisible = True Then
            MsgBox("Done")
        End If
        ButtonOneAll.Visibility = Visibility.Visible
        ButtonGo.Visibility = Visibility.Visible
        ButtonLogout.Visibility = Visibility.Visible
        ButtonLogout.IsEnabled = True
        ComboBoxLessons.IsEnabled = True
        ComboBoxTime.IsEnabled = True
        GridBatchControl.Visibility = Visibility.Hidden
    End Sub

    Private Sub ButtonLogout_Click(sender As Object, e As RoutedEventArgs) Handles ButtonLogout.Click
        Cursor = Input.Cursors.Wait
        ButtonLogout.IsEnabled = False
        If _bwuser.IsBusy = False Then
            _bwuser.RunWorkerAsync()
        End If
    End Sub

    Private Sub ButtonSelect_Click(sender As Object, e As RoutedEventArgs) Handles ButtonSelect.Click
        Dim browser As New WebBrowser
        browser.Owner = Me
        browser.Aspsession = _aspsession
        browser.Ecpacid = _ecpacid
        browser.ShowDialog()
        If browser.lesson <> "" Then
            TextBoxLesson.Text = browser.Lesson
        End If
    End Sub

    Private Sub ButtonRandomPlus_Click(sender As Object, e As RoutedEventArgs) Handles ButtonRandomPlus.Click
        RandomPlus()
    End Sub

    Private Sub ButtonManage_Click(sender As Object, e As RoutedEventArgs) Handles ButtonManage.Click
        Dim passwordmgr As New PasswordMgr
        passwordmgr.Owner = Me
        passwordmgr.ShowDialog()
        Try
            Dim passwordsDataSet As PasswordsDataSet = CType(FindResource("PasswordsDataSet"), PasswordsDataSet)
            Dim passwordsDataSetTableTableAdapter As TableTableAdapter = New TableTableAdapter()
            passwordsDataSetTableTableAdapter.Fill(passwordsDataSet.Table)
            Dim tableViewSource As CollectionViewSource = CType(FindResource("TableViewSource"), CollectionViewSource)
            tableViewSource.View.MoveCurrentToFirst()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Function Random()
        TextBoxScoreIn0.Text = (Val(TextBoxScoreIn1.Text) + Val(TextBoxScoreIn2.Text) + Val(TextBoxScoreIn3.Text) + Val(TextBoxScoreIn4.Text)) / 4
        TextBoxScore1.Text = Val(TextBoxScoreIn1.Text) + Format(Rnd(), ".00")
        TextBoxScore2.Text = Val(TextBoxScoreIn2.Text) + Format(Rnd(), ".00")
        TextBoxScore3.Text = Val(TextBoxScoreIn3.Text) + Format(Rnd(), ".00")
        TextBoxScore4.Text = Val(TextBoxScoreIn4.Text) + Format(Rnd(), ".00")
        TextBoxScore0.Text = Format((Val(TextBoxScore1.Text) + Val(TextBoxScore2.Text) + Val(TextBoxScore3.Text) + Val(TextBoxScore4.Text)) / 4,
                               ".00")
        TextBoxTime.Text = Int(400 + 200 * Rnd())
        Return 0
    End Function

    Public Function RandomPlus()
        TextBoxScoreIn0.Text = (Val(TextBoxScoreIn1.Text) + Val(TextBoxScoreIn2.Text) + Val(TextBoxScoreIn3.Text) + Val(TextBoxScoreIn4.Text)) / 4
        TextBoxScore1.Text = Val(TextBoxScoreIn1.Text) + Format((4 * Rnd() + 1) * Rnd(), ".00")
        TextBoxScore2.Text = Val(TextBoxScoreIn2.Text) + Format((4 * Rnd() + 1) * Rnd(), ".00")
        TextBoxScore3.Text = Val(TextBoxScoreIn3.Text) + Format((4 * Rnd() + 1) * Rnd(), ".00")
        TextBoxScore4.Text = Val(TextBoxScoreIn4.Text) + Format((4 * Rnd() + 1) * Rnd(), ".00")
        TextBoxScore0.Text = Format((Val(TextBoxScore1.Text) + Val(TextBoxScore2.Text) + Val(TextBoxScore3.Text) + Val(TextBoxScore4.Text)) / 4,
                               ".00")
        TextBoxTime.Text = Int(400 + 200 * Rnd())
        Return 0
    End Function

    Public Function Go()
        Dim lesson As String
        Dim score0 As String
        Dim score1 As String
        Dim score2 As String
        Dim score3 As String
        Dim score4 As String
        Dim time As String
        Dim istestmode As String
        Dim datago As String
        Dim webgo As New WebClient()
        With webgo
            .Headers.Clear()
            .Headers.Add("Accept", "*/*")
            .Headers.Add("Accept-Language", "zh-CN")
            .Headers.Add("Referer",
                         "http://cn.myet.com/Upload/ElizaWeb4Content/FlashSettings/LessonSelfTest.swf?CultureName=zh-CN&MyETURLPrefix=http://cn.myet.com/ElizaWeb&Sock")
            .Headers.Add("x-flash-version", "11,5,502,149")
            .Headers.Add("Content-Type", "application/x-www-form-urlencoded")
            .Headers.Add("Accept-Encoding", "gzip, deflate")
            .Headers.Add("User-Agent",
                         "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.2; WOW64; Trident/7.0; .NET4.0E; .NET4.0C; InfoPath.3; Media Center PC 6.0; .NET CLR 3.5.30729; .NET CLR 2.0.50727; .NET CLR 3.0.30729; WWTClient2)")
            .Headers.Add("Pragma", "no-cache")
            .Headers.Add("Cookie", _cookiesend)
        End With
        lesson = TextBoxLesson.Text
        score0 = TextBoxScore0.Text
        score1 = TextBoxScore1.Text
        score2 = TextBoxScore2.Text
        score3 = TextBoxScore3.Text
        score4 = TextBoxScore4.Text
        time = TextBoxTime.Text
        If CheckBoxTest.IsChecked = True Then
            istestmode = 1
        Else
            istestmode = 0
        End If
        Dim randomid As String = Format(Rnd(), "0.000000000") + Rnd() / 8
        datago = "RandomID=" + randomid + "&FlashData=<ScoreInfo AccountID=""" + _accountid + """ LessonID=""" + lesson +
                 """ Score=""" + score0 + """ PronunciationScore=""" + score1 + """ PitchScore=""" + score2 +
                 """ TimingScore=""" + score3 + """ IntensityScore=""" + score4 + """ TimeElapsed=""" + time +
                 """ WhereFrom="""" TestMode=""" + istestmode +
                 """> <ignoreWhitespace>false</ignoreWhitespace> </ScoreInfo>"
        Try
            webgo.UploadStringAsync(New Uri("http://cn.myet.com/ElizaWeb/LessonSelfTestServices.aspx?op=UploadSpeakingScore"), "POST", datago)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        webgo.Dispose()
        Return 0
    End Function

    Public Function Login(ByRef username As String)
        Dim webtemp As String
        Dim conn As SqlConnection =
                New SqlConnection(
                    "Data Source=(LocalDB)\v11.0;AttachDbFilename=|DataDirectory|\Passwords.mdf;Integrated Security=True")
        Dim sql As String = "SELECT Password FROM [Table] WHERE (Username = '" + username + "')"
        Dim cmd As New SqlCommand(sql, conn)
        Try
            conn.Open()
            _password = cmd.ExecuteScalar().ToString.Replace(" ", "")
            conn.Close()
            conn.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        _datalogin =
            "__VIEWSTATE=&hdnCultureName=zh-CN&hdnSysAdminBulletinStatus=None&ReturnUrl=LoginPost.aspx&hdnPassword=&hdnWarningMsg=&hdnAutoLogin=N&hdnIsFirstLogin=Y&hdnLoginVerifyUrl=&hdnClientIP=&UserName=" +
            username + "&Password=" + _password + "&btnLogin=%E7%99%BB%E5%BD%95"
        Dim weblogin1 As New WebClient()
        With weblogin1
            .Headers.Add("Accept",
                         "application/x-ms-application, image/jpeg, application/xaml+xml, image/gif, image/pjpeg, application/x-ms-xbap, application/x-shockwave-flash, */*")
            .Headers.Add("Accept-Language", "zh-CN")
            .Headers.Add("Accept-Encoding", "gzip, deflate")
            .Headers.Add("Content-Type", "application/x-www-form-urlencoded")
            .Headers.Add("User-Agent",
                         "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.2; WOW64; Trident/7.0; .NET4.0E; .NET4.0C; InfoPath.3; Media Center PC 6.0; .NET CLR 3.5.30729; .NET CLR 2.0.50727; .NET CLR 3.0.30729; WWTClient2)")
            .Headers.Add("Referer",
                         "http://cn.myet.com/ElizaWeb/Alliance/MyET_Login.aspx?EptParams=BTKH2sQnFyVfRAldwWOL7CVXYIplizhUheIcGB2ULMs=&LoginVerifyUrl=")
        End With
        weblogin1.UploadString("http://cn.myet.com/ElizaWeb/Authentication/ValidateMyETUsernameNPassword.aspx?SaveAccount=N",
                         _datalogin)
        _aspsession = weblogin1.ResponseHeaders.Get("Set-Cookie").Substring(18, 24)
        weblogin1.Dispose()
        Dim urlauth As String
        _cookielogin = "LLang=EN; TargetServerKey=CN1-LLabs; APVersion=5708; WPort=49156; LastSessionID=" + _aspsession +
                      "; RememberMyAccount=N; ASP.NET_SessionId=" + _aspsession +
                      "; ContentProvider=; IsCookieSupported=Y; IsAllianceAccount=N; TmpRememberMyAccount=N; ConnType=Local; APArgs=ContentProvider=&Version=5708&MediaLocation=&ServerKey=&LaunchMyETArg=&MAC=9E-05-40-A5-BE-DE&SocketPort=1024&PPort=300&WPort=49157; APFixedArgs=Version=5708&MediaLocation=&MAC=9E-05-40-A5-BE-DE&SocketPort=1024&PPort=300&WPort=49157; Mac=9E-05-40-A5-BE-DE"
        Dim weblogin2 As New WebClient()
        With weblogin2
            .Headers.Add("Accept",
                         "application/x-ms-application, image/jpeg, application/xaml+xml, image/gif, image/pjpeg, application/x-ms-xbap, application/x-shockwave-flash, */*")
            .Headers.Add("Accept-Language", "zh-CN")
            .Headers.Add("Accept-Encoding", "gzip, deflate")
            .Headers.Add("Content-Type", "application/x-www-form-urlencoded")
            .Headers.Add("User-Agent",
                         "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.2; WOW64; Trident/7.0; .NET4.0E; .NET4.0C; InfoPath.3; Media Center PC 6.0; .NET CLR 3.5.30729; .NET CLR 2.0.50727; .NET CLR 3.0.30729; WWTClient2)")
            .Headers.Add("Referer",
                         "http://cn.myet.com/ElizaWeb/Authentication/ValidateMyETUsernameNPassword.aspx?SaveAccount=N")
            .Headers.Add("Cookie", _cookielogin)
        End With
        urlauth = "http://cn.myet.com/ElizaWeb/Authentication/LoginPost.aspx?ESID=" + _aspsession + "&UserName=" + username
        webtemp = weblogin2.DownloadString(urlauth)
        _isloggedin = True
        Try
            _accountid = webtemp.Substring(694, 6)
            _ecpacid = weblogin2.ResponseHeaders.Get("Set-Cookie").Split("; path=/")(2).Substring(16, 24)
        Catch ex As Exception
            MsgBox("Login failed", MsgBoxStyle.Critical, "YourET")
            _isloggedin = False
        End Try
        weblogin2.Dispose()
        _cookiesend = "LLang=EN; TargetServerKey=CN1-LLabs; APVersion=5708; WPort=49155; LastSessionID=" + _aspsession +
                     "; MyETAccountID=" + username +
                     "; RememberMyAccount=N; LastLessonID=CN-PEP-XXQ-00033; ASP.NET_SessionId=" + _aspsession +
                     "; ContentProvider=; IsCookieSupported=Y; IsAllianceAccount=N; TmpRememberMyAccount=Y; TmpPassword=" +
                     _password + "; EcpACID=" + _ecpacid +
                     "; CourseIDInSessionInfo=PEP-XXQ-005; ConnType=Local; APArgs=ContentProvider=&Version=5708&MediaLocation=&ServerKey=&LaunchMyETArg=&MAC=9E-05-40-A5-BE-DE&SocketPort=1024&PPort=300&WPort=49157; APFixedArgs=Version=5708&MediaLocation=&MAC=9E-05-40-A5-BE-DE&SocketPort=1024&PPort=300&WPort=49157; Mac=9E-05-40-A5-BE-DE"
        Return 0
    End Function

    Public Function Logout()
        Dim weblogout As New WebClient()
        With weblogout
            .Headers.Add("Accept",
                         "application/x-ms-application, image/jpeg, application/xaml+xml, image/gif, image/pjpeg, application/x-ms-xbap, application/x-shockwave-flash, */*")
            .Headers.Add("Accept-Language", "zh-CN")
            .Headers.Add("Accept-Encoding", "gzip, deflate")
            .Headers.Add("User-Agent",
                         "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.2; WOW64; Trident/7.0; .NET4.0E; .NET4.0C; InfoPath.3; Media Center PC 6.0; .NET CLR 3.5.30729; .NET CLR 2.0.50727; .NET CLR 3.0.30729; WWTClient2)")
            .Headers.Add("Referer", "http://cn.myet.com/ElizaWeb/Home.aspx")
            .Headers.Add("Cookie", _cookielogin)
        End With
        weblogout.DownloadString("http://cn.myet.com/ElizaWeb/Logout.aspx")
        weblogout.Dispose()
        _isloggedin = False
        Return 0
    End Function

    Public Function PlusOne()
        Try
            TextBoxLesson.Text = TextBoxLesson.Text.Substring(0, 11) + Format(Val(TextBoxLesson.Text.Substring(12)) + 1, "00000")
        Catch ex As Exception
        End Try
        Return 0
    End Function

    Private Sub bwgo_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles _bwgo.DoWork
        For i As Integer = 1 To _numlessons
            If _bwgo.CancellationPending Then
                Return
            End If
            _progresslessons = i / _numlessons * 100
            _bwgo.ReportProgress(_progresslessons)
            If i < _numlessons Then
                Thread.Sleep(_timewait)
            End If
        Next
        Return
    End Sub

    Private Sub bwgo_ProgressChanged(ByVal sender As Object, ByVal e As ProgressChangedEventArgs) _
        Handles _bwgo.ProgressChanged
        RandomPlus()
        Go()
        If _progresslessons < 100 Then
            PlusOne()
        End If
        ProgressBarLessons.Value = _progresslessons
        LabelLessonsProgress.Content = Int(_progresslessons).ToString + "%"
    End Sub

    Private Sub bwgo_RunWorkerCompleted(ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs) _
        Handles _bwgo.RunWorkerCompleted
        If _isall = 0 Then
            GridBatchControl.Visibility = Visibility.Hidden
            ButtonGo.Visibility = Visibility.Visible
            ButtonOneAll.Visibility = Visibility.Visible
            ButtonLogout.Visibility = Visibility.Visible
            ButtonLogout.IsEnabled = True
            ComboBoxLessons.IsEnabled = True
            ComboBoxTime.IsEnabled = True
            If IsVisible = True Then
                MsgBox("Done")
            End If
        End If
    End Sub

    Private Sub Buttonstop_Click(sender As Object, e As RoutedEventArgs) Handles ButtonStop.Click
        If _bwall.IsBusy = True Then
            _bwall.CancelAsync()
        End If
        _bwgo.CancelAsync()
        GridBatchControl.Visibility = Visibility.Hidden
        ButtonGo.Visibility = Visibility.Visible
        ButtonOneAll.Visibility = Visibility.Visible
        ButtonLogout.Visibility = Visibility.Visible
        ButtonLogout.IsEnabled = True
        ComboBoxLessons.IsEnabled = True
        ComboBoxTime.IsEnabled = True
    End Sub

    Private Sub buttonone_Click(sender As Object, e As RoutedEventArgs) Handles ButtonOneAll.Click
        If _isall = 1 Then
            ButtonOneAll.Content = "One"
            _isall = 0
        Else
            ButtonOneAll.Content = "All"
            _isall = 1
        End If
    End Sub

    Private Sub MainWindow_Closing(sender As Object, e As CancelEventArgs)
        If _isloggedin = True Then
            Logout()
        End If
    End Sub

    Protected Overrides Sub OnMouseRightButtonDown(e As MouseButtonEventArgs)
        OnMouseRightButtonUp(e)
        _notify.Text = "YourET - " + ComboBoxUsers.Text.Replace(" ", "")
        _notify.Icon = My.Resources.yourettray
        _notify.Visible = True
        Hide()
    End Sub

    Private Sub Notify_MouseClick(sender As Object, e As MouseEventArgs) Handles _notify.Click
        Show()
        _notify.Visible = False
    End Sub

    Private Sub bwload_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles _bwload.DoWork
        Try
            Dim passwordsDataSet As PasswordsDataSet = CType(FindResource("PasswordsDataSet"), PasswordsDataSet)
            Dim passwordsDataSetTableTableAdapter As TableTableAdapter = New TableTableAdapter()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub bwload_RunWorkerCompleted(ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs) _
        Handles _bwload.RunWorkerCompleted
        Try
            Dim passwordsDataSet As PasswordsDataSet = CType(FindResource("PasswordsDataSet"), PasswordsDataSet)
            Dim passwordsDataSetTableTableAdapter As TableTableAdapter = New TableTableAdapter()
            passwordsDataSetTableTableAdapter.Fill(passwordsDataSet.Table)
            Dim tableViewSource As CollectionViewSource = CType(FindResource("TableViewSource"), CollectionViewSource)
            tableViewSource.View.MoveCurrentToFirst()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        ComboBoxUsers.IsEnabled = True
        ButtonLogin.IsEnabled = True
        LabelLoading.Visibility = Visibility.Hidden
    End Sub
End Class


