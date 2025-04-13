Imports System.Data
Imports MySql.Data.MySqlClient

Public Class StaffCustomerControl
    Inherits UserControl

    Private customerGrid As DataGridView
    Private txtSearch As TextBox
    Private txtFirstName, txtLastName, txtMiddleName, txtContact, txtEmail, txtAddress, txtCity, txtProvince, txtZip As TextBox
    Private cmbGender, cmbCustomerType, cmbStatus As ComboBox
    Private dtpBirthDate As DateTimePicker

    Public Sub New()
        InitializeComponent()
        SetupLayout()
        LoadCustomers()
    End Sub

    Private Sub SetupLayout()
        ' Data Grid
        customerGrid = New DataGridView With {
            .Dock = DockStyle.Top,
            .Height = 250,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            .ReadOnly = True,
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            .AllowUserToAddRows = False
        }
        AddHandler customerGrid.CellClick, AddressOf customerGrid_CellClick

        ' Search bar
        Dim searchPanel As New Panel With {.Dock = DockStyle.Top, .Height = 50}
        Dim lblSearch As New Label With {.Text = "Search Customers:", .Dock = DockStyle.Top, .Height = 20}
        txtSearch = New TextBox With {.Dock = DockStyle.Top}
        AddHandler txtSearch.TextChanged, AddressOf txtSearch_TextChanged

        searchPanel.Controls.Add(txtSearch)
        searchPanel.Controls.Add(lblSearch)

        ' Input fields
        txtFirstName = New TextBox()
        txtLastName = New TextBox()
        txtMiddleName = New TextBox()
        txtContact = New TextBox()
        txtEmail = New TextBox()
        txtAddress = New TextBox()
        txtCity = New TextBox()
        txtProvince = New TextBox()
        txtZip = New TextBox()

        cmbGender = New ComboBox With {.DropDownStyle = ComboBoxStyle.DropDownList}
        cmbGender.Items.AddRange({"Male", "Female", "Other"})

        cmbCustomerType = New ComboBox With {.DropDownStyle = ComboBoxStyle.DropDownList}
        cmbCustomerType.Items.AddRange({"Regular", "Walk-in", "Corporate"})

        cmbStatus = New ComboBox With {.DropDownStyle = ComboBoxStyle.DropDownList}
        cmbStatus.Items.AddRange({"Active", "Inactive"})

        dtpBirthDate = New DateTimePicker()

        ' Buttons
        Dim btnAdd As New Button With {.Text = "Register"}
        AddHandler btnAdd.Click, AddressOf btnAdd_Click

        Dim btnUpdate As New Button With {.Text = "Update"}
        AddHandler btnUpdate.Click, AddressOf btnUpdate_Click

        Dim btnClear As New Button With {.Text = "Clear"}
        AddHandler btnClear.Click, AddressOf btnClear_Click

        ' Layout
        Dim layout As New TableLayoutPanel With {
            .Dock = DockStyle.Fill,
            .ColumnCount = 2,
            .RowCount = 14,
            .Padding = New Padding(10)
        }

        layout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 30))
        layout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 70))

        layout.Controls.Add(New Label With {.Text = "First Name"}, 0, 0)
        layout.Controls.Add(txtFirstName, 1, 0)

        layout.Controls.Add(New Label With {.Text = "Last Name"}, 0, 1)
        layout.Controls.Add(txtLastName, 1, 1)

        layout.Controls.Add(New Label With {.Text = "Middle Name"}, 0, 2)
        layout.Controls.Add(txtMiddleName, 1, 2)

        layout.Controls.Add(New Label With {.Text = "Gender"}, 0, 3)
        layout.Controls.Add(cmbGender, 1, 3)

        layout.Controls.Add(New Label With {.Text = "Birth Date"}, 0, 4)
        layout.Controls.Add(dtpBirthDate, 1, 4)

        layout.Controls.Add(New Label With {.Text = "Contact Number"}, 0, 5)
        layout.Controls.Add(txtContact, 1, 5)

        layout.Controls.Add(New Label With {.Text = "Email"}, 0, 6)
        layout.Controls.Add(txtEmail, 1, 6)

        layout.Controls.Add(New Label With {.Text = "Address"}, 0, 7)
        layout.Controls.Add(txtAddress, 1, 7)

        layout.Controls.Add(New Label With {.Text = "City"}, 0, 8)
        layout.Controls.Add(txtCity, 1, 8)

        layout.Controls.Add(New Label With {.Text = "Province"}, 0, 9)
        layout.Controls.Add(txtProvince, 1, 9)

        layout.Controls.Add(New Label With {.Text = "Zip Code"}, 0, 10)
        layout.Controls.Add(txtZip, 1, 10)

        layout.Controls.Add(New Label With {.Text = "Customer Type"}, 0, 11)
        layout.Controls.Add(cmbCustomerType, 1, 11)

        layout.Controls.Add(New Label With {.Text = "Status"}, 0, 13)
        layout.Controls.Add(cmbStatus, 1, 13)

        ' Bottom panel
        Dim bottomPanel As New FlowLayoutPanel With {.Dock = DockStyle.Bottom, .FlowDirection = FlowDirection.LeftToRight}
        bottomPanel.Controls.AddRange({btnAdd, btnUpdate, btnClear})

        Me.Controls.Add(layout)
        Me.Controls.Add(searchPanel)
        Me.Controls.Add(customerGrid)
        Me.Controls.Add(bottomPanel)
    End Sub

    Private Sub LoadCustomers()
        Dim query As String = "SELECT * FROM customer"
        customerGrid.DataSource = GetData(query)
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs)
        Dim query As String = "INSERT INTO customer (FirstName, LastName, MiddleName, Gender, BirthDate, ContactNumber, Email, AddressLine, City, Province, ZipCode, CustomerType, Status)
                               VALUES (@FirstName, @LastName, @MiddleName, @Gender, @BirthDate, @ContactNumber, @Email, @AddressLine, @City, @Province, @ZipCode, @CustomerType, @Status)"

        Dim parameters As New Dictionary(Of String, Object) From {
            {"@FirstName", txtFirstName.Text},
            {"@LastName", txtLastName.Text},
            {"@MiddleName", txtMiddleName.Text},
            {"@Gender", cmbGender.Text},
            {"@BirthDate", dtpBirthDate.Value},
            {"@ContactNumber", txtContact.Text},
            {"@Email", txtEmail.Text},
            {"@AddressLine", txtAddress.Text},
            {"@City", txtCity.Text},
            {"@Province", txtProvince.Text},
            {"@ZipCode", txtZip.Text},
            {"@CustomerType", cmbCustomerType.Text},
            {"@Status", cmbStatus.Text}
        }

        If ExecuteQuery(query, parameters) Then
            MessageBox.Show("Customer registered.")
            LoadCustomers()
            ClearFields()
        End If
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs)
        If customerGrid.SelectedRows.Count = 0 Then
            MessageBox.Show("Select a customer to update.")
            Return
        End If

        Dim customerID As Integer = Convert.ToInt32(customerGrid.SelectedRows(0).Cells("CustomerID").Value)
        Dim query As String = "UPDATE customer SET FirstName=@FirstName, LastName=@LastName, MiddleName=@MiddleName,
                               Gender=@Gender, BirthDate=@BirthDate, ContactNumber=@ContactNumber, Email=@Email,
                               AddressLine=@AddressLine, City=@City, Province=@Province, ZipCode=@ZipCode,
                               CustomerType=@CustomerType, Status=@Status
                               WHERE CustomerID=@CustomerID"

        Dim parameters As New Dictionary(Of String, Object) From {
            {"@CustomerID", customerID},
            {"@FirstName", txtFirstName.Text},
            {"@LastName", txtLastName.Text},
            {"@MiddleName", txtMiddleName.Text},
            {"@Gender", cmbGender.Text},
            {"@BirthDate", dtpBirthDate.Value},
            {"@ContactNumber", txtContact.Text},
            {"@Email", txtEmail.Text},
            {"@AddressLine", txtAddress.Text},
            {"@City", txtCity.Text},
            {"@Province", txtProvince.Text},
            {"@ZipCode", txtZip.Text},
            {"@CustomerType", cmbCustomerType.Text},
            {"@Status", cmbStatus.Text}
        }

        If ExecuteQuery(query, parameters) Then
            MessageBox.Show("Customer updated.")
            LoadCustomers()
            ClearFields()
        End If
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs)
        Dim searchText As String = txtSearch.Text.Trim()
        Dim query As String = "SELECT * FROM customer WHERE FirstName LIKE @search OR LastName LIKE @search OR MiddleName LIKE @search"
        Dim parameters As New Dictionary(Of String, Object) From {
            {"@search", "%" & searchText & "%"}
        }
        customerGrid.DataSource = GetData(query, parameters)
    End Sub

    Private Sub customerGrid_CellClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex >= 0 Then
            Dim row = customerGrid.Rows(e.RowIndex)
            txtFirstName.Text = row.Cells("FirstName").Value.ToString()
            txtLastName.Text = row.Cells("LastName").Value.ToString()
            txtMiddleName.Text = row.Cells("MiddleName").Value.ToString()
            cmbGender.Text = row.Cells("Gender").Value.ToString()
            dtpBirthDate.Value = Convert.ToDateTime(row.Cells("BirthDate").Value)
            txtContact.Text = row.Cells("ContactNumber").Value.ToString()
            txtEmail.Text = row.Cells("Email").Value.ToString()
            txtAddress.Text = row.Cells("AddressLine").Value.ToString()
            txtCity.Text = row.Cells("City").Value.ToString()
            txtProvince.Text = row.Cells("Province").Value.ToString()
            txtZip.Text = row.Cells("ZipCode").Value.ToString()
            cmbCustomerType.Text = row.Cells("CustomerType").Value.ToString()
            cmbStatus.Text = row.Cells("Status").Value.ToString()
        End If
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs)
        ClearFields()
        customerGrid.ClearSelection()
    End Sub


    Private Sub ClearFields()
        txtFirstName.Clear()
        txtLastName.Clear()
        txtMiddleName.Clear()
        txtContact.Clear()
        txtEmail.Clear()
        txtAddress.Clear()
        txtCity.Clear()
        txtProvince.Clear()
        txtZip.Clear()
        cmbCustomerType.SelectedIndex = -1
        cmbStatus.SelectedIndex = -1
        cmbGender.SelectedIndex = -1
        dtpBirthDate.Value = DateTime.Now
    End Sub
End Class
