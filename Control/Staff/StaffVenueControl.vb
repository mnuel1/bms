Imports System.Data
Imports MySql.Data.MySqlClient
Imports System.Drawing.Drawing2D

Public Class StaffVenueControl
    Inherits UserControl

    Private dgvVenueAvailability As DataGridView
    Private WithEvents btnRefresh As Button
    Private colorAvailable As Color = Color.FromArgb(220, 247, 220)  ' Light green
    Private colorUnavailable As Color = Color.FromArgb(255, 228, 228) ' Light red
    Private headerColor As Color = Color.FromArgb(130, 204, 221)      ' Light blue

    Public Sub New()
        InitializeComponent()
        SetupLayout()
        ApplyStyles()
        LoadVenueAvailability()
    End Sub

    Private Sub StaffVenueControl_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Additional logic if needed when control loads
    End Sub

    Private Sub SetupLayout()
        ' Main container Panel with rounded corners and padding
        Dim mainPanel As New Panel With {
            .Dock = DockStyle.Fill,
            .Padding = New Padding(15),
            .BackColor = Color.White
        }

        ' Header Panel with gradient background
        Dim headerPanel As New Panel With {
            .Dock = DockStyle.Top,
            .Height = 60,
            .Padding = New Padding(15, 10, 15, 10)
        }

        ' Title with cute icon
        Dim lblHeader As New Label With {
            .Text = "🏢 Venue Availability",
            .Dock = DockStyle.Left,
            .Font = New Font("Segoe UI", 16, FontStyle.Bold),
            .ForeColor = Color.FromArgb(70, 70, 70),
            .AutoSize = True
        }

        ' Refresh button with icon
        btnRefresh = New Button With {
            .Text = "↻ Refresh",
            .Dock = DockStyle.Right,
            .FlatStyle = FlatStyle.Flat,
            .Font = New Font("Segoe UI", 10),
            .Width = 110,
            .Cursor = Cursors.Hand
        }

        ' Enhanced DataGridView
        dgvVenueAvailability = New DataGridView With {
            .Dock = DockStyle.Fill,
            .ReadOnly = True,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            .AllowUserToAddRows = False,
            .BorderStyle = BorderStyle.None,
            .CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
            .RowHeadersVisible = False,
            .EnableHeadersVisualStyles = False,
            .AllowUserToResizeRows = False
        }

        ' Legend panel at the bottom
        Dim legendPanel As New FlowLayoutPanel With {
            .Dock = DockStyle.Bottom,
            .Height = 40,
            .FlowDirection = FlowDirection.LeftToRight,
            .Padding = New Padding(10, 5, 10, 5)
        }

        ' Available Legend
        Dim availableLegend As New Panel With {
            .BackColor = colorAvailable,
            .Size = New Size(20, 20),
            .Margin = New Padding(0, 0, 5, 0)
        }

        Dim lblAvailable As New Label With {
            .Text = "Available",
            .AutoSize = True,
            .Font = New Font("Segoe UI", 9),
            .Margin = New Padding(0, 0, 20, 0)
        }

        ' Unavailable Legend
        Dim unavailableLegend As New Panel With {
            .BackColor = colorUnavailable,
            .Size = New Size(20, 20),
            .Margin = New Padding(0, 0, 5, 0)
        }

        Dim lblUnavailable As New Label With {
            .Text = "Unavailable",
            .AutoSize = True,
            .Font = New Font("Segoe UI", 9)
        }

        ' Add controls to the form
        headerPanel.Controls.Add(lblHeader)
        headerPanel.Controls.Add(btnRefresh)

        legendPanel.Controls.Add(availableLegend)
        legendPanel.Controls.Add(lblAvailable)
        legendPanel.Controls.Add(unavailableLegend)
        legendPanel.Controls.Add(lblUnavailable)

        mainPanel.Controls.Add(dgvVenueAvailability)
        mainPanel.Controls.Add(legendPanel)
        mainPanel.Controls.Add(headerPanel)

        Me.Controls.Add(mainPanel)
    End Sub

    Private Sub ApplyStyles()
        ' Style the DataGridView
        With dgvVenueAvailability
            ' Header styling
            .ColumnHeadersDefaultCellStyle.BackColor = headerColor
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.White
            .ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI Semibold", 10)
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            .ColumnHeadersHeight = 40

            ' Rows styling
            .DefaultCellStyle.Font = New Font("Segoe UI", 9.5)
            .DefaultCellStyle.Padding = New Padding(5)
            .AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250)
            .RowTemplate.Height = 35

            ' Selection styling
            .DefaultCellStyle.SelectionBackColor = Color.FromArgb(220, 236, 255)  ' Light blue selection
            .DefaultCellStyle.SelectionForeColor = Color.Black
        End With

        ' Refresh button styling
        With btnRefresh
            .BackColor = Color.FromArgb(130, 204, 221)
            .ForeColor = Color.White
            .FlatAppearance.BorderColor = Color.FromArgb(100, 180, 200)
            .FlatAppearance.BorderSize = 1
        End With
    End Sub

    Private Sub LoadVenueAvailability()
        ' Hardcoded venue ENUM values (update if needed)
        Dim venues As String() = {
            "Albay Astrodome",
            "Legazpi Convention Center",
            "Ibalong Centrum for Recreation",
            "Penaranda Park",
            "Cagsawa Ruins Park",
            "Lignon Hill Nature Park",
            "Pacific Mall Event Center",
            "Embarcadero de Legazpi",
            "Avenue Plaza Hotel",
            "CWC (Camsur Watersports Complex)",
            "Villa Caceres Hotel",
            "Biggs Diner Function Hall",
            "Naga City Civic Center",
            "Bicol University Gymnasium",
            "Jardin Real de Naga",
            "Ateneo de Naga University Gym",
            "Sorsogon Capitol Park",
            "Rizal Beach Resort",
            "Misibis Bay Resort",
            "Balay Cena Una",
            "Hotel Venezia",
            "Doña Mercedes Country Lodge"
        }

        ' Query to get unavailable venues and their latest EndTime
        Dim query As String = "
            SELECT VenueLocation, MAX(CONCAT(EventDate, ' ', EndTime)) AS UntilTime
            FROM event
            WHERE EventDate >= CURDATE()            
            GROUP BY VenueLocation
        "

        Dim bookedVenues As DataTable = GetData(query)
        Dim availabilityTable As New DataTable()
        availabilityTable.Columns.Add("Venue")
        availabilityTable.Columns.Add("Status")
        availabilityTable.Columns.Add("Available After")

        For Each venue As String In venues
            Dim rows = bookedVenues.Select($"VenueLocation = '{venue}'")
            If rows.Length > 0 Then
                Dim endDateTime As DateTime = Convert.ToDateTime(rows(0)("UntilTime"))
                availabilityTable.Rows.Add(venue, "Unavailable", endDateTime.ToString("yyyy-MM-dd HH:mm"))
            Else
                availabilityTable.Rows.Add(venue, "Available", "-")
            End If
        Next

        dgvVenueAvailability.DataSource = availabilityTable

        ' Apply conditional formatting
        FormatAvailabilityGrid()
    End Sub

    Private Sub FormatAvailabilityGrid()
        ' Format the grid with colors and icons
        For Each row As DataGridViewRow In dgvVenueAvailability.Rows
            Dim status As String = row.Cells("Status").Value.ToString()

            If status = "Available" Then
                row.Cells("Status").Value = "✓ Available"
                row.DefaultCellStyle.BackColor = colorAvailable
            Else
                row.Cells("Status").Value = "✗ Unavailable"
                row.DefaultCellStyle.BackColor = colorUnavailable
            End If
        Next
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        LoadVenueAvailability()
    End Sub
End Class