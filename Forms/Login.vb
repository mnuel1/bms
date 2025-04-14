Imports System.Security.Cryptography
Imports System.Text

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
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
    End Sub

    Private Sub loginBtn_Click(sender As Object, e As EventArgs) Handles loginBtn.Click
        Dim username As String = usernameTextBox.Text.Trim().ToLower()
        Dim password As String = passwordTextbox.Text

        ' Step 1: Fetch user by username
        Dim query As String = "SELECT * FROM user WHERE LOWER(Username) = @Username AND Status = 'Active'"
        Dim parameters As New Dictionary(Of String, Object) From {
        {"@Username", username}
    }

        Dim dt As DataTable = GetData(query, parameters)

        ' Step 2: Check if user exists
        If dt.Rows.Count = 1 Then
            Dim userRow As DataRow = dt.Rows(0)
            Dim storedHash As String = userRow("Password").ToString()
            Dim storedSalt As String = userRow("Salt").ToString()

            ' Create a new instance of PasswordHasher (same params used when hashing)
            Dim hasher As New PasswordHasher(50000, 16, "SHA256")

            ' Step 3: Verify password
            If hasher.VerifyPassword(password, storedHash, storedSalt) Then
                ' ✅ Login success — store session info
                SessionInfo.LoggedInUserID = Convert.ToInt32(userRow("UserID"))
                SessionInfo.LoggedInUserFullName = userRow("FullName").ToString()
                SessionInfo.LoggedInUserLevel = userRow("UserLevelName").ToString()

                ' 🗒️ Insert login log
                Dim logQuery As String = "INSERT INTO login_logs (UserID, UserType, Name, Date, Time) VALUES (@UserID, @UserType, @Name, @Date, @Time)"
                Dim logParams As New Dictionary(Of String, Object) From {
                {"@UserID", SessionInfo.LoggedInUserID},
                {"@UserType", SessionInfo.LoggedInUserLevel},
                {"@Name", SessionInfo.LoggedInUserFullName},
                {"@Date", Date.Now.ToString("yyyy-MM-dd")},
                {"@Time", Date.Now.ToString("HH:mm:ss")}
            }
                ExecuteQuery(logQuery, logParams)

                ' ✅ Redirect based on user level
                Select Case SessionInfo.LoggedInUserLevel
                    Case "Administrator"
                        AdminDashboard.Show()
                    Case "Accountant/Clerk"
                        ClerkDashboard.Show()
                    Case "Staff"
                        StaffDashboard.Show()
                    Case Else
                        MessageBox.Show("Unknown user level.")
                        Return
                End Select

                Me.Hide()
            Else
                MessageBox.Show("Invalid password.")
            End If
        Else
            MessageBox.Show("Invalid username or inactive account.")
        End If
    End Sub




    Private Sub SplitContainer1_Panel2_Paint(sender As Object, e As PaintEventArgs) Handles SplitContainer1.Panel2.Paint

    End Sub
End Class
