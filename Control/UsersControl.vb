Imports System.Data
Imports MySql.Data.MySqlClient

Public Class UsersControl
    Inherits UserControl

    Private usersDataGrid As DataGridView
    Private txtFullName, txtUsername, txtPassword, txtEmail, txtPhone As TextBox
    Private cmbRole, cmbStatus As ComboBox

    Public Sub New()
        InitializeComponent()
        SetupLayout()
        LoadUsers()
    End Sub

    Private Sub SetupLayout()
        ' DataGridView
        usersDataGrid = New DataGridView With {
            .Name = "usersDataGrid",
            .Dock = DockStyle.Top,
            .Height = 250,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            .ReadOnly = True,
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            .AllowUserToAddRows = False
        }


        AddHandler usersDataGrid.CellClick, AddressOf usersDataGrid_CellClick

        ' Input Controls
        txtFullName = New TextBox()
        txtUsername = New TextBox()
        txtPassword = New TextBox() With {.PasswordChar = "*"c}
        txtEmail = New TextBox()
        txtPhone = New TextBox()

        cmbRole = New ComboBox With {.DropDownStyle = ComboBoxStyle.DropDownList}
        cmbRole.Items.AddRange({"Administrator", "Staff", "Accountant/Clerk"})

        cmbStatus = New ComboBox With {.DropDownStyle = ComboBoxStyle.DropDownList}
        cmbStatus.Items.AddRange({"Active", "Inactive"})

        ' Buttons
        Dim btnAdd As New Button With {.Text = "Add"}
        AddHandler btnAdd.Click, AddressOf btnAdd_Click

        Dim btnUpdate As New Button With {.Text = "Update"}
        AddHandler btnUpdate.Click, AddressOf btnUpdate_Click

        Dim btnDelete As New Button With {.Text = "Delete"}
        AddHandler btnDelete.Click, AddressOf btnDelete_Click

        Dim btnClear As New Button With {.Text = "Clear"}
        AddHandler btnClear.Click, AddressOf btnClear_Click

        ' Layout
        Dim layout As New TableLayoutPanel With {
            .Dock = DockStyle.Fill,
            .ColumnCount = 2,
            .RowCount = 9,
            .Padding = New Padding(10)
        }
        layout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 30))
        layout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 70))

        layout.Controls.Add(New Label With {.Text = "Full Name"}, 0, 0)
        layout.Controls.Add(txtFullName, 1, 0)

        layout.Controls.Add(New Label With {.Text = "Username"}, 0, 1)
        layout.Controls.Add(txtUsername, 1, 1)

        layout.Controls.Add(New Label With {.Text = "Password"}, 0, 2)
        layout.Controls.Add(txtPassword, 1, 2)

        layout.Controls.Add(New Label With {.Text = "Role"}, 0, 3)
        layout.Controls.Add(cmbRole, 1, 3)

        layout.Controls.Add(New Label With {.Text = "Email"}, 0, 4)
        layout.Controls.Add(txtEmail, 1, 4)

        layout.Controls.Add(New Label With {.Text = "Phone Number"}, 0, 5)
        layout.Controls.Add(txtPhone, 1, 5)

        layout.Controls.Add(New Label With {.Text = "Status"}, 0, 6)
        layout.Controls.Add(cmbStatus, 1, 6)

        layout.Controls.Add(btnAdd, 0, 7)
        layout.Controls.Add(btnUpdate, 1, 7)
        layout.Controls.Add(btnDelete, 0, 8)
        layout.Controls.Add(btnClear, 1, 8)

        ' Add controls to UserControl
        Me.Controls.Add(layout)
        Me.Controls.Add(usersDataGrid)
    End Sub

    Private Sub LoadUsers()
        Try
            Dim query As String = "SELECT UserID, FullName, Username, UserLevelName, Email, PhoneNumber, Status FROM user"
            Dim dt As DataTable = GetData(query)
            usersDataGrid.DataSource = dt
        Catch ex As Exception
            MessageBox.Show("Failed to load users: " & ex.Message)
        End Try
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs)
        Dim hasher As New PasswordHasher(50000, 16, "SHA256")

        Dim passwordResult = hasher.HashPassword(txtPassword.Text)

        Dim query As String = "
        INSERT INTO user 
        (FullName, Username, Password, Salt, UserLevelName, Email, PhoneNumber, Status, CreatedAt) 
        VALUES 
        (@FullName, @Username, @Password, @Salt, @UserLevelName, @Email, @PhoneNumber, @Status, NOW())"

        Dim parameters As New Dictionary(Of String, Object) From {
        {"@FullName", txtFullName.Text},
        {"@Username", txtUsername.Text},
        {"@Password", passwordResult.HashBase64},
        {"@Salt", passwordResult.SaltBase64},
        {"@UserLevelName", cmbRole.Text},
        {"@Email", txtEmail.Text},
        {"@PhoneNumber", txtPhone.Text},
        {"@Status", cmbStatus.Text}
    }

        If ExecuteQuery(query, parameters) Then
            MessageBox.Show("User added successfully.")
            LoadUsers()
            ClearFields()
        End If
    End Sub


    Private Sub UsersControl_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs)
        If usersDataGrid.SelectedRows.Count = 0 Then
            MessageBox.Show("Select a user to update.")
            Return
        End If

        Dim userId As Integer = Convert.ToInt32(usersDataGrid.SelectedRows(0).Cells("UserID").Value)
        Dim query As String = "UPDATE user SET FullName=@FullName, Username=@Username, UserLevelName=@UserLevelName, Email=@Email, PhoneNumber=@PhoneNumber, Status=@Status"
        Dim parameters As New Dictionary(Of String, Object) From {
        {"@UserID", userId},
        {"@FullName", txtFullName.Text},
        {"@Username", txtUsername.Text},
        {"@UserLevelName", cmbRole.Text},
        {"@Email", txtEmail.Text},
        {"@PhoneNumber", txtPhone.Text},
        {"@Status", cmbStatus.Text}
    }

        ' Only update password & salt if a new password is entered
        If Not String.IsNullOrWhiteSpace(txtPassword.Text) Then
            Dim hasher As New PasswordHasher(50000, 16, "SHA256")
            Dim passwordResult = hasher.HashPassword(txtPassword.Text)

            query &= ", Password=@Password, Salt=@Salt"
            parameters.Add("@Password", passwordResult.HashBase64)
            parameters.Add("@Salt", passwordResult.SaltBase64)
        End If

        ' Final WHERE clause
        query &= " WHERE UserID=@UserID"

        If ExecuteQuery(query, parameters) Then
            MessageBox.Show("User updated successfully.")
            LoadUsers()
            ClearFields()
        End If
    End Sub


    Private Sub btnDelete_Click(sender As Object, e As EventArgs)
        If usersDataGrid.SelectedRows.Count = 0 Then
            MessageBox.Show("Select a user to delete.")
            Return
        End If

        Dim userId As Integer = Convert.ToInt32(usersDataGrid.SelectedRows(0).Cells("UserID").Value)
        Dim query As String = "DELETE FROM user WHERE UserID=@UserID"
        Dim parameters As New Dictionary(Of String, Object) From {
            {"@UserID", userId}
        }

        If MessageBox.Show("Are you sure you want to delete this user?", "Confirm", MessageBoxButtons.YesNo) = DialogResult.Yes Then
            If ExecuteQuery(query, parameters) Then
                MessageBox.Show("User deleted successfully.")
                LoadUsers()
                ClearFields()
            End If
        End If
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs)
        ClearFields()
    End Sub

    Private Sub ClearFields()
        txtFullName.Clear()
        txtUsername.Clear()
        txtPassword.Clear()
        txtEmail.Clear()
        txtPhone.Clear()
        cmbRole.SelectedIndex = -1
        cmbStatus.SelectedIndex = -1
    End Sub

    Private Sub usersDataGrid_CellClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex >= 0 Then
            Dim row As DataGridViewRow = usersDataGrid.Rows(e.RowIndex)
            txtFullName.Text = row.Cells("FullName").Value.ToString()
            txtUsername.Text = row.Cells("Username").Value.ToString()
            txtPassword.Clear()
            cmbRole.Text = row.Cells("UserLevelName").Value.ToString()
            txtEmail.Text = row.Cells("Email").Value.ToString()
            txtPhone.Text = row.Cells("PhoneNumber").Value.ToString()
            cmbStatus.Text = row.Cells("Status").Value.ToString()
        End If
    End Sub
End Class
