Imports System.Security.Cryptography
Imports System.Text
Imports MySql.Data.MySqlClient
Imports System.Drawing.Drawing2D

Public Class PasswordHasher

    Private ReadOnly _iterations As Integer
        Private ReadOnly _saltBytes As Integer
        Private ReadOnly _hashAlgorithm As String

        ''' <summary>
        ''' Initializes a new instance of the PasswordHasher class.
        ''' </summary>
        ''' <param name="iterations">Number of PBKDF2 iterations (default: 50000).</param>
        ''' <param name="saltBytes">Length of the random salt in bytes (default: 16).</param>
        ''' <param name="hashAlgorithm">Hash algorithm for PBKDF2 (default: SHA256).</param>
        Public Sub New(Optional iterations As Integer = 50000, Optional saltBytes As Integer = 16, Optional hashAlgorithm As String = "SHA256")
            If iterations <= 0 Then Throw New ArgumentException("Iterations must be positive.", NameOf(iterations))
            If saltBytes <= 0 Then Throw New ArgumentException("Salt bytes must be positive.", NameOf(saltBytes))
            If String.IsNullOrEmpty(hashAlgorithm) Then Throw New ArgumentException("Hash algorithm cannot be empty.", NameOf(hashAlgorithm))

            _iterations = iterations
            _saltBytes = saltBytes
            _hashAlgorithm = hashAlgorithm
        End Sub

        ''' <summary>
        ''' Hashes a password using PBKDF2 and returns base64-encoded hash and salt.
        ''' </summary>
        ''' <param name="password">The password to hash.</param>
        ''' <returns>Tuple of (hashBase64, saltBase64).</returns>
        ''' <exception cref="ArgumentException">Thrown if password is null or empty.</exception>
        Public Function HashPassword(password As String) As (HashBase64 As String, SaltBase64 As String)
            If String.IsNullOrEmpty(password) Then Throw New ArgumentException("Password cannot be null or empty.", NameOf(password))

            ' Generate random salt
            Dim salt As Byte()
            Using rng = RandomNumberGenerator.Create()
                salt = New Byte(_saltBytes - 1) {}
                rng.GetBytes(salt)
            End Using

            ' Hash password with PBKDF2
            Using pbkdf2 = New Rfc2898DeriveBytes(password, salt, _iterations, HashAlgorithmName.SHA256)
                Dim hashBytes = pbkdf2.GetBytes(32) ' 32 bytes for SHA256
                Dim hashBase64 = Convert.ToBase64String(hashBytes)
                Dim saltBase64 = Convert.ToBase64String(salt)
                Return (hashBase64, saltBase64)
            End Using
        End Function

        ''' <summary>
        ''' Verifies a password against a stored hash and salt.
        ''' </summary>
        ''' <param name="password">The password to verify.</param>
        ''' <param name="storedHashBase64">Base64-encoded stored hash.</param>
        ''' <param name="storedSaltBase64">Base64-encoded stored salt.</param>
        ''' <returns>True if the password matches, False otherwise.</returns>
        Public Function VerifyPassword(password As String, storedHashBase64 As String, storedSaltBase64 As String) As Boolean
            If String.IsNullOrEmpty(password) OrElse String.IsNullOrEmpty(storedHashBase64) OrElse String.IsNullOrEmpty(storedSaltBase64) Then
                Return False
            End If

            Try
                ' Decode stored hash and salt
                Dim storedHash = Convert.FromBase64String(storedHashBase64)
                Dim salt = Convert.FromBase64String(storedSaltBase64)

                ' Recompute hash
                Using pbkdf2 = New Rfc2898DeriveBytes(password, salt, _iterations, HashAlgorithmName.SHA256)
                    Dim hashBytes = pbkdf2.GetBytes(32) ' 32 bytes for SHA256
                    ' Compare hashes
                    Return hashBytes.SequenceEqual(storedHash)
                End Using
            Catch ex As FormatException
                ' Invalid base64
                Return False
            Catch ex As ArgumentException
                ' Invalid parameters
                Return False
            End Try
        End Function
    End Class


Public Class Login
    Private Sub Login_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            ' Apply form styling
            Me.BackColor = Color.FromArgb(245, 246, 245) ' Off-white
            Me.FormBorderStyle = FormBorderStyle.FixedSingle
            Me.WindowState = FormWindowState.Maximized

            ' Center panel
            CenterPanel()

            ' Style textboxes
            For Each txt In {usernameTextBox, passwordTextbox}
                txt.BackColor = Color.FromArgb(232, 246, 239) ' Soft mint
                txt.ForeColor = Color.FromArgb(51, 51, 51)
                txt.Font = New Font("Comic Sans MS", 12.0!)
                txt.BorderStyle = BorderStyle.None ' Remove default border
                MakeTextBoxRounded(txt)
            Next

            ' Style buttons
            loginBtn.BackColor = Color.FromArgb(168, 230, 207) ' Mint green
            loginBtn.ForeColor = Color.FromArgb(51, 51, 51)
            loginBtn.FlatStyle = FlatStyle.Flat
            loginBtn.FlatAppearance.BorderSize = 0
            loginBtn.Font = New Font("Comic Sans MS", 12, FontStyle.Bold)

            btnCancel.BackColor = Color.FromArgb(200, 200, 200) ' Light gray
            btnCancel.ForeColor = Color.FromArgb(51, 51, 51)
            btnCancel.FlatStyle = FlatStyle.Flat
            btnCancel.FlatAppearance.BorderSize = 0
            btnCancel.Font = New Font("Comic Sans MS", 12, FontStyle.Bold)

            ' Add hover effects
            AddHandler loginBtn.MouseEnter, Sub() loginBtn.BackColor = Color.FromArgb(148, 210, 187)
            AddHandler loginBtn.MouseLeave, Sub() loginBtn.BackColor = Color.FromArgb(168, 230, 207)
            AddHandler btnCancel.MouseEnter, Sub() btnCancel.BackColor = Color.FromArgb(180, 180, 180)
            AddHandler btnCancel.MouseLeave, Sub() btnCancel.BackColor = Color.FromArgb(200, 200, 200)

            ' Make buttons rounded
            MakeButtonRounded(loginBtn)
            MakeButtonRounded(btnCancel)

            ' Set default status
            lblStatus.Text = ""
        Catch ex As Exception
            MessageBox.Show("Error in Load: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Close()
        End Try
    End Sub

    Private Sub Login_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        CenterPanel()
    End Sub

    Private Sub CenterPanel()
        If pnlContainer IsNot Nothing Then
            Dim x As Integer = (Me.ClientSize.Width - pnlContainer.Width) \ 2
            Dim y As Integer = (Me.ClientSize.Height - pnlContainer.Height) \ 2
            pnlContainer.Location = New Point(Math.Max(x, 0), Math.Max(y, 0))
        End If
    End Sub

    Private Sub MakeButtonRounded(btn As Button)
        Dim path As New GraphicsPath()
        Dim radius As Integer = 20
        Dim rect As New Rectangle(0, 0, btn.Width, btn.Height)
        path.AddArc(rect.X, rect.Y, radius, radius, 180, 90)
        path.AddArc(rect.Width - radius, rect.Y, radius, radius, 270, 90)
        path.AddArc(rect.Width - radius, rect.Height - radius, radius, radius, 0, 90)
        path.AddArc(rect.X, rect.Height - radius, radius, radius, 90, 90)
        path.CloseFigure()
        btn.Region = New Region(path)
    End Sub

    Private Sub MakeTextBoxRounded(txt As TextBox)
        Dim path As New GraphicsPath()
        Dim radius As Integer = 10
        Dim rect As New Rectangle(0, 0, txt.Width, txt.Height)
        path.AddArc(rect.X, rect.Y, radius, radius, 180, 90)
        path.AddArc(rect.Width - radius, rect.Y, radius, radius, 270, 90)
        path.AddArc(rect.Width - radius, rect.Height - radius, radius, radius, 0, 90)
        path.AddArc(rect.X, rect.Height - radius, radius, radius, 90, 90)
        path.CloseFigure()
        txt.Region = New Region(path)
    End Sub

    Private Sub loginBtn_Click(sender As Object, e As EventArgs) Handles loginBtn.Click
        Try
            Dim username As String = usernameTextBox.Text.Trim().ToLower()
            Dim password As String = passwordTextbox.Text

            If String.IsNullOrEmpty(username) OrElse String.IsNullOrEmpty(password) Then
                lblStatus.Text = "Please enter both username and password."
                lblStatus.ForeColor = Color.FromArgb(255, 0, 0)
                Return
            End If

            ' Fetch user by username
            Dim query As String = "SELECT * FROM user WHERE LOWER(Username) = @Username AND Status = 'Active'"
            Using conn As New MySqlConnection(DatabaseHelper.connString)
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@Username", username)
                    Dim dt As New DataTable()
                    conn.Open()
                    Using adapter As New MySqlDataAdapter(cmd)
                        adapter.Fill(dt)
                    End Using

                    ' Check if user exists
                    If dt.Rows.Count = 1 Then
                        Dim userRow As DataRow = dt.Rows(0)
                        Dim storedHash As String = userRow("Password").ToString()
                        Dim storedSalt As String = userRow("Salt").ToString()

                        ' Verify password
                        Dim hasher As New PasswordHasher(50000, 16, "SHA256")
                        If hasher.VerifyPassword(password, storedHash, storedSalt) Then
                            ' Login success - store session info
                            SessionInfo.LoggedInUserID = Convert.ToInt32(userRow("UserID"))
                            SessionInfo.LoggedInUserFullName = userRow("FullName").ToString()
                            SessionInfo.LoggedInUserLevel = userRow("UserLevelName").ToString()

                            ' Insert login log
                            Dim logQuery As String = "INSERT INTO login_logs (UserID, UserType, Name, Date, Time) VALUES (@UserID, @UserType, @Name, @Date, @Time)"
                            Using logCmd As New MySqlCommand(logQuery, conn)
                                logCmd.Parameters.AddWithValue("@UserID", SessionInfo.LoggedInUserID)
                                logCmd.Parameters.AddWithValue("@UserType", SessionInfo.LoggedInUserLevel)
                                logCmd.Parameters.AddWithValue("@Name", SessionInfo.LoggedInUserFullName)
                                logCmd.Parameters.AddWithValue("@Date", Date.Now.ToString("yyyy-MM-dd"))
                                logCmd.Parameters.AddWithValue("@Time", Date.Now.ToString("HH:mm:ss"))
                                logCmd.ExecuteNonQuery()
                            End Using

                            ' Redirect based on user level
                            Select Case SessionInfo.LoggedInUserLevel
                                Case "Administrator"
                                    Dim adminForm As New AdminDashboard()
                                    adminForm.Show()
                                Case "Accountant/Clerk"
                                    Dim clerkForm As New ClerkDashboard()
                                    clerkForm.Show()
                                Case "Staff"
                                    Dim staffForm As New StaffDashboard()
                                    staffForm.Show()
                                Case Else
                                    lblStatus.Text = "Unknown user level."
                                    lblStatus.ForeColor = Color.FromArgb(255, 0, 0)
                                    Return
                            End Select

                            lblStatus.Text = "Login successful!"
                            lblStatus.ForeColor = Color.FromArgb(0, 128, 0)
                            Me.Hide()
                        Else
                            lblStatus.Text = "Invalid password."
                            lblStatus.ForeColor = Color.FromArgb(255, 0, 0)
                        End If
                    Else
                        lblStatus.Text = "Invalid username or inactive account."
                        lblStatus.ForeColor = Color.FromArgb(255, 0, 0)
                    End If
                End Using
            End Using
        Catch ex As Exception
            lblStatus.Text = "Login failed: " & ex.Message
            lblStatus.ForeColor = Color.FromArgb(255, 0, 0)
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
End Class
