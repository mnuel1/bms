Imports System.Data
Imports MySql.Data.MySqlClient

Public Class StaffVenueControl
    Inherits UserControl

    Private dgvVenueAvailability As DataGridView

    Public Sub New()
        InitializeComponent()
        SetupLayout()
        LoadVenueAvailability()
    End Sub

    Private Sub StaffVenueControl_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub SetupLayout()
        ' Venue Availability Grid
        dgvVenueAvailability = New DataGridView With {
            .Dock = DockStyle.Fill,
            .ReadOnly = True,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            .AllowUserToAddRows = False
        }

        ' Label
        Dim lblHeader As New Label With {
            .Text = "Venue Availability",
            .Dock = DockStyle.Top,
            .Font = New Font("Segoe UI", 12, FontStyle.Bold),
            .Height = 35
        }

        ' Layout
        Me.Controls.Add(dgvVenueAvailability)
        Me.Controls.Add(lblHeader)
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
        availabilityTable.Columns.Add("Availability")
        availabilityTable.Columns.Add("Unavailable Until")

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
    End Sub
End Class
