Imports MySql.Data.MySqlClient

Public Class UsersControl
    Inherits UserControl

    Private usersDataGrid As DataGridView
    Private txtName As TextBox
    Private txtUsername As TextBox
    Private txtPassword As TextBox
    Private cmbRole As ComboBox
    Private cmbStatus As ComboBox

    Public Sub New()
        InitializeComponent()
        SetupLayout()
        LoadUsers()
    End Sub

    Private Sub SetupLayout()
        ' Initialize controls
        usersDataGrid = New DataGridView With {
            .Name = "usersDataGrid",
            .Dock = DockStyle.Top,
            .Height = 200,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            .ReadOnly = True
        }

        ' Add columns to DataGridView
        usersDataGrid.Columns.Add("ID", "ID")
        usersDataGrid.Columns.Add("Name", "Name")
        usersDataGrid.Columns.Add("Username", "Username")
        usersDataGrid.Columns.Add("Role", "Role")
        usersDataGrid.Columns.Add("Status", "Status")

        ' Input fields
        Dim lblName As New Label With {.Text = "Name"}
        txtName = New TextBox With {.Name = "txtName"}

        Dim lblUsername As New Label With {.Text = "Username"}
        txtUsername = New TextBox With {.Name = "txtUsername"}

        Dim lblPassword As New Label With {.Text = "Password"}
        txtPassword = New TextBox With {.Name = "txtPassword", .PasswordChar = "*"c}

        Dim lblRole As New Label With {.Text = "Role"}
        cmbRole = New ComboBox With {.Name = "cmbRole"}
        cmbRole.Items.AddRange({"Admin", "Staff", "Clerk"})

        Dim lblStatus As New Label With {.Text = "Status"}
        cmbStatus = New ComboBox With {.Name = "cmbStatus"}
        cmbStatus.Items.AddRange({"Active", "Inactive"})

        ' Buttons
        Dim btnAdd As New Button With {.Text = "Add", .Name = "btnAdd"}
        Dim btnUpdate As New Button With {.Text = "Update", .Name = "btnUpdate"}
        Dim btnDelete As New Button With {.Text = "Delete", .Name = "btnDelete"}
        Dim btnClear As New Button With {.Text = "Clear", .Name = "btnClear"}

        ' Layout (TableLayoutPanel for alignment)
        Dim layout As New TableLayoutPanel With {
            .Dock = DockStyle.Fill,
            .ColumnCount = 2,
            .RowCount = 7,
            .Padding = New Padding(10)
        }
        layout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 30))
        layout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 70))

        ' Add controls to layout
        layout.Controls.Add(lblName, 0, 0)
        layout.Controls.Add(txtName, 1, 0)

        layout.Controls.Add(lblUsername, 0, 1)
        layout.Controls.Add(txtUsername, 1, 1)

        layout.Controls.Add(lblPassword, 0, 2)
        layout.Controls.Add(txtPassword, 1, 2)

        layout.Controls.Add(lblRole, 0, 3)
        layout.Controls.Add(cmbRole, 1, 3)

        layout.Controls.Add(lblStatus, 0, 4)
        layout.Controls.Add(cmbStatus, 1, 4)

        layout.Controls.Add(btnAdd, 0, 5)
        layout.Controls.Add(btnUpdate, 1, 5)
        layout.Controls.Add(btnDelete, 0, 6)
        layout.Controls.Add(btnClear, 1, 6)

        ' Add everything to the UserControl
        Me.Controls.Add(layout)
        Me.Controls.Add(usersDataGrid)

        ' Event Handlers for Button Clicks
        AddHandler btnAdd.Click, AddressOf btnAdd_Click
        AddHandler btnUpdate.Click, AddressOf btnUpdate_Click
        AddHandler btnDelete.Click, AddressOf btnDelete_Click
        AddHandler btnClear.Click, AddressOf btnClear_Click

        ' Event Handler for DataGridView Cell Click
        AddHandler usersDataGrid.CellClick, AddressOf usersDataGrid_CellClick
    End Sub

    Private Sub LoadUsers()
        ' Load users from the database into DataGridView
        Try
            Dim query As String = "SELECT id, name, username, role, status FROM users"
            Dim dt As DataTable = GetData(query)
            usersDataGrid.DataSource = dt
        Catch ex As Exception
            MessageBox.Show("Failed to load users: " & ex.Message)
        End Try
    End Sub

    ' Button: Add New User
    Private Sub btnAdd_Click(sender As Object, e As EventArgs)
        Dim query As String = "INSERT INTO users (name, username, password, role, status) VALUES (@name, @username, @password, @role, @status)"
        Dim parameters As New Dictionary(Of String, Object) From {
            {"@name", txtName.Text},
            {"@username", txtUsername.Text},
            {"@password", txtPassword.Text},
            {"@role", cmbRole.SelectedItem.ToString()},
            {"@status", cmbStatus.SelectedItem.ToString()}
        }
        If ExecuteQuery(query, parameters) Then
            MessageBox.Show("User added successfully!")
            LoadUsers() ' Refresh the DataGridView
            ClearForm()
        End If
    End Sub

    ' Button: Update User
    Private Sub btnUpdate_Click(sender As Object, e As EventArgs)
        If usersDataGrid.SelectedRows.Count > 0 Then
            Dim selectedRow As DataGridViewRow = usersDataGrid.SelectedRows(0)
            Dim userId As Integer = Convert.ToInt32(selectedRow.Cells("ID").Value)

            Dim query As String = "UPDATE users SET name = @name, username = @username, password = @password, role = @role, status = @status WHERE id = @id"
            Dim parameters As New Dictionary(Of String, Object) From {
                {"@name", txtName.Text},
                {"@username", txtUsername.Text},
                {"@password", txtPassword.Text},
                {"@role", cmbRole.SelectedItem.ToString()},
                {"@status", cmbStatus.SelectedItem.ToString()},
                {"@id", userId}
            }

            If ExecuteQuery(query, parameters) Then
                MessageBox.Show("User updated successfully!")
                LoadUsers() ' Refresh the DataGridView
                ClearForm()
            End If
        End If
    End Sub

    ' Button: Delete User
    Private Sub btnDelete_Click(sender As Object, e As EventArgs)
        If usersDataGrid.SelectedRows.Count > 0 Then
            Dim selectedRow As DataGridViewRow = usersDataGrid.SelectedRows(0)
            Dim userId As Integer = Convert.ToInt32(selectedRow.Cells("ID").Value)

            Dim query As String = "DELETE FROM users WHERE id = @id"
            Dim parameters As New Dictionary(Of String, Object) From {
                {"@id", userId}
            }

            If ExecuteQuery(query, parameters) Then
                MessageBox.Show("User deleted successfully!")
                LoadUsers() ' Refresh the DataGridView
            End If
        End If
    End Sub

    ' Button: Clear the form fields
    Private Sub btnClear_Click(sender As Object, e As EventArgs)
        ClearForm()
    End Sub

    ' Clear all input fields
    Private Sub ClearForm()
        txtName.Clear()
        txtUsername.Clear()
        txtPassword.Clear()
        cmbRole.SelectedIndex = -1
        cmbStatus.SelectedIndex = -1
    End Sub

    ' Handle DataGridView CellClick to load selected row data into form
    Private Sub usersDataGrid_CellClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex >= 0 Then
            Dim selectedRow As DataGridViewRow = usersDataGrid.Rows(e.RowIndex)
            txtName.Text = selectedRow.Cells("Name").Value.ToString()
            txtUsername.Text = selectedRow.Cells("Username").Value.ToString()
            cmbRole.SelectedItem = selectedRow.Cells("Role").Value.ToString()
            cmbStatus.SelectedItem = selectedRow.Cells("Status").Value.ToString()
        End If
    End Sub
End Class
