

/*
Question 1

a) DeliveryAddress is a struct (Value Type), so copying it creates an independent copy. Changing the copy does not affect the original.

b) Customer is a class (Reference Type), so both variables refer to the same object. Changing one affects the other.

Question 2

a) Problems: direct access to data, no validation, and uncontrolled modification.

b) Private fields with public properties protect the data and allow validation and controlled access.
*/

/*
using System;

public struct DeliveryAddress
{
    public string City { get; set; }
    public string Street { get; set; }
    public int BuildingNumber { get; set; }

    public DeliveryAddress(string city, string street, int buildingNumber)
    {
        City = city;
        Street = street;
        BuildingNumber = buildingNumber;
    }

    public string GetFullAddress()
    {
        return $"{City}, {Street}, Building {BuildingNumber}";
    }
}

public class Driver
{
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }

    public Driver(string fullName, string phoneNumber)
    {
        FullName = fullName;
        PhoneNumber = phoneNumber;
    }

    public void PrintDriver()
    {
        Console.WriteLine($"Driver Name: {FullName}");
        Console.WriteLine($"Phone: {PhoneNumber}");
    }
}

public abstract class Shipment
{
    private string trackingCode;
    private string description;
    private decimal weight;
    private decimal deliveryFee;

    public string TrackingCode
    {
        get { return trackingCode; }
    }

    public string Description
    {
        get { return description; }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                description = value;
        }
    }

    public decimal Weight
    {
        get { return weight; }
        set
        {
            if (value > 0)
                weight = value;
        }
    }

    public decimal DeliveryFee
    {
        get { return deliveryFee; }
        private set
        {
            if (value > 0)
                deliveryFee = value;
        }
    }

    public DeliveryAddress Destination { get; set; }

    public abstract decimal EstimatedCost { get; }

    public Shipment(string trackingCode)
    {
        trackingCode = trackingCode?.Trim();

        if (string.IsNullOrWhiteSpace(trackingCode))
            trackingCode = "UNKNOWN";

        this.trackingCode = trackingCode;
        description = "Unknown";
        weight = 1;
        deliveryFee = 50;
        Destination = new DeliveryAddress("Unknown", "Unknown", 0);
    }

    public Shipment(
        string trackingCode,
        string description,
        decimal weight,
        decimal deliveryFee,
        DeliveryAddress destination)
    {
        trackingCode = trackingCode?.Trim();

        if (string.IsNullOrWhiteSpace(trackingCode))
            trackingCode = "UNKNOWN";

        this.trackingCode = trackingCode;

        this.description = string.IsNullOrWhiteSpace(description)
            ? "Unknown"
            : description;

        this.weight = weight > 0 ? weight : 1;

        this.deliveryFee = deliveryFee > 0
            ? deliveryFee
            : 50;

        Destination = destination;
    }

    public void UpdateDeliveryFee(decimal newFee)
    {
        if (newFee > 0)
            DeliveryFee = newFee;
    }

    public abstract void PrintShipment();

    public virtual void UpdateWeight(int newWeight)
    {
        if (newWeight > 0)
            Weight = newWeight;
    }

    public virtual void UpdateWeight(decimal newWeight)
    {
        if (newWeight > 0)
            Weight = newWeight;
    }
}

public class StandardShipment : Shipment, ITrackable, IInsurable
{
    public StandardShipment(string trackingCode)
        : base(trackingCode)
    {
    }

    public StandardShipment(
        string trackingCode,
        string description,
        decimal weight,
        decimal deliveryFee,
        DeliveryAddress destination)
        : base(trackingCode, description, weight, deliveryFee, destination)
    {
    }

    public override decimal EstimatedCost
    {
        get
        {
            return DeliveryFee + (Weight * 5);
        }
    }

    public override void PrintShipment()
    {
        Console.WriteLine("Standard Shipment");
        Console.WriteLine($"Tracking Code: {TrackingCode}");
        Console.WriteLine($"Description: {Description}");
        Console.WriteLine($"Weight: {Weight}");
        Console.WriteLine($"Delivery Fee: {DeliveryFee}");
        Console.WriteLine($"Destination: {Destination.GetFullAddress()}");
        Console.WriteLine($"Estimated Cost: {EstimatedCost}");
    }

    public string GetTrackingStatus()
    {
        return "Ready";
    }

    public decimal CalculateInsurance()
    {
        return EstimatedCost * 0.05m;
    }
}

public class ExpressShipment : Shipment, ITrackable, IInsurable
{
    public decimal ExtraFee { get; set; }

    public ExpressShipment(
        string trackingCode,
        decimal extraFee)
        : base(trackingCode)
    {
        ExtraFee = extraFee >= 0 ? extraFee : 0;
    }

    public ExpressShipment(
        string trackingCode,
        string description,
        decimal weight,
        decimal deliveryFee,
        DeliveryAddress destination,
        decimal extraFee)
        : base(trackingCode, description, weight, deliveryFee, destination)
    {
        ExtraFee = extraFee >= 0 ? extraFee : 0;
    }

    public override decimal EstimatedCost
    {
        get
        {
            return DeliveryFee + (Weight * 5) + ExtraFee;
        }
    }

    public override void PrintShipment()
    {
        Console.WriteLine("Express Shipment");
        Console.WriteLine($"Tracking Code: {TrackingCode}");
        Console.WriteLine($"Description: {Description}");
        Console.WriteLine($"Weight: {Weight}");
        Console.WriteLine($"Delivery Fee: {DeliveryFee}");
        Console.WriteLine($"Extra Fee: {ExtraFee}");
        Console.WriteLine($"Destination: {Destination.GetFullAddress()}");
        Console.WriteLine($"Estimated Cost: {EstimatedCost}");
    }

    public string GetTrackingStatus()
    {
        return "Out for Delivery";
    }

    public decimal CalculateInsurance()
    {
        return EstimatedCost * 0.08m;
    }
}

public class InternationalShipment : Shipment, ITrackable, IInsurable
{
    public string DestinationCountry { get; set; }
    public decimal CustomsFee { get; set; }

    public InternationalShipment(
        string trackingCode,
        string destinationCountry,
        decimal customsFee)
        : base(trackingCode)
    {
        DestinationCountry = string.IsNullOrWhiteSpace(destinationCountry)
            ? "Unknown"
            : destinationCountry;

        CustomsFee = customsFee >= 0 ? customsFee : 0;
    }

    public InternationalShipment(
        string trackingCode,
        string description,
        decimal weight,
        decimal deliveryFee,
        DeliveryAddress destination,
        string destinationCountry,
        decimal customsFee)
        : base(trackingCode, description, weight, deliveryFee, destination)
    {
        DestinationCountry = string.IsNullOrWhiteSpace(destinationCountry)
            ? "Unknown"
            : destinationCountry;

        CustomsFee = customsFee >= 0 ? customsFee : 0;
    }

    public override decimal EstimatedCost
    {
        get
        {
            return DeliveryFee + (Weight * 5) + CustomsFee;
        }
    }

    public override void PrintShipment()
    {
        Console.WriteLine("International Shipment");
        Console.WriteLine($"Tracking Code: {TrackingCode}");
        Console.WriteLine($"Description: {Description}");
        Console.WriteLine($"Weight: {Weight}");
        Console.WriteLine($"Delivery Fee: {DeliveryFee}");
        Console.WriteLine($"Destination: {Destination.GetFullAddress()}");
        Console.WriteLine($"Destination Country: {DestinationCountry}");
        Console.WriteLine($"Customs Fee: {CustomsFee}");
        Console.WriteLine($"Estimated Cost: {EstimatedCost}");
    }

    public string GetTrackingStatus()
    {
        return "Delivered";
    }

    public decimal CalculateInsurance()
    {
        return EstimatedCost * 0.12m;
    }
    public virtual void GenerateCustomsReport()
    {
        Console.WriteLine($"Customs Report for {TrackingCode}");
        Console.WriteLine($"Country: {DestinationCountry}");
        Console.WriteLine($"Customs Fee: {CustomsFee}");
    }
}

public class PriorityInternationalShipment : InternationalShipment
{
    public PriorityInternationalShipment(
        string trackingCode,
        string description,
        decimal weight,
        decimal deliveryFee,
        DeliveryAddress destination,
        string destinationCountry,
        decimal customsFee)
        : base(
            trackingCode,
            description,
            weight,
            deliveryFee,
            destination,
            destinationCountry,
            customsFee)
    {
    }

    public override void GenerateCustomsReport()
    {
        Console.WriteLine($"Priority Customs Report for {TrackingCode}");
        Console.WriteLine($"Country: {DestinationCountry}");
        Console.WriteLine($"Customs Fee: {CustomsFee}");
        Console.WriteLine("Priority Shipment");
    }
}

public sealed class CompletedShipment : StandardShipment
{
    public CompletedShipment(
        string trackingCode,
        string description,
        decimal weight,
        decimal deliveryFee,
        DeliveryAddress destination)
        : base(
            trackingCode,
            description,
            weight,
            deliveryFee,
            destination)
    {
    }
}

public interface ITrackable
{
    string GetTrackingStatus();
}

public interface IInsurable
{
    decimal CalculateInsurance();
}

public class DeliveryReport
{
    public void PrintShipment(ITrackable shipment)
    {
        Console.WriteLine($"Tracking Status: {shipment.GetTrackingStatus()}");
    }

    public void PrintInsurance(IInsurable shipment)
    {
        Console.WriteLine($"Insurance Cost: {shipment.CalculateInsurance()}");
    }
}

public class DeliveryCenter
{
    public string CenterName { get; set; }

    private Shipment[] shipments = new Shipment[20];

    public DeliveryCenter(string centerName)
    {
        CenterName = centerName;
    }

    public Shipment this[int index]
    {
        get
        {
            if (index >= 0 && index < shipments.Length)
                return shipments[index];

            return null;
        }
        set
        {
            if (index >= 0 && index < shipments.Length)
                shipments[index] = value;
        }
    }

    public Shipment this[string trackingCode]
    {
        get
        {
            for (int i = 0; i < shipments.Length; i++)
            {
                if (shipments[i] != null &&
                    shipments[i].TrackingCode == trackingCode)
                {
                    return shipments[i];
                }
            }

            return null;
        }
    }

    public bool AddShipment(Shipment shipment)
    {
        if (shipment == null)
            return false;

        for (int i = 0; i < shipments.Length; i++)
        {
            if (shipments[i] == null)
            {
                shipments[i] = shipment;
                return true;
            }
        }

        return false;
    }

    public bool RemoveShipment(string trackingCode)
    {
        for (int i = 0; i < shipments.Length; i++)
        {
            if (shipments[i] != null &&
                shipments[i].TrackingCode == trackingCode)
            {
                shipments[i] = null;
                return true;
            }
        }

        return false;
    }

    public void PrintAllShipments()
    {
        for (int i = 0; i < shipments.Length; i++)
        {
            if (shipments[i] != null)
            {
                shipments[i].PrintShipment();
                Console.WriteLine();
            }
        }
    }
    public void PrintTrackingStatuses()
    {
        for (int i = 0; i < shipments.Length; i++)
        {
            if (shipments[i] is ITrackable trackable)
            {
                Console.WriteLine(
                    $"{shipments[i].TrackingCode}: {trackable.GetTrackingStatus()}");
            }
        }
    }
}

public static class DeliveryHelper
{
    public static void PrintShipmentDetails(Shipment shipment)
    {
        if (shipment != null)
            shipment.PrintShipment();
    }
}

public class Program
{
    public static void Main()
    {
        DeliveryAddress address1 =
            new DeliveryAddress("Cairo", "Nasr City", 10);

        DeliveryAddress address2 =
            new DeliveryAddress("Giza", "Dokki", 20);

        DeliveryAddress address3 =
            new DeliveryAddress("Cairo", "Heliopolis", 30);

        Driver driver =
            new Driver("Ahmed Mohamed", "01000000000");

        DeliveryCenter center =
            new DeliveryCenter("Main Delivery Center");

        StandardShipment standard =
            new StandardShipment(
                "SH001",
                "Laptop",
                3,
                80,
                address1);

        ExpressShipment express =
            new ExpressShipment(
                "SH002",
                "Mobile Phone",
                2,
                60,
                address2,
                30);

        InternationalShipment international =
            new InternationalShipment(
                "SH003",
                "Television",
                8,
                120,
                address3,
                "Germany",
                100);

        center.AddShipment(standard);
        center.AddShipment(express);
        center.AddShipment(international);

        Console.WriteLine("=== Driver ===");
        driver.PrintDriver();

        Console.WriteLine();
        Console.WriteLine("=== Shipment Details ===");

        center.PrintAllShipments();

        Console.WriteLine("=== Delivery Helper ===");
        DeliveryHelper.PrintShipmentDetails(standard);

        Console.WriteLine();
        Console.WriteLine("=== Update Weight ===");

        standard.UpdateWeight(4);
        express.UpdateWeight(2.5m);

        Console.WriteLine($"SH001 New Weight: {standard.Weight}");
        Console.WriteLine($"SH002 New Weight: {express.Weight}");

        Console.WriteLine();
        Console.WriteLine("=== Tracking Statuses ===");

        center.PrintTrackingStatuses();

        Console.WriteLine();
        Console.WriteLine("=== Delivery Reports ===");

        DeliveryReport report = new DeliveryReport();

        report.PrintShipment(standard);
        report.PrintShipment(express);
        report.PrintShipment(international);

        Console.WriteLine();
        report.PrintInsurance(standard);
        report.PrintInsurance(express);
        report.PrintInsurance(international);

        Console.WriteLine();
        Console.WriteLine("=== ITrackable Array ===");

        ITrackable[] trackableShipments =
        {
            standard,
            express,
            international
        };

        foreach (ITrackable shipment in trackableShipments)
        {
            Console.WriteLine(
                $"Status: {shipment.GetTrackingStatus()}");
        }

        Console.WriteLine();
        Console.WriteLine("=== IInsurable Array ===");

        IInsurable[] insurableShipments =
        {
            standard,
            express,
            international
        };

        foreach (IInsurable shipment in insurableShipments)
        {
            Console.WriteLine(
                $"Insurance: {shipment.CalculateInsurance()}");
        }

        Console.WriteLine();
        Console.WriteLine("=== Integer Indexer ===");

        Shipment firstShipment = center[0];

        if (firstShipment != null)
            firstShipment.PrintShipment();

        Console.WriteLine();
        Console.WriteLine("=== String Indexer ===");

        Shipment searchedShipment = center["SH002"];
        if (searchedShipment != null)
            searchedShipment.PrintShipment();
        else
            Console.WriteLine("Shipment not found.");

        Console.WriteLine();
        Console.WriteLine("=== Shipment Array Polymorphism ===");

        Shipment[] shipments =
        {
            standard,
            express,
            international
        };

        foreach (Shipment shipment in shipments)
        {
            shipment.PrintShipment();
            Console.WriteLine();
        }

        Console.WriteLine("=== Customs Report ===");

        international.GenerateCustomsReport();

        PriorityInternationalShipment priority =
            new PriorityInternationalShipment(
                "SH004",
                "Camera",
                5,
                100,
                address1,
                "France",
                70);

        priority.GenerateCustomsReport();

        Console.WriteLine();
        Console.WriteLine("=== Completed Shipment ===");

        CompletedShipment completed =
            new CompletedShipment(
                "SH005",
                "Tablet",
                2,
                70,
                address2);

        completed.PrintShipment();

        Console.WriteLine();
        Console.WriteLine("=== Delivery Address Struct Copy ===");

        DeliveryAddress copiedAddress = address1;

        copiedAddress.City = "Alexandria";
        copiedAddress.Street = "Smouha";
        copiedAddress.BuildingNumber = 50;

        Console.WriteLine($"Original: {address1.GetFullAddress()}");
        Console.WriteLine($"Copy: {copiedAddress.GetFullAddress()}");
    }
}
*/





